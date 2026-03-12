using MongoDB.Driver;
using BOOKINGAPI.Models;

namespace BOOKINGAPI.Services;

public class ReservationService
{
    private readonly IMongoCollection<Reservation> _reservations;
    private readonly IMongoCollection<User> _users;
    private readonly IMongoCollection<Destination> _destinations;

    public ReservationService(IMongoDatabase database)
    {
        _reservations = database.GetCollection<Reservation>("Reservations");
        _users = database.GetCollection<User>("Users");
        _destinations = database.GetCollection<Destination>("Destinations");
    }

    public async Task<List<Reservation>> GetAllAsync() =>
        await _reservations.Find(_ => true).ToListAsync();

    public async Task<Reservation?> GetByIdAsync(string id) =>
        await _reservations.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Reservation reservation) =>
        await _reservations.InsertOneAsync(reservation);

    public async Task UpdateAsync(string id, Reservation reservation) =>
        await _reservations.ReplaceOneAsync(x => x.Id == id, reservation);

    public async Task DeleteAsync(string id) =>
        await _reservations.DeleteOneAsync(x => x.Id == id);

    public async Task<bool> UserExistsAsync(string userId)
    {
        var user = await _users.Find(x => x.Id == userId).FirstOrDefaultAsync();
        return user is not null;
    }

    public async Task<bool> DestinationExistsAsync(string destinationId)
    {
        var destination = await _destinations.Find(x => x.Id == destinationId).FirstOrDefaultAsync();
        return destination is not null;
    }
}