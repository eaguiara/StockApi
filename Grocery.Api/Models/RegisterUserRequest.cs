using System.ComponentModel.DataAnnotations;

namespace Grocery.Api.Models
{
    public class RegisterUserRequest
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }

    }
}
