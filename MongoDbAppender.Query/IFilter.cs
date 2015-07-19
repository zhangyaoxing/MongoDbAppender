using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDbAppender.Query.Dto;

namespace MongoDbAppender.Query
{
    /// <summary>
    /// For log filtering.
    /// </summary>
    public interface IFilter
    {
        /// <summary>
        /// Filter start time.
        /// </summary>
        DateTime BeginAt { get; set; }

        /// <summary>
        /// Filter end time.
        /// </summary>
        DateTime EndAt { get; set; }

        /// <summary>
        /// Log levels
        /// </summary>
        IEnumerable<LogLevel> LogLevels { get; set; }

        /// <summary>
        /// Machine that logged this message.
        /// </summary>
        string MachineName { get; set; }

        /// <summary>
        /// Keyword used to do the full text search.
        /// </summary>
        string Keyword { get; set; }

        /// <summary>
        /// Page index. Based on 0.
        /// </summary>
        int PageIndex { get; set; }

        /// <summary>
        /// Page size.
        /// </summary>
        int PageSize { get; set; }
    }
}
