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
        public IEnumerable<LevelCountDto> GetOverallStatistics(string reposName)
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
            return new Filter();
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
                throw new ArgumentException("PageIndex must be non-negative integer.", "PageIndex");
            }
            if (filter.PageSize <= 0)
            {
                throw new ArgumentException("PageSize must be positive integer.", "PageSize");
            }

            IList<IMongoQuery> conditions = new List<IMongoQuery>();
            IMongoQuery condition;
            if (filter.BeginAt != DateTime.MinValue)
            {
                condition = MongoQuery.GTE("timestamp", new BsonDateTime(filter.BeginAt));
                conditions.Add(condition);
            }
            if (filter.EndAt != DateTime.MaxValue)
            {
                condition = MongoQuery.LTE("timestamp", new BsonDateTime(filter.EndAt));
                conditions.Add(condition);
            }
            if (filter.LogLevel.Count<LogLevel>() > 0)
            {
                condition = MongoQuery.EQ("level", new BsonString(filter.LogLevel.ToString().ToUpper()));
                conditions.Add(condition);
            }
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
