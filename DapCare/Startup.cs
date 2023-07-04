using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DapCare.Startup))]
namespace DapCare
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
