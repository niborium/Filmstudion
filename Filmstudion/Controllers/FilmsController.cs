using AutoMapper;
using Filmstudion.Models.Film;
using Filmstudion.Resources.FilmCopies;
using Filmstudion.Resources.Movies;
using Filmstudion.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Filmstudion.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/[controller]")]

    public class FilmsController : ControllerBase
    {

        private readonly IFilmServices _filmService;
        private readonly IFilmCopyService _filmCopyService;

        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IFilmStudioServices _filmStudioService;

        public FilmsController(IFilmServices filmService, IFilmCopyService filmCopyService, IUserService userService, IMapper mapper, IFilmStudioServices filmStudioService)
        {
            _filmService = filmService;
            _filmCopyService = filmCopyService;
            _userService = userService;
            _mapper = mapper;
            _filmStudioService = filmStudioService;
        }
        [HttpPut]
        public async Task<ActionResult<FilmResource>> PostNewFilmAsync([FromBody] CreateUpdateFilmResource resource)
        {
            try
            {
                var userName = User.Identity.Name;
                var userId = int.Parse(userName);
                var user = _userService.GetById(userId);

                if (user.IsAdmin == false) { return Unauthorized("Endast administratörer tillåts denna åtgärd."); }

                var film = _mapper.Map<CreateUpdateFilmResource, Film>(resource);
                _filmService.Add(film);

                if (await _filmService.SaveChangesAsync())
                {
                    var newCopies = film.NumberOfCopies;
                    var id = film.FilmId;
                    _filmCopyService.CreateCopies(newCopies, id);

                    var result = _mapper.Map<Film, FilmResource>(film);
                    result.AvailableFilmcopies = await _filmCopyService.GetAllFilmCopiesByFilmIdAsync(id);

                    return Created("Film skapades", result);
                }
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Det gick inte att spara filmen.");
            }
            return BadRequest();
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<FilmResource[]>> GetAllFilmsAsync()
        {
            try
            {
                var films = await _filmService.GetAllFilmsAsync();
                var Films = _mapper.Map<FilmResource[]>(films);
                if (User.Identity.IsAuthenticated)
                {
                    foreach (var film in Films)
                    {
                        var filmCopies = await _filmCopyService.GetAllFilmCopiesByFilmIdAsync(film.FilmId);
                        var available = filmCopies.Where(f => f.RentedOut == false);
                        film.AvailablefCopies = available.Count();
                        film.AvailableFilmcopies = available;
                    }
                }
                return Films;
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "[Serverfel] Det gick inte att hämta data från databasen");
            }
        }
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<FilmResource>> GetFilmByIdAsync(int id)
        {
            try
            {
                var film = await _filmService.GetFilmByIdAsync(id);
                var films = _mapper.Map<FilmResource>(film);
                if (User.Identity.IsAuthenticated)
                {
                    var filmCopies = await _filmCopyService.GetAllFilmCopiesByFilmIdAsync(film.FilmId);
                    var available = filmCopies.Where(f => f.RentedOut == false);
                    films.AvailablefCopies = available.Count();
                    films.AvailableFilmcopies = available;
                }
                return films;
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "[Serverfel] Det gick inte att hämta data från databasen");
            }
        }
        [HttpPatch("{filmId}")]
        public async Task<ActionResult<FilmResource>> UpdateFilmInformationAsync(int filmId, [FromBody] CreateUpdateFilmResource resource)
        {
            try
            {
                var userName = User.Identity.Name;
                var userId = int.Parse(userName);
                var user = _userService.GetById(userId);

                if (!user.IsAdmin) { return Unauthorized("Endast administratörer tillåts denna åtgärd."); }

                var oldFilm = await _filmService.GetFilmByIdAsync(filmId);

                if (oldFilm == null) { return NotFound($"Kunde inte hitta film med id: {filmId}"); }

                if (oldFilm.NumberOfCopies != resource.NumberOfCopies)
                {
                    if (oldFilm.NumberOfCopies < resource.NumberOfCopies)
                    {
                        int oldCopies = oldFilm.NumberOfCopies;
                        int newCopies = resource.NumberOfCopies;
                        _filmCopyService.CreateCopies(oldCopies, newCopies, filmId);
                    }

                    else if (oldFilm.NumberOfCopies > resource.NumberOfCopies)
                    {
                        var amount = resource.NumberOfCopies;
                        var copies = await _filmCopyService.GetAllFilmCopiesByFilmIdAsync(filmId);
                        _filmCopyService.DeleteCopies(amount, copies);
                    }
                }
                var newFilm = _mapper.Map(resource, oldFilm);
                var result = _mapper.Map<Film, FilmResource>(newFilm);
                result.AvailableFilmcopies = await _filmCopyService.GetAllFilmCopiesByFilmIdAsync(filmId);

                if (await _filmService.SaveChangesAsync()) { return Ok(result); }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "[Serverfel] Det gick inte att skicka data till databasen");
            }
            return BadRequest();
        }
        [HttpPut("{filmId}")]
        public async Task<ActionResult<FilmResource>> UpdateFilmInfoCopiesAsync(int filmId, [FromBody] CreateUpdateFilmResource resource)
        {
            try
            {
                var userName = User.Identity.Name;
                var userId = int.Parse(userName);
                var user = _userService.GetById(userId);

                if (!user.IsAdmin) { return Unauthorized("Endast administratörer tillåts denna åtgärd."); }

                var oldFilm = await _filmService.GetFilmByIdAsync(filmId);
                if (oldFilm == null) { return NotFound($"Kunde inte hitta film med id: {filmId}"); }

                if (oldFilm.NumberOfCopies != resource.NumberOfCopies)
                {
                    if (oldFilm.NumberOfCopies < resource.NumberOfCopies)
                    {
                        int oldCopies = oldFilm.NumberOfCopies;
                        int newCopies = resource.NumberOfCopies;
                        _filmCopyService.CreateCopies(oldCopies, newCopies, filmId);
                    }

                    else if (oldFilm.NumberOfCopies > resource.NumberOfCopies)
                    {
                        var amount = resource.NumberOfCopies;
                        var copies = await _filmCopyService.GetAllFilmCopiesByFilmIdAsync(filmId);
                        _filmCopyService.DeleteCopies(amount, copies);
                    }
                }
                var newFilm = _mapper.Map(resource, oldFilm);
                var result = _mapper.Map<Film, FilmResource>(newFilm);
                result.AvailableFilmcopies = await _filmCopyService.GetAllFilmCopiesByFilmIdAsync(filmId);

                if (await _filmService.SaveChangesAsync()) { return Ok(result); }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "[Serverfel] Det gick inte att skicka data till databasen");
            }
            return BadRequest();
        }
        [HttpPost]
        [Route("/api/Films/rent")]
        public async Task<ActionResult<FilmCopyResource>> RentFilmCopyAsync(int id, int studioid) //id = filmId
        {
            try
            {
                var userName = User.Identity.Name;
                var userId = int.Parse(userName);
                var user = _userService.GetById(userId);

                if (user.IsAdmin) { return Unauthorized("Endast FilmStudios tillåts denna åtgärd."); }

                var filmCopies = await _filmStudioService.GetRentedFilmCopiesAsync(studioid);
                var rentedFilmCopy = filmCopies.FirstOrDefault(f => f.FilmId == id);

                if (rentedFilmCopy != null) { return StatusCode(StatusCodes.Status403Forbidden, $"Du har redan hyrt en kopia av filmen med FilmId: {id}. Vänligen återlämna innan du fortsätter."); }

                var filmCopy = await _filmCopyService.GetAvaibleFilmCopyByFilmIdAsync(id);

                if (filmCopy == null) { return StatusCode(StatusCodes.Status409Conflict, $"Kunde inte hitta tillgänglig filmkopia med FilmId: {id}"); }

                filmCopy.RentedOut = true;
                filmCopy.FilmStudioId = studioid;

                var resource = _mapper.Map<FilmCopyResource>(filmCopy);
                var film = await _filmService.GetFilmByIdAsync(resource.FilmId);
                resource.FilmName = film.Name;

                if (await _filmCopyService.SaveChangesAsync()) { return Created("", resource); }
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "[Serverfel] Det gick inte att skicka data till databasen");
            }
            return BadRequest();
        }
        [HttpPut]
        [Route("/api/Films/return")]
        public async Task<ActionResult<FilmCopyResource>> ReturnFilmCopyAsync(int id, int studioid)
        {
            try
            {
                var userName = User.Identity.Name;
                var userId = int.Parse(userName);
                var user = _userService.GetById(userId);

                if (user.IsAdmin) { return Unauthorized("Endast FilmStudios tillåts denna åtgärd."); }

                var filmCopies = await _filmStudioService.GetRentedFilmCopiesAsync(studioid);
                var filmCopy = filmCopies.FirstOrDefault(f => f.FilmId == id);

                if (filmCopy == null) { return StatusCode(StatusCodes.Status409Conflict, $"Kunde inte hitta hyrd filmkopia med filmId: {id}"); }

                filmCopy.RentedOut = false;
                filmCopy.FilmStudioId = 0;

                var resource = _mapper.Map<FilmCopyResource>(filmCopy);
                var film = await _filmService.GetFilmByIdAsync(resource.FilmId);
                resource.FilmName = film.Name;

                if (await _filmCopyService.SaveChangesAsync()) { return Created("", resource); }
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "[Serverfel] Det gick inte att skicka data till databasen");
            }
            return BadRequest();
        }
    }
}
