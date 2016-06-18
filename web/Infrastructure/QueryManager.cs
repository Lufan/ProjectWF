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
    public abstract class QueryManager<TDocument> : IQueryManager<TDocument> where TDocument : IDocument
    {
        private readonly IDocumentQueryStore<TDocument> _store;

        public QueryManager(IDocumentQueryStore<TDocument> store)
        {
            if (store == null)
            {
                throw new ArgumentNullException("store");
            }
            _store = store;
        }

        public Task<TDocument> FindByIdAsync(string id)
        {
            //If id is null or empty, do nothing
            if (string.IsNullOrEmpty(id))
            {
                return null;
            }
            return _store.FindByIdAsync(id);
        }

        public Task<IEnumerable<TDocument>> FindByNameAsync(Expression<Func<TDocument, object>> expression, string name)
        {
            //If name is null or empty, do nothing
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }
            return _store.FindByFieldAsync(expression, name);
        }

        public Task<IQueryable<TDocument>> TakeNAsync(int count, int startPosition = 0)
        {
            //If count is 0, do nothing
            if (count == 0)
            {
                return null;
            }

            return Task.FromResult(_store.GetAsQueryable.Skip(startPosition).Take(count));
        }
    }
}