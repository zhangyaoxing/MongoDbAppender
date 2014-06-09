using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Log4net.Appender.MongoDb;

namespace MongoDbAppender.Query.Dto
{
    public class FilterResultDto
    {
        public int PageIndex { get; set; }
        public int PageCount { get; set; }
        public long EntryCount { get; set; }
        public IList<LogEntry> LogEntries { get; set; }
    }
}
