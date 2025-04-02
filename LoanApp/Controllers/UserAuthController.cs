using Azure.Core;
using LoanApp.Data;
using LoanApp.Features.Commands.Create;
using LoanApp.Features.DTOS;
using LoanApp.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LoanApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediatR;
        private readonly PasswordHasher<User> _passwordHasher;

        public UserAuthController(ApplicationDbContext context, IConfiguration configuration,IMediator mediatR)
        {
            _context = context;
            _configuration = configuration;
            _mediatR = mediatR;
            _passwordHasher = new PasswordHasher<User>();
        }

        [HttpPost("login")]
        public IActionResult Login(UserLoginDto login)
        {
            var user = _context.Users.SingleOrDefault
                (u => u.Email == login.Email);
            if (user == null)
            {
                return Unauthorized("Invalid email or password.");
            }
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, login.PasswordHash);

            if (result == PasswordVerificationResult.Failed)
            {
                return Unauthorized("Invalid email or password.");
            }

            var token = GenerateJwtToken(user);
            return Ok(new { token });
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[] {
                new Claim(ClaimTypes.Name,user.Email),
                  new Claim(ClaimTypes.Role,user.Role),

           };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: credential
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost("register")]
        
        public async Task<IActionResult> Register(User user) {
            var UserId = await _mediatR.Send(new CreateUserCommand(user.FirstName,user.LastName,user.Email,user.PasswordHash,user.ConfirmPassword,user.PhoneNumber, user.Role));
            return Ok(UserId);
        }
    }
}
