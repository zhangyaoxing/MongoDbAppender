using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MongoDbAppender
{
    /// <summary>
    /// Describes a log entity.
    /// </summary>
    [BsonIgnoreExtraElements(true)]
    public class LogEntry
    {
        /// <summary>
        /// Record ID.
        /// </summary>
        public ObjectId Id { get; set; }

        /// <summary>
        /// Time of generated.
        /// </summary>
        [BsonElement("timestamp")]
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Log level. Represents the log4net log level.
        /// </summary>
        [BsonElement("level")]
        public string Level { get; set; }

        /// <summary>
        /// Thread generates the entry
        /// </summary>
        [BsonElement("thread")]
        public string Thread { get; set; }

        /// <summary>
        /// Username of the one runs the application domain
        /// </summary>
        [BsonElement("userName")]
        public string UserName { get; set; }

        /// <summary>
        /// Log message.
        /// </summary>
        [BsonElement("message")]
        public string Message { get; set; }

        /// <summary>
        /// Logger name.
        /// </summary>
        [BsonElement("loggerName")]
        public string LoggerName { get; set; }

        /// <summary>
        /// Domain that generates the log.
        /// </summary>
        [BsonElement("domain")]
        public string Domain { get; set; }

        /// <summary>
        /// Machine that generates the log.
        /// </summary>
        [BsonElement("machineName")]
        public string MachineName { get; set; }

        /// <summary>
        /// Source code file name.
        /// </summary>
        [BsonElement("fileName")]
        public string FileName { get; set; }

        /// <summary>
        /// Methods that generats the log.
        /// </summary>
        [BsonElement("method")]
        public string Method { get; set; }

        /// <summary>
        /// Line number.
        /// </summary>
        [BsonElement("lineNumber")]
        public string LineNumber { get; set; }

        /// <summary>
        /// Class that generates the log.
        /// </summary>
        [BsonElement("className")]
        public string ClassName { get; set; }

        /// <summary>
        /// Data that attached to exception.
        /// Represents the Data property of Exception class.
        /// </summary>
        [BsonElement("data")]
        public IDictionary<string, object> Data { get; set; }

        /// <summary>
        /// The exception.
        /// </summary>
        [BsonElement("exception")]
        public ExceptionEntry Exception { get; set; }

        /// <summary>
        /// Exception
        /// </summary>
        [BsonIgnoreExtraElements(true)]
        public class ExceptionEntry
        {
            /// <summary>
            /// Name of the exception. 
            /// </summary>
            [BsonElement("name")]
            public string Name { get; set; }

            /// <summary>
            /// Error message.
            /// </summary>
            [BsonElement("message")]
            public string Message { get; set; }

            /// <summary>
            /// Source.
            /// </summary>
            [BsonElement("source")]
            public string Source { get; set; }

            /// <summary>
            /// Stacktrace
            /// </summary>
            [BsonElement("stackTrace")]
            public string StackTrace { get; set; }

            /// <summary>
            /// Inner exception (if any, otherwise null).
            /// </summary>
            [BsonElement("innerException")]
            public ExceptionEntry InnerException { get; set; }
        }
    }
}
