using MongoDbAppender.Query.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDbAppender.Query
{
    public interface IQueryConstants
    {

        int DefaultStatMinutes { get; }

        LogLevel DefaultLevel { get; }
    }
}
