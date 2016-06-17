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
    public sealed class AppRoleStore<TRole> : IQueryableRoleStore<TRole> where TRole : AppUserRole, IDocument
    {
        private readonly IDatabase _database;

        private IDataTable<TRole> _collection;

        private IDataTable<TRole> GetRolesCollection()
        {
            // TO DO: get collection (table) name from config file
            if (_collection == null) _collection = _database.GetCollection<TRole>("Roles");
            return _collection;
        }

        public AppRoleStore(IDatabase database)
        {
            if (database == null)
            {
                throw new ArgumentNullException("database");
            }

            _database = database;
        }

        #region IQuerayableRoleStore interface implemented
        public IQueryable<TRole> Roles
        {
            get
            {
                return GetRolesCollection().GetCollection();
            }
        }
        #endregion IQuerayableRoleStore interface implemented

        #region IRoleStore interface implementation
        public async Task CreateAsync(TRole role)
        {
            //user required for create action
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }
            await GetRolesCollection().Insert(role);
        }

        public async Task DeleteAsync(TRole role)
        {
            //if role is null do nothing
            if (role == null)
            {
                return;
            }

            await GetRolesCollection().Remove<TRole>(r => r.Id == role.Id);
        }

        public async Task<TRole> FindByIdAsync(string roleId)
        {
            //if roleId is null do nothing
            if (roleId == null)
            {
                return null;
            }

            var role = await GetRolesCollection().FindOneById(roleId);
            return role;
        }

        public async Task<TRole> FindByNameAsync(string roleName)
        {
            //if roleName is null do nothing
            if (roleName == null)
            {
                return null;
            }

            var role = await GetRolesCollection().Find(r => r.Name, roleName);
            return role.FirstOrDefault();
        }

        public async Task UpdateAsync(TRole role)
        {
            //if role is null do nothing
            if (role == null)
            {
                return;
            }

            await GetRolesCollection().Update(role);
            return;
        }

        public void Dispose()
        {
            //do nothing
        }
        #endregion  IRoleStore interface implementation
    }
}