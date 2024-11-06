﻿using AuthenticationServer.Api.Dtos;
using AuthenticationServer.Api.Dtos.Authentication;
using AuthenticationServer.Api.Dtos.Tokens;
using AuthenticationServer.Api.Dtos.User;
using AuthenticationServer.Api.Services.Interfaces;
using gp_backend.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationServer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _RoleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;
        public AuthController(ITokenService tokenService, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, ILogger<AuthController> logger, RoleManager<IdentityRole> roleManager)
        {
            _tokenService = tokenService;
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _RoleManager = roleManager;
        }
        // login
        [HttpPost("auth/login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            try
            {
                // check the properties validation
                if (!ModelState.IsValid)
                {
                    return BadRequest(new BaseResponse(state: false, message: ModelState.Values.SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage).ToList(), null));
                }

                // check if the user registerd
                var count = await _userManager.Users
                    .Where(u => u.Email == model.Email)
                    .CountAsync();
                if(count > 0)
                    return Unauthorized(new BaseResponse(false, new List<string> { "the password is not correct." }, null));

                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, true, false);
                    if (result.Succeeded)
                    {
                        var tokens = await _tokenService.GenerateTokensAsync(user);
                        return Ok(new BaseResponse(true, new List<string> { "Success" }, new { access = tokens.accessToken, refresh = tokens.refreshToken }));
                    }

                    return Unauthorized(new BaseResponse(false, new List<string> { "the password is not correct." }, null));
                }

                return BadRequest(new BaseResponse(false, new List<string> { "the user not registered." }, null));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "an error occurred while logging a user.");
                return StatusCode(500, new BaseResponse(false, new List<string> { "An error occurred while processing your request" }, null));
            }
        }
        // Register
        [HttpPost("auth/register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            try
            {
                // check the properties validation
                if (!ModelState.IsValid)
                {
                    return BadRequest(new BaseResponse(state: false, message: ModelState.Values.SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage).ToList(), null));
                }

                // check if the email is unique
                var email = await _userManager.FindByEmailAsync(model.Email);
                if (email != null)
                    return BadRequest(new BaseResponse(false, new List<string> { "This email already exists." }, null));

                // create the user
                var user = new ApplicationUser { Email = model.Email, UserName = Guid.NewGuid().ToString(), FullName = model.FullName };
                var result = await _userManager.CreateAsync(user, model.Password);

                // return the token
                if (result.Succeeded)
                {
                    var tokens = await _tokenService.GenerateTokensAsync(user);
                    return Ok(new BaseResponse(true, new List<string> { "Success" }, new { access = tokens.accessToken, refresh = tokens.refreshToken }));
                }

                return BadRequest(new BaseResponse(false, result.Errors.Select(e => e.Description).ToList(), null));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "an error occurred while registering a user.");
                return StatusCode(500, new BaseResponse(false, new List<string> { "An error occurred while processing your request" }, null));
            }
        }
        // Refresh
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
        {
            try
            {
                var user = await _userManager.Users.Include(u => u.RefreshTokens).SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == request.RefreshToken));
                if (user == null || !user.RefreshTokens.Any(t => t.Token == request.RefreshToken && t.ExpirationDate > DateTime.UtcNow))
                {
                    return Unauthorized(new BaseResponse(false, new List<string> { "Invalid refresh token" }, null));
                }

                var tokens = await _tokenService.GenerateTokensAsync(user);
                return Ok(new BaseResponse(true, new List<string> { "Success" }, new { access = tokens.accessToken, refresh = tokens.refreshToken }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "an error occurred while logging a user.");
                return StatusCode(500, new BaseResponse(false, new List<string> { "An error occurred while generating refresh token your request" }, null));
            }
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<GetAllUsersDto>>> GetAllUsers()
        {
            var users = _userManager.Users
                .Select(u => new GetAllUsersDto
                {
                    Id = u.Id,
                    UserName = u.FullName
                })
                .ToList();

            return Ok(users);
        }
    }
}
