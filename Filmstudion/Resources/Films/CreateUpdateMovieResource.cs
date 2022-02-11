using System;
using System.ComponentModel.DataAnnotations;

namespace Filmstudion.Resources.Movies
{
    public class CreateUpdateFilmResource
    {
        public int FilmId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int ReleaseDate { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string Director { get; set; }
        [Required]
        [Range(1, 9)]
        public int NumberOfCopies { get; set; }
    }
}
