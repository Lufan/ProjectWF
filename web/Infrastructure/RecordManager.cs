using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using DomainLayer.DataAccess;
using DomainLayer.DataAccess.Record;
using DomainLayer.Identity;

namespace web.Infrastructure
{
    public abstract class RecordManager<TDocument, TUser> : IRecordManager<TDocument, TUser> where TDocument : IDocument where TUser : IAppUser
    {
        private readonly IDocumentRecordStore<TDocument, TUser> _store;

        public RecordManager(IDocumentRecordStore<TDocument, TUser> store)
        {
            if (store == null)
            {
                throw new ArgumentNullException("store");
            }

            _store = store;
        }

        public Task<string> CreateAsync(TDocument document, TUser user)
        {
            if (document == null)
            {
                throw new ArgumentNullException("document");
            }
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return _store.CreateAsync(document, user);
        }

        public Task<long> DeleteAsync(TDocument document, TUser user)
        {
            if (document == null)
            {
                throw new ArgumentNullException("document");
            }
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return _store.DeleteAsync(document, user);
        }

        public Task UpdateAsync(TDocument document, TUser user)
        {
            if (document == null)
            {
                throw new ArgumentNullException("document");
            }
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return _store.UpdateAsync(document, user);
        }

        public Task UpdateAsync<TQVal, TUVal>(Expression<Func<TDocument, bool>> predicat,
                                              Expression<Func<TDocument, TUVal>> updateExpression,
                                              TUVal updateValue,
                                              TUser user
            )
        {
            if (predicat == null)
            {
                throw new ArgumentNullException("predicat");
            }
            if (updateExpression == null)
            {
                throw new ArgumentNullException("updateExpression");
            }
            if (updateValue == null)
            {
                throw new ArgumentNullException("updateValue");
            }
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return _store.UpdateAsync<TUVal>(predicat, updateExpression, updateValue, user);
        }
    }
}