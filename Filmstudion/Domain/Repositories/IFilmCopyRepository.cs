using Filmstudion.Models.Film;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Filmstudion.Repositories
{
    public interface IFilmCopyRepository
    {
        void CreateCopies(int filmCopies, int movieId);
        void CreateCopies(int oldCopies, int newCopies, int movieId);
        void DeleteCopies(int amount, IEnumerable<FilmCopy> filmCopies);
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveChangesAsync();
        Task<IEnumerable<FilmCopy>> GetAllRentedFilmCopiesAsync();
        Task<IEnumerable<FilmCopy>> GetAllFilmCopiesByFilmIdAsync(int movieId);
        Task<FilmCopy> GetAvaibleFilmCopyByFilmIdAsync(int movieId);
    }
}
