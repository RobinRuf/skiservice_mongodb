using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace skiservice.Models
{
    /// <summary>
    /// Model for a service
    /// </summary>
    public class ServiceModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("ServiceName")]
        [Required]
        [MaxLength(50)]
        public string ServiceName { get; set; }

        [BsonElement("Price")]
        [Required]
        public decimal Price { get; set; }
    }
}
