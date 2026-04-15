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

    public async Task<bool> ExistsByNumBilletAsync(string numBillet)
    {
        return await _billets
            .Find(b => b.Num_Billet == numBillet)
            .AnyAsync();
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

    public async Task<BilletAvecDestinationDto?> GetBilletAvecDestinationByIdAsync(string id)
    {
        var b = await _billets
            .Find(x => x.Id == id)
            .FirstOrDefaultAsync();

        if (b == null)
            return null;

        var destination = await _destinations
            .Find(x => x.Id == b.Id_Destination)
            .FirstOrDefaultAsync();

        return new BilletAvecDestinationDto
        {
            Id = b.Id,
            Num_Billet = b.Num_Billet,
            Total_Billet = b.Total_Billet,
            Type = b.Type,
            Id_Destination = b.Id_Destination,
            Prix = b.Prix,
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

    //pour reduc de billet
    public async Task<bool> AcheterBilletAsync(string billetId, int qty)
    {
        var update = Builders<Billet>.Update.Inc(x => x.Total_Billet, -qty);

        var result = await _billets.UpdateOneAsync(
            x => x.Id == billetId && x.Total_Billet >= qty,
            update
        );

        return result.ModifiedCount > 0;
    }
}