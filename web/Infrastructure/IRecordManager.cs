
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DomainLayer.DataAccess;
using DomainLayer.Identity;

namespace web.Infrastructure
{
    public interface IRecordManager<TDocument, TUser> where TDocument : IDocument where TUser : IAppUser
    {
        Task CreateAsync(TDocument document, TUser user);

        Task<long> DeleteAsync(TDocument document, TUser user);

        Task UpdateAsync(TDocument document, TUser user);

        Task UpdateAsync<TQVal, TUVal>(Expression<Func<TDocument, bool>> predicat,
                                       Expression<Func<TDocument, TUVal>> updateExpression,
                                       TUVal updateValue,
                                       TUser user
        );
    }
}
