using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using skiservice.Common;

namespace skiservice.Models
{
    public class UserModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("UserName")]
        [Required]
        public string UserName { get; set; }

        [BsonElement("PasswordHash")]
        public byte[] PasswordHash { get; set; }

        [BsonElement("PasswordSalt")]
        public byte[] PasswordSalt { get; set; }

        [BsonElement("PasswordInputAttempt")]
        public int PasswordInputAttempt { get; set; } = 0;

        [BsonElement("IsLocked")]
        public bool IsLocked { get; set; }

        // Anpassungen für die Rolle, abhängig von ihrer Darstellung (Referenz oder eingebettetes Dokument)
        [BsonElement("Role")]
        public Roles Role { get; set; }
    }
}
