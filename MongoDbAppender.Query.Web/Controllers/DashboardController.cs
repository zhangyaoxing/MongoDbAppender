using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MongoDbAppender.Query.Web.Controllers
{
    public class DashboardController : Controller
    {
        public IOverview Overview { get; set; }
        //
        // GET: /Dashboard/

        public ActionResult Index()
        {
            this.Overview.GetLogRepositories();
            return View();
        }
    }
}
