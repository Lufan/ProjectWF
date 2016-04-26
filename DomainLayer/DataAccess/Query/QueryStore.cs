using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace DomainLayer.DataAccess.Query
{
    /// <summary>
    /// Implementation of the interface IDocumentQueryStore<TDocument> 
    /// <see cref="IDocumentQueryStore<TDocument>"/>
    /// </summary>
    /// <typeparam name="TDocument">Document record type.</typeparam>
    public class QueryStore<TDocument> : IDocumentQueryStore<TDocument> where TDocument: class, IDocument
    {
        private readonly IDatabase _database;

        private readonly string _collectionName;

        private IDataTable<TDocument> _collection;

        private IDataTable<TDocument> GetCollection()
        {
            if (_collection == null)
            {
                this._collection = _database.GetCollection<TDocument>(_collectionName);
            }
            return this._collection;
        }

        public QueryStore(
            IDbContext context, 
            string collectionName
            )
        {
            if (context == null)
            {
                throw new ArgumentNullException("DbContext is null.");
            }

            if (string.IsNullOrEmpty(collectionName))
            {
                throw new ArgumentNullException("CollectionName is null.");
            }

            this._collectionName = collectionName;
            this._database = context.GetDatabase();
        }

        #region IDocumentQueryStore interface implementation
        /// <summary>
        /// Find record by ID value.
        /// </summary>
        /// <param name="documentId">Id field value of the document.</param>
        /// <returns>Task<TDocument> found record.</returns>
        public async Task<TDocument> FindByIdAsync(string documentId)
        {
            //if documentId is null do nothing
            if (documentId == null)
            {
                return null;
            }
            var doc = await this.GetCollection().FindOneById(documentId);
            return doc;
        }

        /// <summary>
        /// Find records coincidence pattern in the specified field.
        /// </summary>
        /// <param name="expression">The expression defining the search field.</param>
        /// <param name="pattern">The pattern to match (Symbol case will be ignored).</param>
        /// <returns>Task<TDocument> found records.</returns>
        public async Task<IEnumerable<TDocument>> FindByFieldAsync(
            System.Linq.Expressions.Expression<Func<TDocument, object>> expression, 
            string pattern
            )
        {
            var docs = await this.GetCollection().
                Find(expression, new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase));

            return docs;
        }

        /// <summary>
        /// Get database collection as queryable.
        /// </summary>
        public IQueryable<TDocument> GetAsQueryable
        {
            get
            {
                return this.GetCollection().GetCollection();
            }
        }
        #endregion IDocumentQueryStore interface implementation
    }
}