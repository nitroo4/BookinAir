using MongoDB.Driver;
using BOOKINGAPI.Models;
using BOOKINGAPI.DTOs;

namespace BOOKINGAPI.Services;

public class BilletService
{
    private readonly IMongoCollection<Billet> _billets;
    private readonly IMongoCollection<Destination> _destinations;

    public BilletService(IMongoDatabase database)
    {
        _billets = database.GetCollection<Billet>("Billets");
        _destinations = database.GetCollection<Destination>("Destinations");
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

    public async Task<BilletAvecDestinationDto?> GetBilletAvecDestinationAsync(string id)
    {
        var billet = await _billets.Find(x => x.Id == id).FirstOrDefaultAsync();

        if (billet is null)
            return null;

        var destination = await _destinations
            .Find(x => x.Id == billet.Id_Destination)
            .FirstOrDefaultAsync();

        return new BilletAvecDestinationDto
        {
            Id = billet.Id,
            Num_Billet = billet.Num_Billet,
            Total_Billet = billet.Total_Billet,
            Type = billet.Type,
            Id_Destination = billet.Id_Destination,
            Prix = billet.Prix,
            Destination = destination
        };
    }

    public async Task<List<BilletAvecDestinationDto>> GetAllBilletsAvecDestinationAsync()
    {
        var billets = await _billets.Find(_ => true).ToListAsync();
        var result = new List<BilletAvecDestinationDto>();

        foreach (var billet in billets)
        {
            var destination = await _destinations
                .Find(x => x.Id == billet.Id_Destination)
                .FirstOrDefaultAsync();

            result.Add(new BilletAvecDestinationDto
            {
                Id = billet.Id,
                Num_Billet = billet.Num_Billet,
                Total_Billet = billet.Total_Billet,
                Type = billet.Type,
                Id_Destination = billet.Id_Destination,
                Prix = billet.Prix,
                Destination = destination
            });
        }

        return result;
    }
}