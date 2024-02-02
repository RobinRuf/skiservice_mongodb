using System;
using System.ComponentModel.DataAnnotations;

namespace skiservice.Dtos
{
    /// <summary>
    /// Data transfer object for creating a new service request DTO
    /// </summary>
    public class CreateServiceOrderDto
    {
        [Required]
        [MaxLength(50)]
        public string Firstname { get; set; }

        [Required]
        [MaxLength(50)]
        public string Lastname { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        [Required]
        public string PriorityId { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        [Required]
        public DateTime PickupDate { get; set; }

        [Required]
        public string ServiceId { get; set; }

        [MaxLength(500)]
        public string Comment { get; set; }
    }
}
