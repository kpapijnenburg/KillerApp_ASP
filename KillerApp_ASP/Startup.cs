using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(KillerApp_ASP.Startup))]
namespace KillerApp_ASP
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
