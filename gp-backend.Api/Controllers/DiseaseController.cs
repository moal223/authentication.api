using gp_backend.Api.Dtos;
using gp_backend.Api.Dtos.Disease;
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
    public class DiseaseController : ControllerBase
    {
        private readonly IGenericRepo<Disease> _diseaseRepo;
        private readonly ISpecialRepo _specialRepo;
        private readonly ILogger<DiseaseController> _logger;
        public DiseaseController(IGenericRepo<Disease> diseaseRepo, ISpecialRepo specialRepo, ILogger<DiseaseController> logger)
        {
            _diseaseRepo = diseaseRepo;
            _specialRepo = specialRepo;
            _logger = logger;
        }

        // Add
        // - first get the specialization
        // if it does not exist the program won't add the disease
        [Authorize(Roles = "Admin")]
        [HttpPost("add")]
        public async Task<IActionResult> Add(AddDiseaseDto model)
        {
            // check the properties validation
            if (!ModelState.IsValid)
            {
                return BadRequest(new BaseResponse(state: false, message: ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage).ToList(), null));
            }

            // check the specialization id
            if(model.SpecializationId <= 0)
            {
                return BadRequest(new BaseResponse(state: false, message: new List<string> { "Invalid specialization."}, null));
            }


            
            var special = await _specialRepo.GetByIdAsync(model.SpecializationId);
            if (special == null) {
                return BadRequest(new BaseResponse(state: false, message: new List<string> { "Invalid specialization." }, null));
            }

            var result = new Disease {
                Description = model.Description,
                Name = model.Name,
                Preventions = model.Preventions,
                Risk = model.Risk
            };

            special.Diseases.Add(result);
            await _specialRepo.SaveAsync();

            return Ok(new BaseResponse(true, new List<string> { "Success" }, new GetDiseaseDetailsDto
            {
                Id = result.Id,
                Description = result.Description,
                Name = result.Name,
                Preventions = result.Preventions,
                Risk = result.Risk
            }));
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
            if(id > 0)
            {
                var result = await _diseaseRepo.GetByIdAsync(id);
                if (result != null)
                    return Ok(new BaseResponse(true, new List<string> { "Success" }, new GetDiseaseDetailsDto
                    {
                        Id = id,
                        Description = result.Description,
                        Name = result.Name,
                        Preventions = result.Preventions,
                        Risk = result.Risk
                    }));
                else
                    return NotFound();
            }
            return BadRequest(new BaseResponse(false, new List<string> { "id not valid" }, null));
        }
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _diseaseRepo.GetAllAsync("");
            var diseases = new List<GetAllDiseaseDto>();

            foreach(var dis in result)
            {
                diseases.Add(new GetAllDiseaseDto { Id = dis.Id, Name = dis.Name });
            }

            return Ok(new BaseResponse(true, new List<string> { "Success" }, diseases));
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("dis-delete")]
        public async Task<IActionResult> Delete(int id)
        {
            // check the properties validation
            if (!ModelState.IsValid)
            {
                return BadRequest(new BaseResponse(state: false, message: ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage).ToList(), null));
            }
            if (id > 0)
            {
                await _diseaseRepo.DeleteAsync(id);
                await _diseaseRepo.SaveAsync();
                return NoContent();
            }
            return BadRequest(new BaseResponse(false, new List<string> { "id not valid" }, null));
        }
    }
}
