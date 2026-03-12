using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using BOOKINGAPI.Enums;

namespace BOOKINGAPI.Models;

public class Billet
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string Num_Billet { get; set; } = null!;
    public int Total_Billet { get; set; } = 50;
    public TypeClasse Type { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id_Destination { get; set; } = null!;
    public double Prix { get; set; }
}