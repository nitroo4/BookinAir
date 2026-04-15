using BOOKINGAPI.Models;
using MongoDB.Driver;

namespace BOOKINGAPI.Services;
public class ReservaService
{
    private readonly IMongoCollection<Reserva> _collection;
    public ReservaService(IMongoDatabase database)
    {
        _collection = database.GetCollection<Reserva>("Reserva");
    }
    public Reserva Create(Reserva reserva)
    {
        _collection.InsertOne(reserva);
        return reserva;
    }

    public async Task<List<Reserva>> GetAllAsync() =>
        await _collection.Find(_ => true).ToListAsync();
}