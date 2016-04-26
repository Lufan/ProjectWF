using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DomainLayer.DataAccess.Record
{
    /// <summary>
    /// Interface for write only access to the database.
    /// </summary>
    /// <typeparam name="TDocument">Document record type.</typeparam>
    /// <typeparam name="TUser">User record type</typeparam>
    public interface IDocumentRecordStore<TDocument, TUser>
    {
        Task CreateAsync(
            TDocument document, 
            TUser user
            );

        Task<long> DeleteAsync(
            TDocument document, 
            TUser user
            );

        Task<long> UpdateAsync(
            TDocument document, 
            TUser user
            );

        Task<long> UpdateAsync<TQVal, TUVal>(
            Expression<Func<TDocument, bool>> predicat,
            Expression<Func<TDocument, TUVal>> updateExpression,
            TUVal updateValue,
            TUser user
            );
    }
}