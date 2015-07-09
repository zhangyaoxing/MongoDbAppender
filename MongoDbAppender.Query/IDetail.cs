using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDbAppender.Query.Dto;
using Log4net.Appender.MongoDb;

namespace MongoDbAppender.Query
{
    /// <summary>
    /// Get detailed log message.
    /// </summary>
    public interface IDetail
    {
        /// <summary>
        /// Get overall statistics based on repository name.
        /// </summary>
        /// <param name="reposName">Repository name</param>
        /// <returns>Statistics</returns>
        IDictionary<LogLevel, LevelCountDto> GetOverallStatistics(string reposName);

        /// <summary>
        /// Filter logs with filter.
        /// </summary>
        /// <param name="reposName">Repository name</param>
        /// <param name="filter">Filter</param>
        /// <returns>Filtered result</returns>
        FilterResultDto FilterLogs(string reposName, IFilter filter);

        /// <summary>
        /// Create a filter
        /// </summary>
        /// <returns>Filter</returns>
        IFilter CreateFilter();
    }
}
