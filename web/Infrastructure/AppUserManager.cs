using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using DomainLayer.Identity;
using DomainLayer.DataAccess;


namespace web.Infrastructure
{
    public class AppUserManager : UserManager<AppUser> 
    {
        public AppUserManager(IUserStore<AppUser> store)
            : base(store)
        {
        }

        public static AppUserManager Create(IdentityFactoryOptions<AppUserManager> options, IOwinContext context)
        {
            IDbContext db = context.Get<IDbContext>();
            IDatabase database = db.GetDatabase();

            AppUserManager manager = new AppUserManager(new AppUserStore<AppUser>(database));

            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = true,
                RequireUppercase = true
            };

            manager.UserValidator = new UserValidator<AppUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = true,
            };

            return manager;
        }
    }
}