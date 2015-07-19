using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MongoDbAppender.Query.Web.Controllers.API
{
    public class EntriesController : BaseApiController
    {
        // GET api/repositories/id/entries
        public HttpResponseMessage Get(HttpRequestMessage request, string id)
        {
            return null;
        }

        // POST api/entries
        public void Post([FromBody]string value)
        {
        }

        // DELETE api/entries/5
        public void Delete(int id)
        {
        }
    }
}
