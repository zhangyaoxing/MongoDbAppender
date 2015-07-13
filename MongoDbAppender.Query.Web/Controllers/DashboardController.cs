using MongoDbAppender.Query.Dto;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MongoDbAppender.Query.Web.Controllers
{
    public class DashboardController : BasePageController
    {
        public int StatMinutes { get; set; }

        public ActionResult Index()
        {
            var repos = this.Overview.GetLogRepositories();
            ViewBag.Repositories = repos;
            ViewBag.RepositoryJson = JsonConvert.SerializeObject(
                repos,
                new JsonSerializerSettings()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    Formatting = Formatting.None
                });
            ViewBag.StatMinutes = this.StatMinutes;
            return View();
        }
    }
}
