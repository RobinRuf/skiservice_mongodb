using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace skiservice.Models
{
    /// <summary>
    /// Model for a service request
    /// </summary>
    public class ServiceOrderModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Firstname")]
        [Required]
        [MaxLength(50)]
        public string Firstname { get; set; }

        [BsonElement("Lastname")]
        [Required]
        [MaxLength(50)]
        public string Lastname { get; set; }

        [BsonElement("Email")]
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [BsonElement("Phone")]
        [Required]
        [Phone]
        public string Phone { get; set; }

        [BsonElement("PriorityId")]
        public string PriorityId { get; set; }

        public PriorityModel Priority { get; set; }

        [BsonElement("CreateDate")]
        [Required]
        public DateTime CreateDate { get; set; }

        [BsonElement("PickupDate")]
        [Required]
        public DateTime PickupDate { get; set; }

        [BsonElement("ServiceId")]
        public string ServiceId { get; set; }

        public ServiceModel Service { get; set; }

        [BsonElement("TotalPrice_CHF")]
        [Required]
        public decimal TotalPrice_CHF { get; set; }

        [BsonElement("StatusId")]
        public string StatusId { get; set; }

        public StatusModel Status { get; set; }

        [BsonElement("Comment")]
        [StringLength(500)]
        public string? Comment { get; set; }

        [BsonElement("UserId")]
        public string? UserId { get; set; }

        public UserModel User { get; set; }
    }
}
