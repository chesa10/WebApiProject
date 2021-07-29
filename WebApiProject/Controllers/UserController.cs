using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApiProject.Data.Interfaces;
using WebApiProject.DTOs;
using WebApiProject.Errors;
using WebApiProject.Extensions;
using WebApiProject.Models;

namespace WebApiProject.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IConfiguration configuration;

        public UserController(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            this.unitOfWork = unitOfWork;
            this.configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(RegisterReqDTO loginDTO)
        {
            var user = await unitOfWork.UserRepository.Authenticate(loginDTO.UserName, loginDTO.Password);

            ApiError apiError = new ApiError();
            if (user == null)
            {
                apiError.ErrorCode = Unauthorized().StatusCode;
                apiError.ErrorMessage = "Invalid username or password.";
                apiError.ErrorMessage = "Invalid username or password.";
                return Unauthorized(apiError);
            }

            var loginRes = new LoginResDTO();
            loginRes.username = user.Username;
            loginRes.Token = CreateJWT(user);

            return Ok(loginRes);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterReqDTO loginDTO)
        {

            ApiError apiError = new ApiError();
            if ((loginDTO.UserName).IsEmpty() || (loginDTO.Password).IsEmpty())
            {
                apiError.ErrorCode = Unauthorized().StatusCode;
                apiError.ErrorMessage = "Invalid username or password.";
                return BadRequest(apiError);
            }

            if (await unitOfWork.UserRepository.UserAlreadyExists(loginDTO.UserName))
            {
                apiError.ErrorCode = Unauthorized().StatusCode;
                apiError.ErrorMessage = "User already exists.";
                return BadRequest(apiError);
            }

            unitOfWork.UserRepository.Register(loginDTO);
            await unitOfWork.SaveAsync();

            return StatusCode(201);
        }

        private string CreateJWT(User user)
        {
            var secretKey = configuration.GetSection("AppSettings:Key").Value;
            var key = new SymmetricSecurityKey(Encoding.UTF8
                            .GetBytes(secretKey));

            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var signingCredentials = new SigningCredentials(
                key, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(60),
                SigningCredentials = signingCredentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
