using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(BosalMontaze.Startup))]

namespace BosalMontaze
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
