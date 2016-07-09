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

        public async Task<string> Insert(TDocument doc)
        {
            await Task.Run(() => _collection.Add(doc));
            return doc.Id;
        }

        public async Task<long> Remove<TVal>(Expression<Func<TDocument, bool>> predicat)
        {
            return await Task.Run(() =>
            {
                long count = 0;
                while (true)
                {
                    var elem = _collection.FirstOrDefault(e => predicat.Compile()(e));
                    if (elem != null)
                    {
                        _collection.Remove(elem);
                        ++count;
                    }
                    else
                    {
                        break;
                    }
                }
                return count;
            });
        }

        public async Task<long> Update(TDocument doc)
        {
            return await Task.Run(() =>
            {
                long count = 0;
                for (int i = 0; i < _collection.Count; ++i)
                {
                    var elem = _collection.ElementAt(i);
                    if (elem.Id.Equals(doc.Id))
                    {
                        _collection.Remove(elem);
                        _collection.Insert(i, doc);
                        ++count;
                    }
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
                MemberExpression body = (MemberExpression)updateExpression.Body;
                string propertyName = body.Member.Name;
                long count = 0;
                for (int i = 0; i < _collection.Count; ++i)
                {
                    var elem = _collection.ElementAt(i);
                    if (predicat.Compile()(elem))
                    {
                        var prpInfo = elem.GetType().GetProperty(propertyName, typeof(TUVal));
                        if (prpInfo == null)
                        {
                            continue;
                        }
                        _collection.Remove(elem);
                        prpInfo.SetValue(elem, updateValue, null);
                        _collection.Insert(i, elem);
                        ++count;
                    }
                }
                return count;
            });
        }
    }
}