using MongoDbAppender.Query.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MongoDbAppender.Query.Web.Controllers.Ajax
{
    public class RepositoriesController : ApiController
    {
        public IOverview Overview { get; set; }

        // GET api/repositories
        public IEnumerable<LogRepositoryDto> Get()
        {
            var repos = this.Overview.GetLogRepositories();
            return repos;
        }

        // GET api/repositories/name
        public string Get(string name)
        {
            return "value";
        }
    }
}
