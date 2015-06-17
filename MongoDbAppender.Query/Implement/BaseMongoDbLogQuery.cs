using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;

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
        /// Get MongoServer.
        /// </summary>
        public MongoServer MongoServer
        {
            get
            {
                return this.MongoClient.GetServer();
            }
        }

        /// <summary>
        /// Get MongoDatabase
        /// </summary>
        public MongoDatabase Database
        {
            get
            {
                return this.MongoServer.GetDatabase(this.MongoUrl.DatabaseName);
            }
        }

        /// <summary>
        /// Get mongo collection by name.
        /// </summary>
        /// <param name="collectionName">collection name</param>
        /// <typeparam name="T">Type of Log Entry</typeparam>
        /// <returns>MongoDb集合</returns>
        public MongoCollection GetMongoCollection(string collectionName)
        {
            var db = this.Database;
            return db.GetCollection(collectionName);
        }
    }
}
