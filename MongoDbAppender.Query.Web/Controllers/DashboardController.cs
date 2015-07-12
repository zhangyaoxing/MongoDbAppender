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
        //
        // GET: /Dashboard/
        public ActionResult Index()
        {
            var repos = this.Overview.GetLogRepositories();
            //var repoStats = new List<IDictionary<LogLevel, long>>();
            //foreach(var repo in repos)
            //{
            //    var stat = this.Monitor.GetStatistics(repo.Name, TimeSpan.FromMinutes(this.StatMinutes));
            //    repoStats.Add(stat);
            //}

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
