
using AutoMapper;
using Filmstudion.Helpers;
using Filmstudion.Models.User;
using Filmstudion.Resources.Users;
using Filmstudion.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Filmstudion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IFilmStudioServices _filmStudioService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;


        public UsersController(
            IUserService userService, IFilmStudioServices filmStudioService,
            IMapper mapper,
            IConfiguration config)
        {
            _userService = userService;
            _filmStudioService = filmStudioService;
            _mapper = mapper;
            _config = config;
        }
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateAsync([FromBody] AuthenticateModel model)
        {
            var user = _userService.Authenticate(model.Email, model.Password);

            if (user == null)
                return BadRequest(new { message = "E-post eller lösenord är felaktigt" });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["JWT:Secret"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            if (user.IsAdmin == false)
            {
                var filmStudios = await _filmStudioService.GetFilmStudioByIdAsync(user.FilmStudioId);
                return Ok(
                    new
                    {
                        Id = user.Id,
                        Username = user.Email,

                        RoleName = user.RoleName,
                        Token = tokenString,
                        filmStudioId = filmStudios.FilmStudioId,
                        filmStudioName = filmStudios.Name,
                        filmStudioCity = filmStudios.City,
                    }
                    );
            }
            else
            {
                return Ok(new
                {
                    Id = user.Id,
                    Username = user.Email,
                    RoleName = user.RoleName,
                    Token = tokenString
                });
            }
        }
        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult RegisterAdmin([FromBody] RegisterAdminUserModel model)
        {
            var user = _mapper.Map<User>(model);

            try
            {
                _userService.Create(user, model.Password);

                return Ok(new
                {
                    UserId = user.Id,
                    UserName = user.Email,
                    role = "admin"
                });
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
