using AuthenticationServer.Api.Dtos;
using AuthenticationServer.Api.Dtos.Doctor;
using AuthenticationServer.Api.Services.Interfaces;
using gp_backend.Core.Models;
using gp_backend.EF.MSSql.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationServer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _RoleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly ISpecialRepo _specialRepo;
        public DoctorController(ITokenService tokenService, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, ILogger<AuthController> logger, RoleManager<IdentityRole> roleManager,
            ISpecialRepo specialRepo)
        {
            _tokenService = tokenService;
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _RoleManager = roleManager;
            _specialRepo = specialRepo;
        }

        [HttpPost("doc-register")]
        public async Task<IActionResult> RegisterAsDoctor([FromForm] RegisterDoctorDto model)
        {
            try
            {
                // check the properties validation
                if (!ModelState.IsValid)
                {
                    return BadRequest(new BaseResponse(state: false, message: ModelState.Values.SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage).ToList(), null));
                }

                /*
                    check if the Specialization exists and if it dosen't of the taple is empty return this Specialization doesn't exist
                 */
                var specials = (await _specialRepo.GetAllAsync("")).Where(x => x.Id == model.SpecialId).ToList();

                if(specials.Count <= 0)
                    return BadRequest(new BaseResponse(false, new List<string> { "this Specialization doesn't exist!" }, null));

                // check if the email is unique
                var email = await _userManager.FindByEmailAsync(model.Email);
                if (email != null)
                    return BadRequest(new BaseResponse(false, new List<string> { "This email already exists." }, null));

                // create the user
                var user = new ApplicationUser { Email = model.Email,
                    FullName = model.FullName,
                    PhoneNumber = model.PhoneNumber,
                    Specializations = specials,
                    UserName = Guid.NewGuid().ToString() };
                var result = await _userManager.CreateAsync(user, model.Password);

                // return the token
                if (result.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(user, "Doc");
                    if (roleResult.Succeeded)
                    {
                        var tokens = await _tokenService.GenerateTokensAsync(user);
                        return Ok(new BaseResponse(true, new List<string> { "Success" }, new { access = tokens.accessToken, refresh = tokens.refreshToken }));
                    }
                    return BadRequest(new BaseResponse(false, roleResult.Errors.Select(e => e.Description).ToList(), null));
                }

                return BadRequest(new BaseResponse(false, result.Errors.Select(e => e.Description).ToList(), null));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "an error occurred while registering a user.");
                return StatusCode(500, new BaseResponse(false, new List<string> { "An error occurred while processing your request" }, null));
            }
        }
    }
}
