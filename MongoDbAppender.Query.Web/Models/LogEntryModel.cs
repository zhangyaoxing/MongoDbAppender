using MongoDbAppender.Query.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MongoDbAppender.Query.Web.Models
{
    public class LogEntryModel
    {
        /// <summary>
        /// Record ID.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Time of generated.
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Log level. Represents the log4net log level.
        /// </summary>
        public LogLevel Level { get; set; }

        /// <summary>
        /// Thread generates the entry
        /// </summary>
        public string Thread { get; set; }

        /// <summary>
        /// Username of the one runs the application domain
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Log message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Logger name.
        /// </summary>
        public string LoggerName { get; set; }

        /// <summary>
        /// Domain that generates the log.
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// Machine that generates the log.
        /// </summary>
        public string MachineName { get; set; }

        /// <summary>
        /// Source code file name.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Methods that generats the log.
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// Line number.
        /// </summary>
        public string LineNumber { get; set; }

        /// <summary>
        /// Class that generates the log.
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// The exception.
        /// </summary>
        public ExceptionModel Exception { get; set; }
    }

    public class ExceptionModel
    {
        /// <summary>
        /// Name of the exception. 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Error message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Data that attached to exception.
        /// Represents the Data property of Exception class.
        /// </summary>
        public IDictionary<string, object> Data { get; set; }

        /// <summary>
        /// Source.
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// Stacktrace
        /// </summary>
        public string StackTrace { get; set; }

        /// <summary>
        /// Inner exception (if any, otherwise null).
        /// </summary>
        public ExceptionModel InnerException { get; set; }
    }
}