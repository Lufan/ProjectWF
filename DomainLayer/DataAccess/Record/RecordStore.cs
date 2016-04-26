using System;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace DomainLayer.DataAccess.Record
{
    /// <summary>
    /// Implementation of the interface IDocumentRecordStore<TDocument, TUser> 
    /// <see cref="IDocumentRecordStore<TDocument, TUser>"/>
    /// </summary>
    /// <typeparam name="TDocument">Document record type.</typeparam>
    /// <typeparam name="TUser">User record type</typeparam>
    public class RecordStore<TDocument, TUser> : 
        IDocumentRecordStore<TDocument, TUser> where TDocument : class, IDocument
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

        public RecordStore(
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

        #region IDocumentRecordStore implementation
        /// <summary>
        /// Create database record.
        /// </summary>
        /// <param name="document">The document for the creation.</param>
        /// <param name="user">The user carries out the current request.</param>
        /// <returns>
        /// Task<bool> representing the status of the operation: 
        /// true - success, false - fails.
        /// </returns>
        public async Task CreateAsync(
            TDocument document, 
            TUser user
            )
        {
            //document required for create action
            if (document == null)
            {
                throw new ArgumentNullException("Document is null.");
            }
            //user required for create action
            if (user == null)
            {
                throw new ArgumentNullException("User is null.");
            }
            await this.GetCollection().Insert(document);
        }

        /// <summary>
        /// Delete database record.
        /// </summary>
        /// <param name="document">The document to be deleted.</param>
        /// <param name="user">The user carries out the current request.</param>
        /// <returns>
        /// Task<bool> representing the status of the operation: 
        /// true - success, false - fails.
        /// </returns>
        public async Task<long> DeleteAsync(
            TDocument document, 
            TUser user
            )
        {
            //if document is null do nothing
            if (document == null)
            {
                //TODO: log warning into logstore
                return 0;
            }
            //if user is null do nothing
            if (user == null)
            {
                //TODO: log warning into logstore
                return 0;
            }
            var count = await this.GetCollection().Remove<TDocument>(obj => obj.Id == document.Id);
            return count;
        }

        /// <summary>
        /// Update database record.
        /// </summary>
        /// <param name="document">The document for the update.</param>
        /// <param name="user">The user carries out the current request.</param>
        /// <returns>
        /// Task<bool> representing the status of the operation: 
        /// true - success, false - fails.
        /// </returns>
        public async Task<long> UpdateAsync(
            TDocument document, 
            TUser user
            )
        {
            //if document is null do nothing
            if (document == null)
            {
                //TODO: log warning into logstore
                return 0;
            }
            //if user is null do nothing
            if (user == null)
            {
                //TODO: log warning into logstore
                return 0;
            }
            var operationResult = await this.GetCollection().Update(document);
            
            return operationResult;
        }

        /// <summary>
        /// Update database records field in accordance with specified parameters.
        /// </summary>
        /// <param name="queryExpression">The expression defining the field to update.</param>
        /// <param name="queryValue">The value for comparison with the field value.</param>
        /// <param name="updateExpression">The expression for the database query to select record field for update.</param>
        /// <param name="updateValue">The new value to update records.</param>
        /// <param name="user">The user carries out the current request.</param>
        /// <returns>
        /// Task<long> representing the numbers of the updated documents.
        /// </returns>
        public async Task<long> UpdateAsync<TQVal, TUVal>(
            Expression<Func<TDocument, bool>> predicat,
            Expression<Func<TDocument, TUVal>> updateExpression,
            TUVal updateValue,
            TUser user
            )
        {
            //if any parameter is null do nothing
            if (predicat == null ||
                updateExpression == null || 
                updateValue == null || 
                user == null)
            {
                //TODO: log warning into logstore
                return 0;
            }
            var operationResult = await this.GetCollection().Update(predicat, updateExpression, updateValue);
            
            return operationResult;
        }
        #endregion IDocumentRecordStore implementation
    }
}