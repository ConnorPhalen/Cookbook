using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(TechnicalProgrammingProject.Startup))]

namespace TechnicalProgrammingProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}