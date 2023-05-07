using Grocery.Api.Enums;
using Grocery.Api.Models;
using GroceryApi.Data;
using GroceryApi.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Opw.HttpExceptions;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Grocery.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly Context _db;
        private IConfiguration _configuration;

        public AuthController(Context db, IConfiguration config)
        {
            _db = db;
            _configuration = config;
        }


        /// <summary>
        /// Método que registra novo usuário 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpPost("SignUp")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
        {
            if (_db.Users.Any(u => u.Username == request.Username))
            {
                throw new NotFoundException("Username already used");
            }

            var newUser = UserEntity.New(request.Name, request.Username, request.Password, UserRoles.Stock_Analyst);

            _db.Users.Add(newUser);
            await _db.SaveChangesAsync();

            var token = GenerateToken(newUser);

            return CreatedAtAction(null, new { Token = token, newUser });
        }

        /// <summary>
        /// Método para logar
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("SignIn")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = _db.Users
                .Where(u => u.Username == request.Username)
                .FirstOrDefault();

            if (user == null)
            {
                throw new NotFoundException("Username was not found");
            }

            if (!user.ValidatePassword(request.Password))
            {
                throw new UnauthorizedException("Invalid password");
            }

            var token = GenerateToken(user);

            return Ok(new { Token = token, user });
        }

        /// <summary>
        /// Gera o token do usuario
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private string GenerateToken(UserEntity user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]!);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("Id", user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Role, user.Role.ToString()),
                    new Claim(ClaimTypes.UserData, user.Username)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}