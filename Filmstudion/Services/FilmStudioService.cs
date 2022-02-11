using Filmstudion.Models;
using Filmstudion.Models.Film;
using Filmstudion.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Filmstudion.Services
{
    public class FilmStudioService : IFilmStudioServices
    {
        private readonly IFilmStudioRepository _filmStudioRepository;
        public FilmStudioService(IFilmStudioRepository filmStudioRepository)
        {
            _filmStudioRepository = filmStudioRepository;
        }

        public void Add<T>(T entity) where T : class
        {
            _filmStudioRepository.Add(entity);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _filmStudioRepository.SaveChangesAsync());
        }

        public async Task<IEnumerable<FilmStudio>> GetAllFilmStudiosAsync()
        {
            return await _filmStudioRepository.GetAllFilmStudiosAsync();
        }

        public async Task<FilmStudio> GetFilmStudioByIdAsync(int filmStudioid)
        {
            return await _filmStudioRepository.GetFilmStudioByIdAsync(filmStudioid);
        }

        public async Task<IEnumerable<FilmCopy>> GetRentedFilmCopiesAsync(int filmStudioid)
        {
            return await _filmStudioRepository.GetRentedFilmCopiesAsync(filmStudioid);
        }
    }
}
