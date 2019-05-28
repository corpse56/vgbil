using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PatronsBoard.Startup))]
namespace PatronsBoard
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
