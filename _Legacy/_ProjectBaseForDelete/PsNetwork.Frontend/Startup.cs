using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PsNetwork.Frontend.Startup))]
namespace PsNetwork.Frontend
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
