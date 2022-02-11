using AutoMapper;
using Filmstudion.Helpers;
using Filmstudion.Models;
using Filmstudion.Models.User;
using Filmstudion.Resources.FilmCopies;
using Filmstudion.Resources.Public;
using Filmstudion.Resources.Users;
using Filmstudion.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Filmstudion.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/[controller]")]

    public class FilmStudiosController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IFilmServices _filmService;
        private readonly IFilmCopyService _filmCopyService;

        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IFilmStudioServices _filmStudioService;
        public FilmStudiosController(IConfiguration configuration
            , IFilmServices filmService, IFilmCopyService filmCopyService, IUserService userService, IMapper mapper, IFilmStudioServices filmStudioService)
        {
            _configuration = configuration;
            _filmService = filmService;
            _userService = userService;
            _mapper = mapper;
            _filmStudioService = filmStudioService;
            _filmCopyService = filmCopyService;


        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<PublicFilmStudioResource[]>> GetAllFilmStudiosAsync()
        {
            try
            {
                var filmStudios = await _filmStudioService.GetAllFilmStudiosAsync();
                var filmStudiosMap = _mapper.Map<PublicFilmStudioResource[]>(filmStudios);
                if (User.Identity.IsAuthenticated)
                {
                    foreach (var rentedfilm in filmStudiosMap)
                    {
                        var filmCopies = await _filmStudioService.GetRentedFilmCopiesAsync(rentedfilm.FilmStudioId);
                        rentedfilm.RentedFilmcopies = filmCopies;
                    }
                    return Ok(filmStudiosMap);
                }
                else
                {
                    var filmStudiosForunauthenticated = filmStudiosMap.Select(e => new { e.FilmStudioId, e.Name }).ToList();
                    return Ok(filmStudiosForunauthenticated);
                }

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "[Serverfel] Det gick inte att hämta data från databasen");
            }
        }
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterFilmStudioModel model)
        {
            try
            {
                var filmStudio = _mapper.Map<RegisterFilmStudioModel, FilmStudio>(model);
                _filmStudioService.Add(filmStudio);

                var user = new User();
                if (await _filmStudioService.SaveChangesAsync())

                {
                    user.Email = model.Email;
                    user.FilmStudioId = filmStudio.FilmStudioId;
                    user.IsAdmin = false;
                    _userService.Create(user, model.Password);
                }
                return Ok(new
                {
                    FilmStudioId = filmStudio.FilmStudioId,
                    Name = filmStudio.Name,
                    City = filmStudio.City
                });
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [AllowAnonymous]
        [HttpGet("{Id}")]
        public async Task<ActionResult<PublicFilmStudioResource>> GetFilmStudioByIdAsync(int Id)
        {
            try
            {
                var filmStudios = await _filmStudioService.GetFilmStudioByIdAsync(Id);

                if (User.Identity.IsAuthenticated)
                {
                    var userName = User.Identity.Name;
                    var userId = int.Parse(userName);
                    var user = _userService.GetById(userId);

                    if (!user.IsAdmin)
                    {
                        var filmCopies = await _filmStudioService.GetRentedFilmCopiesAsync(filmStudios.FilmStudioId);
                        filmStudios.RentedFilmCopies = filmCopies.ToList();
                    }
                    var filmStudiosMap = _mapper.Map<PublicFilmStudioResource>(filmStudios);
                    return Ok(filmStudiosMap);
                }
                else
                {
                    var filmStudiosForunauthenticated = new
                    { FilmStudioId = filmStudios.FilmStudioId, Name = filmStudios.Name };
                    return Ok(filmStudiosForunauthenticated);
                }

            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "[Serverfel] Det gick inte att hämta data från databasen");
            }
        }
        [HttpGet]
        [Route("/api/mystudio/rentals")]
        public async Task<ActionResult<FilmCopyResource[]>> GetAllRentedFilmCopiesAsync()
        {
            try
            {
                var userName = User.Identity.Name;
                var userId = int.Parse(userName);
                var user = _userService.GetById(userId);

                if (user.IsAdmin)
                {
                    var rented = await _filmCopyService.GetAllRentedFilmCopiesAsync();
                    var filmcopies = _mapper.Map<FilmCopyResource[]>(rented);

                    foreach (var copie in filmcopies)
                    {
                        var film = await _filmService.GetFilmByIdAsync(copie.FilmId);
                        copie.FilmName = film.Name;
                    }
                    return filmcopies;
                }
                else if (!user.IsAdmin && user.FilmStudioId != 0)
                {
                    var filmStudioId = user.FilmStudioId;

                    var rented = await _filmStudioService.GetRentedFilmCopiesAsync(filmStudioId);
                    var filmcopies = _mapper.Map<FilmCopyResource[]>(rented);

                    if (rented == null)
                    {
                        return NotFound($"Du har inga lånade filmer.");
                    }

                    foreach (var copie in filmcopies)
                    {
                        var movie = await _filmService.GetFilmByIdAsync(copie.FilmId);
                        copie.FilmName = movie.Name;
                    }
                    return filmcopies;
                }
                return Unauthorized("Du tillåts inte denna åtgärd.");
            }

            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "[Serverfel] Det gick inte att hämta data från databasen");
            }
        }
    }
}
