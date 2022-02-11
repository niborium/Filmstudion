namespace Filmstudion.Models.Film
{
    public class FilmCopy
    {
        public double FilmCopyId { get; set; }
        public int FilmId { get; set; }
        public bool RentedOut { get; set; }
        public int FilmStudioId { get; set; }
    }
}
