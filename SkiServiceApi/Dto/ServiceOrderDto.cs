using skiservice.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace skiservice.Dto
{
    /// <summary>
    /// Data transfer object for a service request DTO
    /// </summary>
    public class ServiceOrderDto
    {
        [Key]
        public string Id { get; set; }

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
        public PriorityModel Priority { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        [Required]
        public DateTime PickupDate { get; set; }

        [Required]
        public string ServiceId { get; set; }
        public ServiceModel Service { get; set; }

        [Required]
        public decimal TotalPrice_CHF { get; set; }

        public string StatusId { get; set; }
        public StatusModel Status { get; set; }

        [MaxLength(500)]
        public string Comment { get; set; }
    }
}
