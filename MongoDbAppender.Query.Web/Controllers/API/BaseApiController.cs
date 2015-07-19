using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MongoDbAppender.Query.Web.Controllers.API
{
    public abstract class BaseApiController : ApiController
    {
        public IOverview Overview { get; set; }

        public IMonitor Monitor { get; set; }

        public IDetail Detail { get; set; }

        public IQueryConstants QueryConstants { get; set; }
    }
}
