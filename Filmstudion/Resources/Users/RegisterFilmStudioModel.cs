using System.ComponentModel.DataAnnotations;

namespace Filmstudion.Resources.Users
{
    public class RegisterFilmStudioModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public bool IsAdmin = false;
        public enum RoleName
        {
            filmstudio
        }
    }
}
