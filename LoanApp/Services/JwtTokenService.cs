using LoanApp.Features.DTOS;
using LoanApp.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LoanApp.Services
{
    public class JwtTokenService
    {
        private readonly IConfiguration _configuration;

        public JwtTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public LoginResponseDto GenerateJwtToken(User user)
        {
            var claims = new[] {
                new Claim(ClaimTypes.Name,user.Email),
                new Claim(ClaimTypes.Role,user.Role),
                new Claim("userId", user.Id.ToString())
                
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
            var jwt =  new JwtSecurityTokenHandler().WriteToken(token);
            return new LoginResponseDto(jwt, user.Role, user.Id);
           
        }
    }
}
