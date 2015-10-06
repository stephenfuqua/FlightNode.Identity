using FlightNode.Identity.Services.Providers;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(FligthNode.Identity.App.Startup))]

namespace FligthNode.Identity.App
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Don't like this hard-coded value
            var tokenUrl = "http://localhost:50323";

            app = ApiStartup.Configure(app, tokenUrl);
        }
        
        
    }
}