using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

using web.Infrastructure;
using DomainLayer.Identity;
using DomainLayer.DataAccess;

namespace web.App_Start
{
    public class IdentityConfig
    {
        public void Configuration(IAppBuilder app)
        {
            var context = (IIdentityDbContext)System.Web.Mvc.DependencyResolver.Current.GetService(typeof(IIdentityDbContext));

            app.CreatePerOwinContext<IDbContext>(context.Create);
            app.CreatePerOwinContext<AppUserManager>(AppUserManager.Create);
            app.CreatePerOwinContext<AppRoleManager>(AppRoleManager.Create);
            app.UseCookieAuthentication(
                new CookieAuthenticationOptions
                {
                    AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                    // TO DO: get path from config file
                    LoginPath = new PathString("/Account/Login"),
                });
        }
    }
}
