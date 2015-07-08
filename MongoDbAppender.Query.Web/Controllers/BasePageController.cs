using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MongoDbAppender.Query.Web.Controllers
{
    public abstract class BasePageController : Controller
    {
        public IOverview Overview { get; set; }

        public IDetail Detail { get; set; }

        public IMonitor Monitor { get; set; }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
        }
	}
}