
namespace DomainLayer.Contact
{
    public interface IContactDbContext
    {
        DataAccess.IDbContext Create();
    }
}
