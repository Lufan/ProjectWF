using System;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.AspNet.Identity.Owin;
using DomainLayer.Identity;
using DomainLayer.DataAccess;

namespace web.Infrastructure
{
    public sealed class AppRoleManager : RoleManager<AppUserRole>, IDisposable
    {
        public AppRoleManager(AppRoleStore<AppUserRole> store)
            : base(store)
        { }

        public static AppRoleManager Create(IdentityFactoryOptions<AppRoleManager> options, IOwinContext context)
        {
            return new AppRoleManager(new AppRoleStore<AppUserRole>(context.Get<IDbContext>().GetDatabase()));
        }
    }
}