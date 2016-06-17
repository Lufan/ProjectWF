using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Identity
{
    public interface IIdentityDbContext
    {
        DataAccess.IDbContext Create();
    }
}
