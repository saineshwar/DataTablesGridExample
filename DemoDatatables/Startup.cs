using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DemoDatatables.Startup))]
namespace DemoDatatables
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
