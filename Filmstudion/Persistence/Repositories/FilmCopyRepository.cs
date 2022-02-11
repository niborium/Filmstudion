using AutoMapper;
using Filmstudion.Models.Film;
using Filmstudion.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Filmstudion.Persistence.Repositories
{
    public class FilmCopyRepository : BaseRepository, IFilmCopyRepository
    {
        public FilmCopyRepository(AppDbContext context, IMapper mapper) : base(context, mapper)
        { }

        public void CreateCopies(int filmCopies, int FilmId)
        {
            for (double i = 0.0; i < filmCopies; i++)
            {
                double partId = i + 1.0;
                double id = partId / 10;
                var entity = new FilmCopy();
                entity.FilmId = FilmId;
                entity.FilmCopyId = FilmId + id;
                var filmCopy = _mapper.Map<FilmCopy>(entity);
                _context.Add(filmCopy);
                _context.SaveChangesAsync();
            }
        }

        public void CreateCopies(int oldCopies, int newCopies, int FilmId)
        {
            for (double i = oldCopies; i < newCopies; i++)
            {
                double partId = i + 1.0;
                double id = partId / 10;
                var entity = new FilmCopy();
                entity.FilmId = FilmId;
                entity.FilmCopyId = FilmId + id;
                var filmCopy = _mapper.Map<FilmCopy>(entity);
                _context.Add(filmCopy);
                _context.SaveChangesAsync();
            }
        }

        public void DeleteCopies(int amount, IEnumerable<FilmCopy> filmCopies)
        {
            int currentCount = filmCopies.Count();
            foreach (var filmCopy in filmCopies)
            {
                _context.Remove(filmCopy);
                if (currentCount <= amount)
                {
                    break;
                }
                currentCount--;
                _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<FilmCopy>> GetAllRentedFilmCopiesAsync()
        {
            return await _context.FilmCopies.Where(f => f.RentedOut == true).ToListAsync();
        }

        public async Task<IEnumerable<FilmCopy>> GetAllFilmCopiesByFilmIdAsync(int FilmId)
        {
            return await _context.FilmCopies.Where(f => f.FilmId == FilmId).ToListAsync();
        }

        public async Task<FilmCopy> GetAvaibleFilmCopyByFilmIdAsync(int FilmId)
        {
            return await _context.FilmCopies.Where(f => f.FilmId == FilmId).FirstOrDefaultAsync(f => f.RentedOut == false);
        }
    }
}
