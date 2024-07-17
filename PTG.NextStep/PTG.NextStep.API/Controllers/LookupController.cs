using Microsoft.AspNetCore.Mvc;
using PTG.NextStep.Domain.DTO;
using PTG.NextStep.Service;

namespace PTG.NextStep.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LookupController : ControllerBase
    {
        private readonly ILogger<LookupController> _logger;
        private readonly ILookupService _lookupService;
        public LookupController(ILogger<LookupController> logger
            , ILookupService lookupService
            )
        {
            _logger = logger;
            _lookupService = lookupService;
        }

        [HttpGet("postingcodes")]
        public async Task<IActionResult> GetPostingCodes()
        {
            try
            {
                var postingCodes = await _lookupService.GetPostingCodesAsync();
                return Ok(postingCodes);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in method:GetPostingCodes: {ex.Message}", ex);
                return BadRequest(new
                {
                    Error = "Error while fetching Posting Codes.",
                    
                });
            }
        }
    }
}
