﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SignalRWithAuthentication.Data;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SignalRWithAuthentication.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IConfiguration config;

        public TokenController(SignInManager<ApplicationUser> signInManager, IConfiguration config)
        {
            this.signInManager = signInManager;
            this.config = config;
        }

        private string GenerateToken(string userId)
        {
            var key = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(config["JwtKey"]));
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            };
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(issuer: "signalrdemo", audience: "signalrdemo", claims: claims, expires: DateTime.UtcNow.AddDays(1), signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// An action to exchange an identity cookie for a token
        /// </summary>
        /// <returns>A JW Token</returns>
        [HttpGet("/api/token")]
        //[HttpGet("get")]
        [Authorize]
        public IActionResult GetToken()
        {
            return Ok(GenerateToken(User.Identity.Name));
        }

        /// <summary>
        /// Another endpoint for non-Web clients to exchange a user’s valid username and password for a token
        /// </summary>
        /// <param name="login">User name and password</param>
        /// <returns>A JW Token</returns>
        [HttpPost("/api/token")]
        public async Task<IActionResult> GetTokenForCredentialsAsync([FromBody] LoginRequest login)
        {
            var result = await signInManager.PasswordSignInAsync(login.Username, login.Password, false, true);
            return result.Succeeded ? (IActionResult)Ok(GenerateToken(login.Username)) : Unauthorized();
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
