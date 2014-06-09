using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDbAppender.Query.Dto
{
    /// <summary>
    /// Repository
    /// </summary>
    public class LogRepositoryDto
    {
        /// <summary>
        /// Name of repository.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Entry count.
        /// </summary>
        public long EntryCount { get; set; }
    }
}
