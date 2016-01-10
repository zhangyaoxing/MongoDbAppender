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
    public class RepositoriesController : BasePageController
    {
        public int StatMinutes { get; set; }

        public ActionResult Index(string name, string level)
        {
            var activeLevel = LogLevel.All;
            Enum.TryParse<LogLevel>(level, true, out activeLevel);

            ViewBag.ActiveLevel = activeLevel;
            ViewBag.Name = name;

            return View();
        }
	}
}