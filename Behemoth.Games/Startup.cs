using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Behemoth.Games.Startup))]
namespace Behemoth.Games
{
    public partial class Startup 
    {
        public void Configuration(IAppBuilder app) 
        {
            ConfigureAuth(app);
        }
    }
}
