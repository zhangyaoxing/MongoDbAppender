using Log4net.Appender.MongoDb;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDbAppender.Query.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Appender = Log4net.Appender.MongoDb.MongoDbAppender;

namespace MongoDbAppender.Query.Implement
{
    public class Monitor : BaseMongoDbLogQuery, IMonitor
    {
        public IOverview Overview { get; set; }

        public IDictionary<LogLevel, LevelCountDto> GetStatistics(string repoName, TimeSpan timeSpan)
        {
            var fullRepoName = Appender.COLLECTION_PREFIX + repoName;
            var coll = this.Database.GetCollection<LogEntry>(fullRepoName);
            var endTime = DateTime.UtcNow;
            var startTime = DateTime.UtcNow - timeSpan;
            var pipelines = new BsonDocument[]
            {
                new BsonDocument {{"$match", new BsonDocument {{"timestamp", new BsonDocument {{"$gt", startTime}, {"$lt", endTime}}}}}},
                new BsonDocument {{"$group", new BsonDocument {{"_id", "$level"}, {"count", new BsonDocument{{"$sum", 1}}}}}},
                new BsonDocument {{"$project", new BsonDocument {{"Level", "$_id"}, {"Count", "$count"}, {"_id", 0}}}}
            };
            var bsonResult = coll.Aggregate(new AggregateArgs
            {
                Pipeline = pipelines
            });

            var stats = from bson in bsonResult
                        select BsonSerializer.Deserialize<LevelCountDto>(bson);
            var result = stats.ToDictionary<LevelCountDto, LogLevel>(
                (item) => {
                    LogLevel level;
                    if (!Enum.TryParse<LogLevel>(item.Level, true, out level))
                    {
                        this.Logger.Error(string.Format("Unrecognizable level string: {0}.", item.Level));
                        level = LogLevel.Error;
                    }

                    return level;
                });

            return result;
        }
    }
}
