using AuthenticationServer.Api.Dtos.Authentication;

namespace AuthenticationServer.Api.Dtos.Doctor
{
    public class RegisterDoctorDto : BaseUserAuthDto
    {
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
