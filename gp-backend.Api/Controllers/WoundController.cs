using gp_backend.Api.Dtos;
using gp_backend.EF.MySql.Repositories.Interfaces;
using gp_backend.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace gp_backend.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WoundController : ControllerBase
    {
        private readonly IGenericRepo<Wound> _woundRepo;
        private readonly ILogger<WoundController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        public WoundController(IGenericRepo<Wound> woundRepo, ILogger<WoundController> logger,
        UserManager<ApplicationUser> userManager)
        {
            _woundRepo = woundRepo;
            _logger = logger;
            _userManager = userManager;
        }

        // add
        [RequestSizeLimit(1000_000_000)]
        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            try
            {
                // check the properties validation
                if (!ModelState.IsValid)
                {
                    return BadRequest(new BaseResponse(state: false, message: ModelState.Values.SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage).ToList(), null));
                }

                // extract the file description
                var fileDescription = GetDescription(file);

                /*
                 * =======================
                 Block of code here
                 * ===========================
                 */
                var user = await _userManager.GetUserAsync(User);

                var wound = new Wound { Type = "Type",
                    Location = "Location",
                    UploadDate = DateTime.Now.Date,
                    Advice = "Advice",
                    User = user,
                    ApplicationUserId = user.Id,
                    Image = fileDescription,
                    Disease = new Disease { Name = "Disease", Description = "Description", Preventions = new List<string> { "this is list"} }
                };

                var result = await _woundRepo.InsertAsync(wound);
                await _woundRepo.SaveAsync();

                return Ok(new BaseResponse(true, new List<string> { "Uploaded Successfuly" }, null));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "an error occurred while uploading the file.");
                return StatusCode(500, new BaseResponse(false, new List<string> { "An error occurred while processing your request" }, null));
            }
        }
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var user = await _userManager.GetUserAsync(User);
            var wounds = await _woundRepo.GetAllAsync(user.Id);

            var resultList = new List<GetWoundDto>();
            foreach(var wound in wounds)
            {
                resultList.Add(new GetWoundDto
                {
                    Id = wound.Id,
                    file = wound.Image.Content.Content,
                    Type = wound.Type,
                    Location = wound.Location,
                    AddedDate = wound.UploadDate
                });
            }
            if(resultList.Count > 0)
                return Ok(new BaseResponse(true, new List<string> { "Uploaded Successfuly" }, resultList));
            else
                return Ok(new BaseResponse(true, new List<string> { "History Empty" }, resultList));
        }

        [HttpGet("get-id")]
        public async Task<IActionResult> GetById(int id)
        {
            // check the properties validation
            if (!ModelState.IsValid)
            {
                return BadRequest(new BaseResponse(state: false, message: ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage).ToList(), null));
            }

            var result = new Wound();
            if (id > 0)
            {
                result = await _woundRepo.GetByIdAsync(id);
                if (result != null)
                    return Ok(new BaseResponse(true, new List<string> { "Success" }, new GetWoundDetailsDto
                    {
                        Id = result.Id,
                        Advice = result.Advice,
                        Description = result.Disease.Description,
                        Image = result.Image.Content.Content,
                        Name = result.Disease.Name,
                        Location = result.Location,
                        Preventions = result.Disease.Preventions,
                        Type = result.Type,
                        UploadDate = result.UploadDate,
                        Risk = result.Disease.Risk
                    }));
                else
                    return NotFound();
            }
            return BadRequest(new BaseResponse(false, new List<string> { "id not valid" }, null));
        }    
        /*
         Methods
         */
        private FileDescription GetDescription(IFormFile file)
        {
            byte[] fileBytes;

            using (var fs = file.OpenReadStream())
            {
                using (var sr = new BinaryReader(fs))
                {
                    fileBytes = sr.ReadBytes((int)file.Length);
                }
            }
            var fileContent = new FileContent
            {
                Content = fileBytes
            };
            return new FileDescription
            {
                Content = fileContent,
                ContentType = file.ContentType,
                ContentDisposition = file.ContentDisposition,
            };
        }
    }
}
