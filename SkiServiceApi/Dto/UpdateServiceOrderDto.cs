using System;
using System.ComponentModel.DataAnnotations;

namespace skiservice.Dto
{
    /// <summary>
    /// Data transfer object for updating a service request
    /// </summary>
    public class UpdateServiceOrderDto
    {
        [MaxLength(50)]
        public string Firstname { get; set; }

        [MaxLength(50)]
        public string Lastname { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string Phone { get; set; }

        public string PriorityId { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime? PickupDate { get; set; }

        public string ServiceId { get; set; }

        public decimal? Price { get; set; }

        public string StatusId { get; set; }

        [StringLength(500)]
        public string Comment { get; set; }
    }
}
