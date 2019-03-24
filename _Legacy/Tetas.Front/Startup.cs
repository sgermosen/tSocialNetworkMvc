using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Tetas.Front.Startup))]
namespace Tetas.Front
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
