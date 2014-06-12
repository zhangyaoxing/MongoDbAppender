using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDbAppender.Query.Dto;

namespace MongoDbAppender.Query.Implement
{
    class Filter : IFilter
    {
        public Filter()
        {
            this.BeginAt = DateTime.MinValue;
            this.EndAt = DateTime.MaxValue;
            this.LogLevel = new List<LogLevel>();
        }
        public DateTime BeginAt { get; set; }

        public DateTime EndAt { get; set; }

        public IEnumerable<LogLevel> LogLevel { get; set; }

        public string MachineName { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
