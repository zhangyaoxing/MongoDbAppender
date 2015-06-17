using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MongoDbAppender.Query.Web.Startup))]
namespace MongoDbAppender.Query.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
