using BattleSea;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(HubStartup))]
namespace BattleSea
{
    public class HubStartup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}