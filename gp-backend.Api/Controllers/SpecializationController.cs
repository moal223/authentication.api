using gp_backend.Api.Dtos;
using gp_backend.Api.Dtos.Disease;
using gp_backend.Api.Dtos.Special;
using gp_backend.EF.MySql.Repositories.Interfaces;
using gp_backend.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace gp_backend.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class SpecializationController : ControllerBase
    {
        private readonly ISpecialRepo _specialRepo;
        private readonly ILogger<DiseaseController> _logger;

        public SpecializationController(ISpecialRepo specialRepo, ILogger<DiseaseController> logger)
        {
            _specialRepo = specialRepo;
            _logger = logger;
        }

        // Add
        [HttpPost]
        public async Task<IActionResult> Add([FromForm] AddSpecialDto model)
        {
            // check the properties validation
            if (!ModelState.IsValid)
            {
                return BadRequest(new BaseResponse(state: false, message: ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage).ToList(), null));
            }

            var result = await _specialRepo.InsertAsync(new Specialization { Name = model.Name });
            await _specialRepo.SaveAsync();

            return Ok(new BaseResponse(state: true, message: new List<string> { "Success"}, data: new GetSpecialDto
            {
                Id = result.Id, 
                Name = result.Name
            }));
        }

        // get all
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _specialRepo.GetAllAsync("");
            var listDto = new List<GetAllDto>();
            foreach (var item in result)
                listDto.Add(new GetAllDto
                {
                    Id = item.Id,
                    Name = item.Name
                });
            return Ok(new BaseResponse(state: true, message: new List<string> { "Success" }, data: listDto));
        }
        [HttpGet("get-id")]
        public async Task<IActionResult> GetById(int id)
        {
            // check the specialization id
            if (id <= 0)
            {
                return BadRequest(new BaseResponse(state: false, message: new List<string> { "Invalid specialization." }, null));
            }

            var special = await _specialRepo.GetByIdAsync(id);
            var diseases = special.Diseases;
            var diseasesDto = new List<GetDiseaseDetailsDto>();
            foreach (var item in diseases)
                diseasesDto.Add(new GetDiseaseDetailsDto
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    Preventions = item.Preventions,
                    Risk = item.Risk
                });

            return Ok(new BaseResponse(state: true, message: new List<string> { "Success" }, data: new GetSpecialDto
            {
                Id = id,
                Name = special.Name,
                Diseases = diseasesDto
            }));
        }
    }
}
