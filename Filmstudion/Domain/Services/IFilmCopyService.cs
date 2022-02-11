using Filmstudion.Models.Film;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Filmstudion.Services
{
    public interface IFilmCopyService
    {
        void CreateCopies(int filmCopies, int FilmId);
        void CreateCopies(int oldCopies, int newCopies, int FilmId);
        void DeleteCopies(int amount, IEnumerable<FilmCopy> filmCopies);
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveChangesAsync();
        Task<IEnumerable<FilmCopy>> GetAllRentedFilmCopiesAsync();
        Task<IEnumerable<FilmCopy>> GetAllFilmCopiesByFilmIdAsync(int FilmId);
        Task<FilmCopy> GetAvaibleFilmCopyByFilmIdAsync(int FilmId);
    }
}
