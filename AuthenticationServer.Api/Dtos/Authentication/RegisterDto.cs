using System.ComponentModel.DataAnnotations;

namespace AuthenticationServer.Api.Dtos.Authentication
{
    public class RegisterDto : BaseUserAuthDto
    {
        public string? FullName { get; set; }
        [Required(ErrorMessage = "Comfirm password is required.")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Password and comfirm password are not the same.")]
        public string ConfirmPassword { get; set; }
    }
}
