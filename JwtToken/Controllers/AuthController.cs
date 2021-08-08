using System;
using JwtToken.Data;
using JwtToken.Repository;
using JwtToken.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace JwtToken.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IRepository _repository;
        private readonly ITokenService _tokenService;

        public AuthController(IConfiguration configuration, IRepository repository, ITokenService tokenService)
        {
            _configuration = configuration;
            _repository = repository;
            _tokenService = tokenService;
        }

        [HttpPost("Login")]
        public ActionResult<UserReadDto> Login([FromBody]UserModel loginCredential)
        {
            var inDbUser = _repository.GetUser(loginCredential.UserName);
            if (inDbUser.Password == loginCredential.Password)
            {
                //you may login

                var userDto = new UserReadDto
                    {UserName = loginCredential.UserName, Password = loginCredential.Password};
                return Ok(_tokenService.BuildToken(userDto));

            }
            else
            {
                return Unauthorized();
            }
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("AllUsers")]
        public ActionResult GetAllUsers([FromHeader(Name = "Authorization")]string token)
        {
            Console.WriteLine("in function");
            if (token == null || _tokenService.ValidateToken(token))
            {
                return Unauthorized();
            }
            return Ok(_repository.GetAllUsers());
        }
    }
}