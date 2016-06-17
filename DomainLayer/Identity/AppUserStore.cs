using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using DomainLayer.DataAccess;

namespace DomainLayer.Identity
{
    public sealed class AppUserStore<TUser> : IAppUserStore<TUser>, IUserRoleStore<TUser> where TUser : AppUser
    {
        private readonly IDatabase _database;

        private readonly AppRoleStore<AppUserRole> _roleStore;

        private IDataTable<TUser> _collection;

        private IDataTable<TUser> GetUsersCollection()
        {
            if (_collection == null) _collection = _database.GetCollection<TUser>("Users");
            return _collection;
        }


        public AppUserStore(IDatabase database)
        {
            //database required
            if (database == null)
            {
                throw new ArgumentNullException("database is null.");
            }
            _database = database;
            _roleStore = new AppRoleStore<AppUserRole>(database);
        }

        #region IQueryableUserStore implementation
        public IQueryable<TUser> Users
        {
            get
            {
                return GetUsersCollection().GetCollection();
            }
        }
        #endregion

        #region IUserStore implementation
        public async Task CreateAsync(TUser user)
        {
            //user required for create action
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            await GetUsersCollection().Insert(user);
        }

        public async Task DeleteAsync(TUser user)
        {
            //if user is null do nothing
            if (user == null)
            {
                return;
            }

            await GetUsersCollection().Remove<TUser>(u => u.Id == user.Id);
        }

        public async Task<TUser> FindByIdAsync(string userId)
        {
            //if userId is null do nothing
            if (userId == null)
            {
                return null;
            }

            var user = await GetUsersCollection().FindOneById(userId);
            return user;
        }

        public async Task<TUser> FindByNameAsync(string userName)
        {
            //if userName is null do nothing
            if (userName == null)
            {
                return null;
            }

            var user = await GetUsersCollection().Find(u => u.UserName, userName);
            return user.FirstOrDefault();
        }

        public async Task UpdateAsync(TUser user)
        {
            //if user is null do nothing
            if (user == null)
            {
                return;
            }

            await GetUsersCollection().Update(user);
        }

        public void Dispose()
        {
            //do nothing
        }
        #endregion IUserStore implementation

        #region IUserPasswordStore implementation
        public async Task<string> GetPasswordHashAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("User is null.");
            }

