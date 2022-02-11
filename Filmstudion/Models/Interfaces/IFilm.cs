using Filmstudion.Models.Film;
using System;
using System.Collections.Generic;

namespace filmstudion.api.Models
{

    interface IFilm
    {
        public string FilmId { get; set; }
        public string Name { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Country { get; set; }
        public string Director { get; set; }
        public List<FilmCopy> FilmCopies { get; set; }
    }

}