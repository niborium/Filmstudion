using Filmstudion.Models.Film;
using Filmstudion.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Filmstudion.Services
{
    public class FilmService : IFilmServices
    {
        private readonly IFilmRepository _FilmRepository;

        public FilmService(IFilmRepository FilmRepository)
        {
            _FilmRepository = FilmRepository;
        }

        public void Add<T>(T entity) where T : class
        {
            _FilmRepository.Add(entity);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _FilmRepository.SaveChangesAsync());
        }

        public void Delete<T>(T entity) where T : class
        {
            _FilmRepository.Delete(entity);
        }

        public async Task<IEnumerable<Film>> GetAllFilmsAsync()
        {
            return await _FilmRepository.GetAllFilmsAsync();
        }

        public async Task<IEnumerable<FilmCopy>> GetAvailableFilmCopiesAsync(int FilmId)
        {
            return await _FilmRepository.GetAvailableFilmCopiesAsync(FilmId);
        }

        public async Task<Film> GetFilmByIdAsync(int FilmId)
        {
            return await _FilmRepository.GetFilmByIdAsync(FilmId);
        }
    }
}
