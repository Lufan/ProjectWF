using Microsoft.AspNet.Identity;
using System.Collections.Generic;

namespace DomainLayer.Identity
{
    public interface IAppUser : IUser
    {
        string PasswordHash { get; set; }
        string SecurityStamp { get; set; }
        List<AppUserRole> Roles { get; }
        string ContactId { get; }
        List<string> AllowedProjectNumbers { get; set; }
    }
}