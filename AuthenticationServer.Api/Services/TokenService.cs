using AuthenticationServer.Api.Services.Interfaces;
using gp_backend.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AuthenticationServer.Api.Services
{
    public class TokenService : ITokenService
    {
        private readonly Jwt _jwtSettings;
        private readonly UserManager<ApplicationUser> _userManager;
        public TokenService(IOptions<Jwt> jwtSettings, UserManager<ApplicationUser> userManager)
        {
            _jwtSettings = jwtSettings.Value;
            _userManager = userManager;
        }


        private async Task<string> WriteTokenAsync(JwtSecurityToken token)
        {
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /*
         * ===========================================
         Create refresh token and add it to the system
        * ============================================
         */

        // it takes the user and return the access token and refresh token
        public async Task<(string accessToken, string refreshToken)> GenerateTokensAsync(ApplicationUser user)
        {
            var accessToken = await GenerateAccessTokenAsync(user);
            var refreshToken = GenerateRefreshToken();

            // save the refresh to the data base
            user.RefreshTokens.Add(new RefreshToken { Token = refreshToken, ExpirationDate = DateTime.UtcNow.AddDays(7) });
            await _userManager.UpdateAsync(user);

            return (accessToken, refreshToken);
        }
        private async Task<string> GenerateAccessTokenAsync(ApplicationUser user)
        {
            // user main info
            var claims = new List<Claim>
            {
                new ("uid", user.Id.ToString()),
                new (JwtRegisteredClaimNames.Sub, user.FullName),
                new (JwtRegisteredClaimNames.Email, user.Email),
                new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new ("exp", DateTime.Now.AddMinutes(_jwtSettings.DurationMin).ToString())
            };
            // user roles
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim("roles", role));
            }

            // create the jwt token with the specified parameters
            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwtSettings.DurationMin),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key)), SecurityAlgorithms.HmacSha256)
            );
            var authToken = await WriteTokenAsync(token);

            return authToken;
        }

        // Create the refresh token method
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
        public string GetUserIdFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "Id");
            var userId = userIdClaim?.Value;

            return userId;
        }
    }
}
