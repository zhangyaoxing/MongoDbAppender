using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net.Appender;
using MongoDB.Driver;

namespace MongoDbAppender
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
        /// machine name
        /// </summary>
        private string machineName;

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
        }

        protected override void SendBuffer(log4net.Core.LoggingEvent[] events)
        {
            throw new NotImplementedException();
        }
    }
}
