using System.ComponentModel.DataAnnotations;
using skiservice.Common;

namespace skiservice.Dto
{
    public class CreateUserDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        public Roles Role { get; set; }
    }
}
