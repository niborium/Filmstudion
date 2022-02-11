using AutoMapper;
using Filmstudion.Models.Film;
using Filmstudion.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Filmstudion.Persistence.Repositories
{
    public class FilmRepository : BaseRepository, IFilmRepository
    {
        public FilmRepository(AppDbContext context, IMapper mapper) : base(context, mapper)
        {

        }

        public async Task<IEnumerable<Film>> GetAllFilmsAsync()
        {
            return await _context.Films.ToListAsync();
        }

        public async Task<IEnumerable<FilmCopy>> GetAvailableFilmCopiesAsync(int FilmId)
        {
            return await _context.FilmCopies.Where(f => f.FilmId == FilmId).Where(f => f.RentedOut == false).ToListAsync();
        }

        public async Task<Film> GetFilmByIdAsync(int FilmId)
        {
            return await _context.Films.FirstOrDefaultAsync(m => m.FilmId == FilmId);
        }
    }
}
