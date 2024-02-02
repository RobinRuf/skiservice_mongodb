using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace skiservice.Models
{
    /// <summary>
    /// Model for a status
    /// </summary>
    public class StatusModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("StatusName")]
        [Required]
        [MaxLength(50)]
        public string StatusName { get; set; }
    }
}
