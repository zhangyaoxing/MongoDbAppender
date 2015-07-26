using MongoDbAppender.Query.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MongoDbAppender.Query.Web.Models
{
    public class RepositoryModel
    {
        public string Name { get; set;}

        public IDictionary<LogLevel, long> Stat { get; set; }
    }
}