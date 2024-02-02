using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace skiservice.Models
{
    /// <summary>
    /// Model for a priority
    /// </summary>
    public class PriorityModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("PriorityName")]
        [Required]
        [MaxLength(50)]
        public string PriorityName { get; set; }

        [BsonElement("Price")]
        [Required]
        public decimal Price { get; set; }
    }
}
