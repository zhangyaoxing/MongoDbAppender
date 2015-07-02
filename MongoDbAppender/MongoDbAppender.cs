using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net.Appender;
using MongoDB.Driver;
using System.Configuration;

namespace Log4net.Appender.MongoDb
{
    public class MongoDbAppender : BufferingAppenderSkeleton
    {
        /// <summary>
        /// Add this prefix to the collection name by default.
        /// </summary>
        public const string COLLECTION_PREFIX = "logs_";

        /// <summary>
        /// Default database.
        /// </summary>
        public const string DEFAULT_DATABASE = "logs";

        /// <summary>
        /// database name
        /// </summary>
        private string dbName;

        /// <summary>
        /// full collection name. (prefix included)
        /// </summary>
        private string fullCollectionName;

        /// <summary>
        /// Is appender currently working normally
        /// </summary>
        private bool isWorking;

        /// <summary>
        /// MongoClinet for mongodb
        /// </summary>
        protected MongoClient client;

        #region Config properties
        /// <summary>
        /// The key of ConnectionStrings in web.config/app.config.
        /// </summary>
        public string ConnectionStringName { get; set; }

        /// <summary>
        /// Database name that will be used to store the logs. Defaults to "logs".
        /// </summary>
        public string DbName
        {
            get
            {
                return this.dbName;
            }
            set
            {
                this.dbName = value;
            }
        }

        /// <summary>
        /// The expected collection name that will be used to store the log data.
        /// Note COLLECTION_PREFIX will be added to this name.
        /// </summary>
        public string CollectionName { get; set; }
        #endregion


        /// <summary>
        /// Connection string for MongoDB. Get from ConnectionStringName.
        /// </summary>
        protected string ConnectionString { get; set; }

        /// <summary>
        /// Full collection name equals to COLLECTION_PREFIX + CollectionName.
        /// </summary>
        protected string FullCollectionName
        {
            get
            {
                if (string.IsNullOrEmpty(this.fullCollectionName))
                {
                    this.fullCollectionName = COLLECTION_PREFIX + this.CollectionName;
                }

                return this.fullCollectionName;
            }
        }

        /// <summary>
        /// Current machine name.
        /// </summary>
        protected string MachineName { get; set; }

        protected override bool RequiresLayout
        {
            get
            {
                return false;
            }
        }

        public MongoDbAppender()
        {
            this.dbName = DEFAULT_DATABASE;
            this.MachineName = System.Environment.MachineName;
            this.isWorking = true;
        }

        protected override void SendBuffer(log4net.Core.LoggingEvent[] events)
        {
            if (!this.isWorking)
            {
                return;
            }

            try
            {
                var db = this.client.GetDatabase(this.dbName);
                var collection = db.GetCollection<LogEntry>(this.FullCollectionName);
                var logs = new List<LogEntry>();
                foreach (var logEvent in events)
                {
                    var log = new LogEntry()
                    {
                        Timestamp = logEvent.TimeStamp,
                        Level = logEvent.Level.ToString(),
                        Thread = logEvent.ThreadName,
                        UserName = logEvent.UserName,
                        Message = logEvent.RenderedMessage,
                        LoggerName = logEvent.LoggerName,
                        Domain = logEvent.Domain,
                        MachineName = this.MachineName
                    };

                    if (logEvent.LocationInformation != null)
                    {
                        var locInfo = logEvent.LocationInformation;
                        log.FileName = locInfo.FileName ?? string.Empty;
                        log.Method = locInfo.MethodName ?? string.Empty;
                        log.LineNumber = locInfo.LineNumber ?? string.Empty;
                        log.ClassName = locInfo.ClassName ?? string.Empty;
                    }

                    if (logEvent.ExceptionObject != null)
                    {
                        log.Exception = this.Exception2Obj(logEvent.ExceptionObject);
                    }

                    logs.Add(log);
                }

                collection.InsertManyAsync(logs);
            }
            catch (Exception e)
            {
                ErrorHandler.Error("记录日志过程中发生错误。", e);
            }
        }

        public override void ActivateOptions()
        {
            base.ActivateOptions();
            this.InitMongo();
        }

        /// <summary>
        /// Init mongodb client.
        /// </summary>
        private void InitMongo()
        {
            // ConnectionStringName is required.
            if (string.IsNullOrWhiteSpace(this.ConnectionStringName))
            {
                ErrorHandler.Error("ConnectionStringName is required.");
                this.isWorking = false;
                return;
            }

            var connString = ConfigurationManager.ConnectionStrings[ConnectionStringName];
            if (connString == null || string.IsNullOrWhiteSpace(connString.ConnectionString))
            {
                ErrorHandler.Error(string.Format("The specified connection string key\"{0}\" can not be found in app.config or web.config.",
                    ConnectionStringName));
                this.isWorking = false;
                return;
            }

            this.ConnectionString = ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString;
            
            if (string.IsNullOrWhiteSpace(this.CollectionName))
            {
                ErrorHandler.Error("CollectionName is required.");
                this.isWorking = false;
                return;
            }

            MongoUrl url = new MongoUrl(this.ConnectionString);
            this.dbName = string.IsNullOrWhiteSpace(url.DatabaseName) ? DEFAULT_DATABASE : url.DatabaseName;
            this.client = new MongoClient(url);
        }

        private ExceptionEntry Exception2Obj(Exception ex)
        {
            var entry = new ExceptionEntry()
            {
                Name = ex.GetType().FullName,
                Message = ex.Message,
                Source = ex.Source,
                StackTrace = ex.StackTrace
            };

            entry.Data = new Dictionary<string, object>();
            for (var enu = ex.Data.GetEnumerator(); enu.MoveNext(); )
            {
                entry.Data[enu.Key.ToString()] = enu.Value;
            }

            if (ex.InnerException != null)
            {
                entry.InnerException = this.Exception2Obj(ex.InnerException);
            }

            return entry;
        }
    }
}
