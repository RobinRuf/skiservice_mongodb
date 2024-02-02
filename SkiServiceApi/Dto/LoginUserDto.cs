using System.ComponentModel.DataAnnotations;

namespace skiservice.Dto
{
    /// <summary>
    /// Data transfer object for a user login DTO
    /// </summary>
    public class LoginUserDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
