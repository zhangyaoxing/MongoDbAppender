using MongoDbAppender.Query.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDbAppender.Query.Implement
{
    public class QueryConstants : IQueryConstants
    {
        public int DefaultStatMinutes { get; set; }

        public LogLevel DefaultLevel { get; set; }

        public int DefaultPageSize { get; set; }

        public int MaxPageSize { get; set; }

        public string DateTimeFormat { get; set; }
    }
}
