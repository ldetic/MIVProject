using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MIVProject.Startup))]
namespace MIVProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
