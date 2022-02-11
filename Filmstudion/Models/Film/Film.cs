using System.Collections.Generic;

namespace Filmstudion.Models.Film
{
    public class Film
    {
        public int FilmId { get; set; }
        public string Name { get; set; }
        public int ReleaseYear { get; set; }
        public string Country { get; set; }
        public string Director { get; set; }
        public int NumberOfCopies { get; set; }
        public List<FilmCopy> FilmCopies { get; set; }
    }
}
