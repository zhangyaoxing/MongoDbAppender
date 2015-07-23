using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDbAppender.Query.Dto;
using Log4net.Appender.MongoDb;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Bson;
using MongoQuery = MongoDB.Driver.Builders.Query;
using Spring.Context.Support;

namespace MongoDbAppender.Query.Implement
{
    /// <summary>
    /// Log detail.
    /// </summary>
    class Detail : BaseMongoDbLogQuery, IDetail
    {
        public IMonitor Monitor { get; set; }

        /// <summary>
        /// Get overall stats.
        /// </summary>
        /// <param name="reposName">Log repository name</param>
        /// <returns>stats</returns>
        public IDictionary<LogLevel, long> GetOverallStatistics(string reposName)
        {
            // TODO: 1000 should be configurable
            var result = this.Monitor.GetStatistics(reposName, TimeSpan.FromDays(1000));
            return result;
        }

        /// <summary>
        /// Filter log by filter.
        /// </summary>
        /// <param name="reposName">Log repository name</param>
        /// <param name="filter">filter</param>
        /// <returns>filtered result</returns>
        public FilterResultDto FilterLogs(string reposName, IFilter filter)
        {
            var db = base.Database;
            var collection = db.GetCollection(COLLECTION_PREFIX + reposName);
            IMongoQuery condition = Filter2Conditions(filter);

            var count = collection.Count(condition);
            var skip = filter.PageIndex * filter.PageSize;
            var limit = skip + filter.PageSize;
            if (skip > count)
            {
                filter.PageIndex = (int)Math.Ceiling(((decimal)count / (decimal)filter.PageSize)) - 1;
                skip = filter.PageIndex * filter.PageSize;
                limit = skip + filter.PageSize;
            }

            var all = collection.FindAs<LogEntry>(condition);
            all.SetSortOrder(SortBy.Descending("timestamp"));
            all.Limit = filter.PageSize + filter.PageIndex * filter.PageSize;
            all.Skip = filter.PageIndex * filter.PageSize;
            var logs = all.ToList<LogEntry>();

            return new FilterResultDto()
            {
                EntryCount = count,
                LogEntries = logs,
                PageCount = (int)Math.Ceiling(((decimal)count / (decimal)filter.PageSize)),
                PageIndex = filter.PageIndex
            };
        }


        /// <summary>
        /// Create a log filter
        /// </summary>
        /// <returns>filter</returns>
        public IFilter CreateFilter()
        {
            var context = ContextRegistry.GetContext();
            var filter = context.GetObject<IFilter>("Filter");
            
            return filter;
        }

        /// <summary>
        /// Generate MongoDB query condition based on filter.
        /// </summary>
        /// <param name="filter">filter</param>
        /// <returns>MongoDb query conditions</returns>
        private IMongoQuery Filter2Conditions(IFilter filter)
        {
            if (filter.PageIndex < 0)
            {
                var ex = new ArgumentException("PageIndex must be non-negative integer.", "PageIndex");
                this.Logger.Error("Wrong page index. Reset to default (0).", ex);
                filter.PageIndex = 0;
            }
            if (filter.PageSize <= 0)
            {
                var ex = new ArgumentException("PageSize must be positive integer.", "PageSize");
                this.Logger.Error("Wrong page size. Reset to default (30).", ex);
            }

            IList<IMongoQuery> conditions = new List<IMongoQuery>();
            IMongoQuery condition;
            if (!string.IsNullOrEmpty(filter.Keyword))
            {
                condition = MongoQuery.Text(filter.Keyword);
                conditions.Add(condition);
            }

            if (filter.BeginAt != DateTime.MinValue)
            {
                condition = Query<LogEntry>.GTE<DateTime>(entry => entry.Timestamp, filter.BeginAt);
                conditions.Add(condition);
            }

            if (filter.EndAt != DateTime.MaxValue)
            {
                condition = Query<LogEntry>.LTE<DateTime>(entry => entry.Timestamp, filter.EndAt);
                conditions.Add(condition);
            }

            if (filter.LogLevels.Count<LogLevel>(level => !Enum.IsDefined(typeof(LogLevel), level)) > 0)
            {
                var invalidLevel = filter.LogLevels.Where<LogLevel>(level => !Enum.IsDefined(typeof(LogLevel), level));
                var ex = new ArgumentException("Invalid LogLevels.", "LogLevels");
                this.Logger.Error(string.Format("Invalid log level: {0}.", string.Join<LogLevel>(", ", invalidLevel)), ex);
                filter.LogLevels = filter.LogLevels.Except<LogLevel>(invalidLevel);
            }
            var logLevelCount = filter.LogLevels.Count<LogLevel>();
            if (logLevelCount == 0)
            {
                this.Logger.Warn("No LogLevels defined. Defaults to LogLevel.All.");
                filter.LogLevels = new List<LogLevel>() { LogLevel.All };
            }
            if (filter.LogLevels.Contains<LogLevel>(LogLevel.All))
            {
                filter.LogLevels = new List<LogLevel>()
                {
                    LogLevel.Trace,
                    LogLevel.Debug,
                    LogLevel.Info,
                    LogLevel.Warn,
                    LogLevel.Error,
                    LogLevel.Fatal
                };
            }
            var levels = from level in filter.LogLevels
                         select level.ToString().ToUpper();
            condition = Query<LogEntry>.In<string>(entry => entry.Level, levels);
            conditions.Add(condition);

            if (!string.IsNullOrWhiteSpace(filter.MachineName))
            {
                condition = MongoQuery.EQ("machineName", new BsonString(filter.MachineName));
                conditions.Add(condition);
            }

            condition = MongoQuery.Null;
            for (var i = 0; i < conditions.Count; i++)
            {
                var c = conditions[i];
                if (i == 0)
                {
                    condition = c;
                }
                else
                {
                    condition = MongoQuery.And(condition, c);
                }
            }
            return condition;
        }
    }
}
