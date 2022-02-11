using AutoMapper;
using Filmstudion.Models;
using Filmstudion.Models.Film;
using Filmstudion.Resources.FilmCopies;
using Filmstudion.Resources.Movies;
using Filmstudion.Resources.Public;

namespace Filmstudion.Mapping
{
    public class ModeltoResourceProfile : Profile
    {
        public ModeltoResourceProfile()
        {
            CreateMap<FilmStudio, PublicFilmStudioResource>();

            CreateMap<Film, FilmResource>();
            CreateMap<Film, CreateUpdateFilmResource>();
            CreateMap<Film, PublicFilmResource>();

            CreateMap<FilmCopy, FilmCopyResource>();
        }
    }
}
