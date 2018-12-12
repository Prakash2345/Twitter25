using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EFProject.Startup))]
namespace EFProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
