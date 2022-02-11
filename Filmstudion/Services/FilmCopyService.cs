using Filmstudion.Models.Film;
using Filmstudion.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Filmstudion.Services
{
    public class FilmCopyService : IFilmCopyService
    {
        private readonly IFilmCopyRepository _filmCopyRepository;

        public FilmCopyService(IFilmCopyRepository filmCopyRepository)
        {
            _filmCopyRepository = filmCopyRepository;
        }

        public void CreateCopies(int filmCopies, int FilmId)
        {
            _filmCopyRepository.CreateCopies(filmCopies, FilmId);
        }

        public void CreateCopies(int oldCopies, int newCopies, int FilmId)
        {
            _filmCopyRepository.CreateCopies(oldCopies, newCopies, FilmId);
        }

        public void DeleteCopies(int amount, IEnumerable<FilmCopy> filmCopies)
        {
            _filmCopyRepository.DeleteCopies(amount, filmCopies);
        }

        public void Add<T>(T entity) where T : class
        {
            _filmCopyRepository.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _filmCopyRepository.Delete(entity);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _filmCopyRepository.SaveChangesAsync());
        }

        public async Task<IEnumerable<FilmCopy>> GetAllRentedFilmCopiesAsync()
        {
            return await _filmCopyRepository.GetAllRentedFilmCopiesAsync();
        }

        public async Task<IEnumerable<FilmCopy>> GetAllFilmCopiesByFilmIdAsync(int FilmId)
        {
            return await _filmCopyRepository.GetAllFilmCopiesByFilmIdAsync(FilmId);
        }

        public async Task<FilmCopy> GetAvaibleFilmCopyByFilmIdAsync(int FilmId)
        {
            return await _filmCopyRepository.GetAvaibleFilmCopyByFilmIdAsync(FilmId);
        }
    }
}
