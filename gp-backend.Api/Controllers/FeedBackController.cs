using gp_backend.Api.Dtos;
using gp_backend.Api.Dtos.Disease;
using gp_backend.Api.Dtos.FeedBack;
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
    public class FeedBackController : ControllerBase
    {
        private readonly IFeedBackRepo _feedBackRepo;
        private readonly ILogger<FeedBackController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public FeedBackController(IFeedBackRepo feedBackRepo, ILogger<FeedBackController> logger, 
            UserManager<ApplicationUser> userManager)
        {
            _feedBackRepo = feedBackRepo;
            _logger = logger;
            _userManager = userManager;
        }
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromForm] AddFeedBackDto model)
        {
            // check the properties validation
            if (!ModelState.IsValid)
            {
                return BadRequest(new BaseResponse(state: false, message: ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage).ToList(), null));
            }

            var uid = User.Claims.FirstOrDefault(x => x.Type == "uid").Value;

            var user = await _userManager.FindByIdAsync(uid);

            if(user != null)
            {
                var feedback = await _feedBackRepo.InsertAsync(new FeedBack
                {
                    Subject = model.Subject,
                    FeedBackContent = model.FeedBackContent,
                    IsRead = false,
                    ApplicationUserId = user.Id,
                    User = user
                });
                await _feedBackRepo.SaveAsync();
                return Ok(new BaseResponse(true, new List<string> { "Success" }, new GetFeedBackDto
                {
                    Id = feedback.Id,
                    Subject = feedback.Subject,
                    FeedBackContent = feedback.FeedBackContent
                }));
            }
            return BadRequest();
        }

        [HttpGet("get-by-id")]
        public async Task<IActionResult> GetById(int id)
        {
            if(id > 0)
            {
                var feedback = await _feedBackRepo.GetByIdAsync(id);
                if (feedback != null)
                {
                    if (!feedback.IsRead)
                    {
                        feedback.IsRead = true;
                        await _feedBackRepo.SaveAsync();
                    }

                    return Ok(new BaseResponse(true, new List<string> { "Success" }, new GetFeedBackDto
                    {
                        Id = feedback.Id,
                        FeedBackContent = feedback.FeedBackContent,
                        Subject = feedback.Subject,
                        IsRead = feedback.IsRead
                    }));
                } 
            }
            return BadRequest(new BaseResponse(false, new List<string> { "Invalid id"}, null));
        }

        [HttpGet("get-all-user")]
        public async Task<IActionResult> GetAll()
        {
            var uid = User.Claims.FirstOrDefault(x => x.Type == "uid").Value;

            if(uid != null)
            {
                var feedBacks = await _feedBackRepo.GetAllAsync(uid);

                var feedbacksDto = new List<GetFeedBackDto>();
                foreach (var item in feedBacks)
                    feedbacksDto.Add(new GetFeedBackDto
                    {
                        Id = item.Id,
                        Subject = item.Subject,
                        FeedBackContent = item.FeedBackContent,
                        IsRead = item.IsRead
                    });

                return Ok(new BaseResponse(true, new List<string> { "Success" }, feedbacksDto));
            }
            return BadRequest();
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllAdmin()
        {
                var feedBacks = await _feedBackRepo.GetAllAsync();

                var feedbacksDto = new List<GetFeedBackDto>();
                foreach (var item in feedBacks)
                    feedbacksDto.Add(new GetFeedBackDto
                    {
                        Id = item.Id,
                        Subject = item.Subject,
                        FeedBackContent = item.FeedBackContent,
                        IsRead = item.IsRead
                    });

                return Ok(new BaseResponse(true, new List<string> { "Success" }, feedbacksDto));
        }
    }
}
