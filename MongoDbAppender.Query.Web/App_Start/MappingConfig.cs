using AutoMapper;
using Log4net.Appender.MongoDb;
using MongoDB.Bson;
using MongoDbAppender.Query.Dto;
using MongoDbAppender.Query.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MongoDbAppender.Query.Web.App_Start
{
    public class MappingConfig
    {
        public static void RegisterMappings()
        {
            Mapper.CreateMap<string, LogLevel>().ConvertUsing((str) => {
                LogLevel level = LogLevel.All;
                Enum.TryParse<LogLevel>(str, true, out level);

                return level;
            });
            Mapper.CreateMap<LogLevel, string>().ConvertUsing(level => level.ToString());
            Mapper.CreateMap<ObjectId, string>().ConvertUsing(id => id.ToString());

            Mapper.CreateMap<LogEntry, LogEntryModel>();
            Mapper.CreateMap<FilterResultDto, FilterResultModel>();
            Mapper.CreateMap<ExceptionEntry, ExceptionModel>();
        }
    }
}