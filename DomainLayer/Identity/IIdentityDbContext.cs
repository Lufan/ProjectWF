
namespace DomainLayer.Identity
{
    public interface IIdentityDbContext
    {
        DataAccess.IDbContext Create();
    }
}
