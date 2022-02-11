namespace filmstudion.api.Models
{

    interface IUser
    {
        public string UserId { get; set; }
        public string Role { get; set; }
        public string Username { get; set; }
        public string FilmStudioId { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
    }
}