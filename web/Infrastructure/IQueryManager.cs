using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DomainLayer.DataAccess;
using DomainLayer.DataAccess.Query;

namespace web.Infrastructure
{
    public interface IQueryManager<TDocument> where TDocument : IDocument
    {
        Task<TDocument> FindByIdAsync(string id);

        Task<IEnumerable<TDocument>> FindByNameAsync(Expression<Func<TDocument, object>> expression, string name);

        Task<IQueryable<TDocument>> TakeNAsync(int count, int startPosition);
    }
}