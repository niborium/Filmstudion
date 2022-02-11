using System.ComponentModel.DataAnnotations;

namespace Filmstudion.Resources.Users
{
    public class AuthenticateModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
