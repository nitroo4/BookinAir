using MongoDB.Driver;
using BOOKINGAPI.Models;

namespace BOOKINGAPI.Services;

public class BilletService
{
    private readonly IMongoCollection<Billet> _billets;

    public BilletService(IMongoDatabase database)
    {
        _billets = database.GetCollection<Billet>("Billets");
    }

    public async Task<List<Billet>> GetAllAsync() =>
        await _billets.Find(_ => true).ToListAsync();

    public async Task<Billet?> GetByIdAsync(string id) =>
        await _billets.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Billet billet) =>
        await _billets.InsertOneAsync(billet);

    public async Task UpdateAsync(string id, Billet billet) =>
        await _billets.ReplaceOneAsync(x => x.Id == id, billet);

    public async Task DeleteAsync(string id) =>
        await _billets.DeleteOneAsync(x => x.Id == id);
}