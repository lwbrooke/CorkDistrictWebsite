using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CorkDistrict.Startup))]
namespace CorkDistrict
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
