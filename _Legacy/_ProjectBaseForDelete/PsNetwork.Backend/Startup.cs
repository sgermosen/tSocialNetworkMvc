using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PsNetwork.Backend.Startup))]
namespace PsNetwork.Backend
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