            return await Task.FromResult(user.PasswordHash);
        }

        public async Task<bool> HasPasswordAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("User is null.");
            }

            return await Task.FromResult(user.PasswordHash != null && user.PasswordHash.Length > 0);
        }

        public async Task SetPasswordHashAsync(TUser user, string passwordHash)
        {
            if (user == null)
            {
                throw new ArgumentNullException("User is null.");
            }
            user.PasswordHash = passwordHash;
            await Task.FromResult(passwordHash);
        }
        #endregion IUserPasswordStore implementation

        #region IUserSecurityStampStore implementation
        public async Task<string> GetSecurityStampAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("User is null.");
            }
            return await Task.FromResult(user.SecurityStamp);
        }

        public async Task SetSecurityStampAsync(TUser user, string stamp)
        {
            if (user == null)
            {
                throw new ArgumentNullException("User is null.");
            }
            user.SecurityStamp = stamp;
            await Task.FromResult(stamp);
        }
        #endregion IUserSecurityStampStore implementation

        #region IAppUser custom data access: Roles
        public async Task<List<AppUserRole>> GetRolesAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("User is null.");
            }
            return await Task.FromResult(user.Roles);
        }

        public async Task AddRoleAsync(TUser user, AppUserRole role)
        {
            if (user == null)
            {
                throw new ArgumentNullException("User is null.");
            }
            if (role == null || user.Roles.FirstOrDefault(r => r.Name == role.Name) == role)
            {
                return;
            }

            user.Roles.Add(role);
            await GetUsersCollection().Update<IList<AppUserRole>>(u => u.Id == user.Id, u => u.Roles, user.Roles);
        }

        public async Task RemoveRoleAsync(TUser user, AppUserRole role)
        {
            if (user == null)
            {
                throw new ArgumentNullException("User is null.");
            }
            if (role == null || !user.Roles.Contains(role))
            {
                return;
            }

            user.Roles.Remove(role);
            await GetUsersCollection().Update<IList<AppUserRole>>(u => u.Id == user.Id, u => u.Roles, user.Roles);
        }

        public async Task RemoveAllRolesAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("User is null.");
            }
            if (user.Roles.Count == 0)
            {
                return;
            }
            user.Roles.RemoveAll(r => true);
            await GetUsersCollection().Update<IList<AppUserRole>>(u => u.Id == user.Id, u => u.Roles, user.Roles);
        }
        #endregion Roles

        #region IAppUser custom data access: ContactId
        public async Task SetContactId(TUser user, string contactId)
        {
            if (user == null)
            {
                throw new ArgumentNullException("User is null.");
            }
            if (contactId == null || contactId == user.ContactId)
            {
                return;
            }

            user.ContactId = contactId;
            await GetUsersCollection().Update<string>(u => u.Id == user.Id, u => u.ContactId, user.ContactId);
        }

        public async Task<string> GetContactId(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("User is null.");
            }
            return await Task.FromResult(user.ContactId);
        }
        #endregion ContactId

        #region IAppUser custom data access: AllowedProjectNumbers
        public async Task<List<string>> GetAllowedProjectNumbers(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("User is null.");
            }
            return await Task.FromResult(user.AllowedProjectNumbers);
        }

        public async Task AddAllowedProjectNumber(TUser user, string projectNumber)
        {
            if (user == null)
            {
                throw new ArgumentNullException("User is null.");
            }
            if (projectNumber == null || user.AllowedProjectNumbers.Contains(projectNumber))
            {
                return;
            }

            user.AllowedProjectNumbers.Add(projectNumber);
            await GetUsersCollection().Update<IList<string>>(u => u.Id == user.Id, u => u.AllowedProjectNumbers, user.AllowedProjectNumbers);
        }

        public async Task RemoveAllowedProjectNumber(TUser user, string projectNumber)
        {
            if (user == null)
            {
                throw new ArgumentNullException("User is null.");
            }
            if (projectNumber == null || !user.AllowedProjectNumbers.Contains(projectNumber))
            {
                return;
            }

            user.AllowedProjectNumbers.Remove(projectNumber);
            await GetUsersCollection().Update<IList<string>>(u => u.Id == user.Id, u => u.AllowedProjectNumbers, user.AllowedProjectNumbers);
        }
        #endregion AllowedProjectNumbers

        #region IUserRoleStore implementation
        public async Task AddToRoleAsync(TUser user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException("User is null.");
            }
            if (string.IsNullOrEmpty(roleName))
            {
                throw new ArgumentNullException("RoleName is null.");
            }

            if (!user.Roles.Exists(r => r.Name == roleName))
            {
                Task<AppUserRole> role = _roleStore.FindByNameAsync(roleName);
                role.Wait();
                if (role.Result != null)
                {
                    user.Roles.Add(role.Result);
                    await this.UpdateAsync(user);
                }
            }
            return;
        }

        async Task<IList<string>> IUserRoleStore<TUser, string>.GetRolesAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("User is null.");
            }

            return await Task.FromResult<IList<string>>(user.Roles.Select(r => r.Name).ToList());
        }

        public async Task<bool> IsInRoleAsync(TUser user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException("User is null.");
            }
            if (string.IsNullOrEmpty(roleName))
            {
                throw new ArgumentNullException("RoleName is null.");
            }

            return await Task.FromResult(user.Roles.Exists(r => r.Name == roleName));
        }

        public async Task RemoveFromRoleAsync(TUser user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException("User is null.");
            }
            if (string.IsNullOrEmpty(roleName))
            {
                throw new ArgumentNullException("RoleName is null.");
            }

            if (user.Roles.Exists(r => r.Name == roleName))
            {
                user.Roles.Remove(user.Roles.First(r => r.Name == roleName));
                await this.UpdateAsync(user);
            }
            return;
        }
        #endregion IUserRoleStore implementation
    }
}