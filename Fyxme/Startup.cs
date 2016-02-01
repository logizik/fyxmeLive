using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Fyxme.Startup))]
namespace Fyxme
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
