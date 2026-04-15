using MongoDB.Driver;
using BOOKINGAPI.Models;

namespace BOOKINGAPI.Services;
public class UserService
{
    private readonly IMongoCollection<User> _users;
    public UserService(IMongoDatabase database)
    {
        _users = database.GetCollection<User>("Users");
    }
    public async Task<List<User>> GetAllAsync() =>
        await _users.Find(_ => true).ToListAsync();
    public async Task<User?> GetByIdAsync(string id) =>
        await _users.Find(x => x.Id == id).FirstOrDefaultAsync();
    public async Task<User?> GetByEmailAsync(string email) =>
    await _users.Find(x => x.Email == email).FirstOrDefaultAsync();
    public async Task CreateAsync(User user) =>
        await _users.InsertOneAsync(user);
    public async Task UpdateAsync(string id, User user) =>
        await _users.UpdateOneAsync(
            x => x.Id == id,
            Builders<User>.Update.Set(u => u.Status, user.Status)
        );
    public async Task DeleteAsync(string id) =>
        await _users.DeleteOneAsync(x => x.Id == id);
}