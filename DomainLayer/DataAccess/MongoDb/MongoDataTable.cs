﻿using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace DomainLayer.DataAccess.MongoDb
{
    public class MongoDataTable<TDocument> : IDataTable<TDocument> where TDocument : IDocument
    {
        private readonly IMongoCollection<TDocument> _collection;

        public MongoDataTable(IMongoCollection<TDocument> collection)
        {
            _collection = collection;
        }

        public IQueryable<TDocument> GetCollection()
        {
            return _collection.AsQueryable<TDocument>();
        }

        public async Task Insert(TDocument doc)
        {
            await _collection.InsertOneAsync(doc);
        }

        public async Task<long> Remove<TVal>(
            System.Linq.Expressions.Expression<Func<TDocument, bool>> predicat
            )
        {
            var result = await _collection.DeleteOneAsync(predicat);
            return result.DeletedCount;
        }

        public async Task<TDocument> FindOneById(string id)
        {
            var result = await this.Find<string>((doc) => doc.Id, id);
            if (result == null) return default(TDocument);
            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<TDocument>> Find<TVal>(
            System.Linq.Expressions.Expression<Func<TDocument, TVal>> expression, 
            TVal value
            )
        {
            var query = Builders<TDocument>.Filter.Eq(expression, value);
            var result = await _collection.FindAsync(query);
            return result.ToList().AsEnumerable();
        }

        public async Task<IEnumerable<TDocument>> Find(
            System.Linq.Expressions.Expression<Func<TDocument, object>> expression, 
            System.Text.RegularExpressions.Regex regex
            )
        {
            var query = Builders<TDocument>.Filter.Regex(expression, new BsonRegularExpression(regex));
            var result = await _collection.FindAsync(query);
            return result.ToList().AsEnumerable();
        }

        public async Task<long> Update(TDocument doc)
        {
            var result = await _collection.ReplaceOneAsync<TDocument>(s => s.Id == doc.Id, doc);
            return result.ModifiedCount;
        }

        public async Task<long> Update<TUVal>(
            System.Linq.Expressions.Expression<Func<TDocument, bool>> predicat,
            System.Linq.Expressions.Expression<Func<TDocument, TUVal>> updateExpression, 
            TUVal updateValue
            )
        {
            var update = Builders<TDocument>.Update.Set(updateExpression, updateValue);
            var result = await _collection.UpdateManyAsync<TDocument>(predicat, update);
            return result.ModifiedCount;
        }
    }
}