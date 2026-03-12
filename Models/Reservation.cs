using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using BOOKINGAPI.Enums;

namespace BOOKINGAPI.Models;

public class Reservation
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string UserId { get; set; } = null!;
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id_Destination { get; set; } = null!;
    public int NombreBillets { get; set; } = 1;
    public DateTime DateReservation { get; set; } = DateTime.Now;
    public ReservationStatus Status { get; set; } = ReservationStatus.EnAttente;
}