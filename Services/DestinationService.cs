using MongoDB.Driver;
using BOOKINGAPI.Models;

namespace BOOKINGAPI.Services;

public class DestinationService
{
    private readonly IMongoCollection<Destination> _destinations;

    public DestinationService(IMongoDatabase database)
    {
        _destinations = database.GetCollection<Destination>("Destinations");
    }

    public async Task<List<Destination>> GetAllAsync() =>
        await _destinations.Find(_ => true).ToListAsync();

    public async Task<Destination?> GetByIdAsync(string id) =>
        await _destinations.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Destination destination) =>
        await _destinations.InsertOneAsync(destination);

    public async Task UpdateAsync(string id, Destination destination) =>
        await _destinations.ReplaceOneAsync(x => x.Id == id, destination);

    public async Task DeleteAsync(string id) =>
        await _destinations.DeleteOneAsync(x => x.Id == id);
}