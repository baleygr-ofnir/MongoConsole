using MongoConsole.Models;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace MongoConsole.Data;

public class MongoCRUD
{
    private IMongoDatabase db;

    public MongoCRUD(string connectionString, string database)
    {
        var client = new MongoClient(connectionString);
        db = client.GetDatabase(database);
    }

    public async Task<User> AddUser(string collectionName, User user)
    {
        var collection = db.GetCollection<User>(collectionName);
        await collection.InsertOneAsync(user);
        return user;
    }

    public async Task<List<User>> GetUsers(string collectionName)
    {
        var collection = db.GetCollection<User>(collectionName);
        var users = await collection.AsQueryable().ToListAsync();
        return users;
    }

    public async Task<User> GetUser(string collectionName, string userId)
    {
        var collection = db.GetCollection<User>(collectionName);
        var user = await collection.Find(user => user.Id == userId).FirstOrDefaultAsync();
        return user;
    }

    public async Task<User> UpdateUser(string collectionName, string userId, User updatedUser)
    {
        var collection = db.GetCollection<User>(collectionName);
        var existingUser = await GetUser(collectionName, userId);
        updatedUser.Id = userId;
        if (string.IsNullOrEmpty(updatedUser.Name))
        {
            updatedUser.Name = existingUser.Name;
        }
        if (updatedUser.Age == 0)
        {
            updatedUser.Age = existingUser.Age;
        }
        if (string.IsNullOrEmpty(updatedUser.Email))
        {
            updatedUser.Email = existingUser.Email;
        }
        if (updatedUser.UserAddress.Equals(null))
        {
            updatedUser.UserAddress = existingUser.UserAddress;
        }
        var result = await collection.ReplaceOneAsync(user => user.Id == userId, updatedUser);
        return result.MatchedCount == 0 ? null : updatedUser;
    }

    public async Task<string> DeleteUser(string collectionName, string userId)
    {
        var collection = db.GetCollection<User>(collectionName);
        await collection.DeleteOneAsync(user => user.Id == userId);
        return $"User with ID {userId} was deleted...";
    }

    public async Task<string> DeleteUsers(string collectionName)
    {
        var collection = db.GetCollection<User>(collectionName);

        await collection.DeleteManyAsync(Builders<User>.Filter.Empty);

        return $"All users deleted from the collection {collectionName}";
    }
}