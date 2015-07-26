using MongoDbAppender.Query.Dto;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace MongoDbAppender.Query.Web.Controllers.API
{
    public class EntriesController : BaseApiController
    {
        //[ResponseType(typeof(FilterResultDto))]
        public FilterResultDto Get(
            string id, 
            string level = "", 
            string beginAt = "", 
            string endAt = "",
            string machineName = "",
            string keyword = "",
            int page = 1,
            int pageSize = 0)
        {
            var filter = this.Detail.CreateFilter();
            DateTime begin, end;
            if (!DateTime.TryParseExact(endAt,
                "yyyy-MM-dd HH:mm:ss",
                CultureInfo.InvariantCulture,
                DateTimeStyles.AdjustToUniversal,
                out end))
            {
                end = DateTime.UtcNow;
                begin = end.AddMinutes(-1 * this.QueryConstants.DefaultStatMinutes);
            }
            else if (!DateTime.TryParseExact(beginAt,
                "yyyy-MM-dd HH:mm:ss",
                CultureInfo.InvariantCulture,
                DateTimeStyles.AdjustToUniversal,
                out begin))
            {
                begin = end.AddMinutes(-1 * this.QueryConstants.DefaultStatMinutes);
            }
            filter.BeginAt = begin;
            filter.EndAt = end;

            if (!string.IsNullOrWhiteSpace(machineName))
            {
                filter.MachineName = machineName;
            }

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                filter.Keyword = keyword;
            }

            if (page >= 1)
            {
                filter.PageIndex = page - 1;
            }

            if (pageSize > 0)
            {
                filter.PageSize = pageSize;
            }
            else
            {
                filter.PageSize = this.QueryConstants.DefaultPageSize;
            }

            var logLevel = this.QueryConstants.DefaultLevel;
            if (Enum.TryParse<LogLevel>(level, true, out logLevel))
            {
                filter.LogLevels = new List<LogLevel>() { logLevel };
            }
            else
            {
                filter.LogLevels = new List<LogLevel>() { this.QueryConstants.DefaultLevel };
            }

            var result = this.Detail.FilterLogs(id, filter);
            //var response = request.CreateResponse<FilterResultDto>(HttpStatusCode.OK, result);

            return result;
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
