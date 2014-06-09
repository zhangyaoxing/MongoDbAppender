using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDbAppender.Query.Dto
{
    /// <summary>
    /// Log statistics
    /// </summary>
    public class StatisticsDto
    {
        /// <summary>
        /// Quantity of trace log
        /// </summary>
        public int TraceQty { get; set; }
        /// <summary>
        /// Quantity of debug log
        /// </summary>
        public int DebugQty { get; set; }
        /// <summary>
        /// Quantity of info log
        /// </summary>
        public int InfoQty { get; set; }
        /// <summary>
        /// Quantity of warn log
        /// </summary>
        public int WarnQty { get; set; }
        /// <summary>
        /// Quantity of error log
        /// </summary>
        public int ErrorQty { get; set; }
        /// <summary>
        /// Quantity of fatal log
        /// </summary>
        public int FatalQty { get; set; }
        /// <summary>
        /// Quantity of all log
        /// </summary>
        public int AllQty { get; set; }

    }
}
