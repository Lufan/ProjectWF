using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DomainLayer.DataAccess
{
    public interface IDataTable<TDocument>
    {
        Task<string> Insert(TDocument doc);

        IQueryable<TDocument> GetCollection();

        Task<long> Remove<TVal>(
            Expression<Func<TDocument, bool>> predicat
            );

        Task<TDocument> FindOneById(string id);

        Task<IEnumerable<TDocument>> Find<TVal>(
            Expression<Func<TDocument, TVal>> expression,
            TVal value
            );

        Task<IEnumerable<TDocument>> Find(
            Expression<Func<TDocument, object>> expression,
            System.Text.RegularExpressions.Regex regex
            );

        Task<long> Update(TDocument doc);

        Task<long> Update<TUVal>(
            Expression<Func<TDocument, bool>> predicat,
            Expression<Func<TDocument, TUVal>> updateExpression, 
            TUVal updateValue
            );
    }
}