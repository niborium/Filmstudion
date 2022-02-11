using Filmstudion.Models.Film;
using System.Collections.Generic;

namespace Filmstudion.Resources.Public
{
    public class PublicFilmStudioResource
    {
        public int FilmStudioId { get; set; }
        public string Name { get; set; }
        public string City { get; set; }

        public IEnumerable<FilmCopy> RentedFilmcopies { get; set; }

    }
}
