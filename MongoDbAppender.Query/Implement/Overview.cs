using Log4net.Appender.MongoDb;
using MongoDbAppender.Query.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDbAppender.Query.Implement
{
    public class Overview : BaseMongoDbLogQuery, IOverview
    {
        public IList<Dto.LogRepositoryDto> GetLogRepositories()
        {
            var collNames = this.Database.GetCollectionNames();
            var repoNames = new List<string>();
            var repos = new List<LogRepositoryDto>();

            foreach(var collName in collNames)
            {
                //if (collName.StartsWith(Log4net.Appender.MongoDb.MongoDbAppender.COLLECTION_PREFIX))
                //{
                    repoNames.Add(collName);
                //}
            }

            foreach(var repoName in repoNames)
            {
                var repo = this.Database.GetCollection<LogEntry>(repoName);
                var count = repo.Count();
                repos.Add(new LogRepositoryDto()
                    {
                        Name = repoName,
                        EntryCount = count
                    });
            }

            return repos;
        }

        public string GetDatabaseName()
        {
            return this.Database.Name;
        }
    }
}
