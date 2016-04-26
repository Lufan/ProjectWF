namespace DomainLayer.DataAccess
{
    public interface IDatabase
    {
        IDataTable<TDocument> GetCollection<TDocument>(string collectionName) where TDocument : IDocument;
    }
}