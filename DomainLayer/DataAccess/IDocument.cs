namespace DomainLayer.DataAccess
{ 
    public interface IDocument
    {
        MongoDB.Bson.ObjectId _Id { get; set; }
        string Id { get;}
    }
}