using gp_backend.Core.Models;

namespace AuthenticationServer.Api.Services.Interfaces
{
    public interface ITokenService
    {
        Task<(string accessToken, string refreshToken)> GenerateTokensAsync(ApplicationUser user);
        string GetUserIdFromToken(string token);
    }
}
