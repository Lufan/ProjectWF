using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DomainLayer.DataAccess.Query
{
    /// <summary>
    /// Interface for read only access to the database.
    /// </summary>
    /// <typeparam name="TDocument">Document record type.</typeparam>
    public interface IDocumentQueryStore<TDocument>
    {
        Task<TDocument> FindByIdAsync(string documentId);

        Task<IEnumerable<TDocument>> FindByFieldAsync(
            Expression<Func<TDocument, object>> expression, 
            string pattern
            );

        IQueryable<TDocument> GetAsQueryable { get; }
    }
}