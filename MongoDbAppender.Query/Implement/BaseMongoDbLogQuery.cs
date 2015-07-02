using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;
using Common.Logging;

namespace MongoDbAppender.Query.Implement
{
    /// <summary>
    /// MongoDb log query base class
    /// </summary>
    public class BaseMongoDbLogQuery
    {
        /// <summary>
        /// Default database
        /// </summary>
        public const string DEFAULT_DATABASE = "logs";

        /// <summary>
        /// Default collection prefix
        /// </summary>
        public const string COLLECTION_PREFIX = "logs_";

        /// <summary>
        /// Logger
        /// </summary>
        public ILog Logger { get; set; }

        /// <summary>
        /// MongoUrl. (Filled by Spring)
        /// </summary>
        public MongoUrl MongoUrl
        {
            get;
            set;
        }

        /// <summary>
        /// MongoClient. (Filled by Spring)
        /// </summary>
        public MongoClient MongoClient
        {
            get;
            set;
        }

        /// <summary>
        /// Get MongoDatabase
        /// </summary>
        public IMongoDatabase Database
        {
            get
            {
                return this.MongoClient.GetDatabase(this.MongoUrl.DatabaseName);
            }
        }
    }
}
