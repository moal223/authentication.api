using Microsoft.AspNetCore.Identity;

namespace gp_backend.Core.Models
{
    public class ApplicationUser : IdentityUser
    {
        public List<RefreshToken>? RefreshTokens { get; set; } = [];
        public List<Wound>? Wounds { get; set; } = [];
        public List<FeedBack>? FeedBacks { get; set; } = [];
        public List<Specialization>? Specializations { get; set; } = [];
    }
}
