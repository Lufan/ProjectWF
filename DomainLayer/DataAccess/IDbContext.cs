using System;

namespace DomainLayer.DataAccess
{
    public interface IDbContext : IDisposable
    {
        IDatabase GetDatabase();
    }
}