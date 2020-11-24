using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TeamAssignment.Startup))]
namespace TeamAssignment
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
