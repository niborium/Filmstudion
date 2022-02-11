using Filmstudion.Models.Film;
using System.Collections.Generic;

namespace filmstudion.api.Models
{

    interface IFIilmStudio
    {
        public string FilmStudioId { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public List<FilmCopy> RentedFilmCopies { get; set; }
    }
}