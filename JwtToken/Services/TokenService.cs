using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtToken.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace JwtToken.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        private const double EXPIRY_DURATION_MINUTES = 30;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string BuildToken(UserReadDto user)
        {
            var issuer = _configuration["Jwt:Key"].ToString();
            var key = _configuration["Jwt:Issuer"].ToString();
            var audience = _configuration["Jwt:Audience"].ToString();
            
            var claims = new[] {    
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier,
                    Guid.NewGuid().ToString())
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF32.GetBytes("1234567891234567"));        
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);           
            var tokenDescriptor = new JwtSecurityToken(issuer, audience, claims, 
                expires: DateTime.Now.AddMinutes(EXPIRY_DURATION_MINUTES), signingCredentials: credentials);        
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);  
        }

        public bool ValidateToken(string token)
        {
            var issuer = _configuration["Jwt:Key"].ToString();
            var key = _configuration["Jwt:Issuer"].ToString();
            var audience = _configuration["Jwt:Audience"].ToString();
            
            var mySecret = Encoding.UTF32.GetBytes("1234567891234567");           
            var mySecurityKey = new SymmetricSecurityKey(mySecret);
            var tokenHandler = new JwtSecurityTokenHandler(); 
            try 
            {
                tokenHandler.ValidateToken(token, 
                    new TokenValidationParameters   
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true, 
                        ValidateAudience = true,    
                        ValidIssuer = issuer,
                        ValidAudience = audience, 
                        IssuerSigningKey = mySecurityKey,
                    }, out SecurityToken validatedToken);          
                
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            return true;    
        }
    }
    
}