using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BOOKINGAPI.Models;

public class Reserva
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string? User_Id { get; set; }
    public string? Billet_Id { get; set; }
    public string? Destination_Id { get; set; }
    public int Qty { get; set; }
    public decimal Prix { get; set; }
}