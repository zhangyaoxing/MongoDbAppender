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
        public ActionResult Index()
        {
            var repos = ViewBag.Repositories;
            ViewBag.RepositoryJson = JsonConvert.SerializeObject(
                repos,
                new JsonSerializerSettings()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    Formatting = Formatting.None
                });
            // TODO: stat minutes should be customizable.
            ViewBag.StatMinutes = this.QueryConstants.DefaultStatMinutes;
            throw new Exception();
            return View();
        }
    }
}
