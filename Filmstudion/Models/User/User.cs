namespace Filmstudion.Models.User
{

    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public int FilmStudioId { get; set; }
        public bool IsAdmin { get; set; }
        public string Password { get; set; }
        public string RoleName { get; set; }
    }

}
