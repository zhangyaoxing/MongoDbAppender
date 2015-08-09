using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MongoDbAppender.Query.Web.Models
{
    public class FilterResultModel
    {
        public int PageIndex { get; set; }
        public int PageCount { get; set; }
        public long EntryCount { get; set; }
        public IList<LogEntryModel> LogEntries { get; set; }
    }
}