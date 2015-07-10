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
        public int StatMinutes { get; set; }

        public IOverview Overview { get; set; }

        public IMonitor Monitor { get; set; }

        // GET api/repositories
        public IEnumerable<LogRepositoryDto> Get()
        {
            var repos = this.Overview.GetLogRepositories();
            return repos;
        }

        // GET api/repositories/id
        public dynamic Get(string id)
        {
            var stat = this.Monitor.GetStatistics(id, TimeSpan.FromMinutes(this.StatMinutes));
            var result = new
            {
                name = id,
                stat = stat
            };
            return result;
        }
    }
}
