using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDbAppender.Query.Dto
{
    /// <summary>
    /// DTO to describe logs grouped by level.
    /// </summary>
    public class LevelCountDto
    {
        /// <summary>
        /// Log level
        /// </summary>
        public string Level { get; set; }

        /// <summary>
        /// Quantity of records of this level
        /// </summary>
        public long Count { get; set; }
    }
}
