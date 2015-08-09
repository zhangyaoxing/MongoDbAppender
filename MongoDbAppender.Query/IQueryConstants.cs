using MongoDbAppender.Query.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDbAppender.Query
{
    public interface IQueryConstants
    {
        /// <summary>
        /// Default minitues to stat.
        /// </summary>
        int DefaultStatMinutes { get; }

        /// <summary>
        /// Default log level to view.
        /// </summary>
        LogLevel DefaultLevel { get; }

        /// <summary>
        /// Default page size if not specified.
        /// </summary>
        int DefaultPageSize { get; }

        /// <summary>
        /// Max page size. Specified page size must be less than this value.
        /// </summary>
        int MaxPageSize { get; }

        /// <summary>
        /// DateTime format.
        /// </summary>
        string DateTimeFormat { get; }
    }
}
