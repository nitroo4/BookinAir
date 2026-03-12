using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using BOOKINGAPI.Enums;

namespace BookingAPI.Models;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string Nom { get; set; } = null!;
    public string Gmail { get; set; } = null!;
    public string Adresse { get; set; } = null!;
    public string Numero { get; set; } = null!;
    public string Password { get; set; } = null!;
    public UserRole Role { get; set; } = UserRole.Client;
    public UserStatus Status { get; set; } = UserStatus.Deconnecte;
}