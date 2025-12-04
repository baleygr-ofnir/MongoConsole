using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoConsole.Models;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public string Email { get; set; }
    public Address UserAddress { get; set; }

    public override string ToString()
    {
        return $"User ID: {Id}, Name: {Name}, Age: {Age}, Email: {Email}, Address: {UserAddress}";
    }
}