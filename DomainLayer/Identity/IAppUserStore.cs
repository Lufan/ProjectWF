using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;

namespace DomainLayer.Identity
{
    public interface IAppUserStore<TUser> : IQueryableUserStore<TUser>, IUserStore<TUser>, IUserPasswordStore<TUser>, IUserSecurityStampStore<TUser> where TUser : AppUser
    {
        Task<List<AppUserRole>> GetRolesAsync(TUser user);
        Task AddRoleAsync(TUser user, AppUserRole role);
        Task RemoveRoleAsync(TUser user, AppUserRole role);
        Task RemoveAllRolesAsync(TUser user);

        Task SetContactId(TUser user, string contactId);
        Task<string> GetContactId(TUser user);

        Task<List<string>> GetAllowedProjectNumbers(TUser user);
        Task AddAllowedProjectNumber(TUser user, string projectNumber);
        Task RemoveAllowedProjectNumber(TUser user, string projectNumber);
    }
}