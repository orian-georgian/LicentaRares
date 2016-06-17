using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(License.HostAsp.Startup))]

namespace License.HostAsp
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseNancy();
        }
    }
}
