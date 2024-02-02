using System.ComponentModel.DataAnnotations;
using skiservice.Common;

namespace skiservice.Dto
{
    /// <summary>
    /// Data transfer object for a user
    /// </summary>
    public class UserDto
    {
        public string Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        public Roles Role { get; set; }

        public bool IsLocked { get; set; }
    }
}
