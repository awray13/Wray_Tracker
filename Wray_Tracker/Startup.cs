using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Wray_Tracker.Startup))]
namespace Wray_Tracker
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
