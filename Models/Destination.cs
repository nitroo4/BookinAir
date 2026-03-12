using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using BOOKINGAPI.Enums;

namespace BOOKINGAPI.Models;

public class Destination
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string Lieu_Depart { get; set; } = null!;
    public string Lieu_Arriver { get; set; } = null!;
    public string Heur_Depart { get; set; } = null!;
    public string Duree { get; set; } = null!;
    public DateTime Date_Depart { get; set; }
    public DateTime Date_Arriver { get; set; }
    public NomVol Nom_Vol { get; set; }
    public double Prix { get; set; }
}
