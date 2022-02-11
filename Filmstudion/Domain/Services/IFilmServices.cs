using Filmstudion.Models.Film;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Filmstudion.Services
{
    public interface IFilmServices
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveChangesAsync();
        Task<IEnumerable<Film>> GetAllFilmsAsync();
        Task<Film> GetFilmByIdAsync(int MovieId);
        Task<IEnumerable<FilmCopy>> GetAvailableFilmCopiesAsync(int MovieId);
    }
}
