﻿using AutoMapper;
using MongoDbAppender.Query.Dto;
using MongoDbAppender.Query.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace MongoDbAppender.Query.Web.Controllers.API
{
    public class RepositoriesController : BaseApiController
    {
        // GET api/repositories
        [ResponseType(typeof(IEnumerable<LogRepositoryDto>))]
        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            var repos = this.Overview.GetLogRepositories();
            var model = Mapper.Map<IList<RepositoryBasicModel>>(repos);
            return request.CreateResponse <IEnumerable<RepositoryBasicModel>>(HttpStatusCode.OK, model);
        }

        // GET api/repositories/id
        public HttpResponseMessage Get(HttpRequestMessage request, string id, string statMins = "")
        {
            int mins;
            if (!int.TryParse(statMins, out mins))
            {
                mins = this.QueryConstants.DefaultStatMinutes;
            }
            var stat = this.Monitor.GetStatistics(id, TimeSpan.FromMinutes(mins));
            var model = new RepositoryModel()
            {
                Name = id,
                Stat = stat
            };

            return request.CreateResponse(HttpStatusCode.OK, model);
        }
    }
}
