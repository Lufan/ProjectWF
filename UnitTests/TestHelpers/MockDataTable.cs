using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DomainLayer.DataAccess;

namespace UnitTests.TestHelpers
{
    class MockDataTable<TDocument> : IDataTable<TDocument> where TDocument : class, IDocument
    {
        private IList<TDocument> _collection;

        public MockDataTable()
        {
            _collection = new List<TDocument>();
        }

        public async Task<IEnumerable<TDocument>> Find(
            Expression<Func<TDocument, object>> expression, 
            Regex regex)
        {
            return await Task.Run(() => _collection.Where(e => regex.IsMatch((string)expression.Compile()(e))));
        }

        public async Task<IEnumerable<TDocument>> Find<TVal>(
            Expression<Func<TDocument, TVal>> expression, 
            TVal value)
        {
            return await Task.Run(() => _collection.Where(e => expression.Compile()(e).Equals(value))
            );
        }

        public async Task<TDocument> FindOneById(string id)
        {
            var result = await this.Find<string>((doc) => doc.Id, id);
            return result.FirstOrDefault();
        }

        public IQueryable<TDocument> GetCollection()
        {
            return _collection.AsQueryable<TDocument>();
        }

        public async Task Insert(TDocument doc)
        {
            await Task.Run(() => _collection.Add(doc));
        }

        public async Task<long> Remove<TVal>(Expression<Func<TDocument, bool>> predicat)
        {
            return await Task.Run(() =>
            {
                var elements = _collection.Where(e => predicat.Compile()(e));
                long count = elements.Count();
                foreach (var elem in elements)
                {
                    _collection.Remove(elem);
                }
                return count;
            });
        }

        public async Task<long> Update(TDocument doc)
        {
            return await Task.Run(() =>
            {
                var elements = _collection.Where(e => e.Id.Equals(doc.Id));
                long count = elements.Count();
                foreach (var elem in elements)
                {
                    _collection.Remove(elem);
                    _collection.Add(doc);
                }
                return count;
            });
        }

        public async Task<long> Update<TUVal>(
            Expression<Func<TDocument, bool>> predicat, 
            Expression<Func<TDocument, TUVal>> updateExpression, 
            TUVal updateValue
            )
        {
            return await Task.Run(() =>
            {
                var elements = _collection.Where(e => predicat.Compile()(e));
                long count = elements.Count();
                MemberExpression body = (MemberExpression)updateExpression.Body;
                string propertyName = body.Member.Name;
                foreach (var elem in elements)
                {
                    //try
                    {
                        var prpInfo = elem.GetType().GetProperty(propertyName, typeof(TUVal));
                        if (prpInfo == null)
                        {
                            --count;
                            continue;
                        }
                        prpInfo.SetValue(elem, updateValue, null);
                    }
                    //catch (System.Reflection.AmbiguousMatchException e) { }
                    _collection.Remove(elem);
                    _collection.Add(elem);
                }
                return count;
            });
        }
    }
}