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
            ViewBag.Name = name;
            ViewBag.StatMinutes = this.StatMinutes;

            return View();
        }
	}
}