using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDbAppender.Query.Dto;

namespace MongoDbAppender.Query
{
    public interface IMonitor
    {
        IDictionary<LogLevel, LevelCountDto> GetStatistics(string repoName, TimeSpan timeSpan);
    }
}
