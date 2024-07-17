using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PTG.NextStep.Database.Search;
using PTG.NextStep.Domain;

namespace PTG.NextStep.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ElasticSearchController : ControllerBase
    {
        private readonly ILogger<ElasticSearchController> _logger;
        private readonly ISearchService _searchService;

        public ElasticSearchController(ILogger<ElasticSearchController> logger, ISearchService searchService)
        {
            _logger = logger;
            _searchService = searchService;
        }
        [HttpGet("index")]
        public async Task<IActionResult> IndexData()
        {
            await _searchService.IndexData();

            var response = new { Message = "Data indexed successfully" };
            return new JsonResult(response)
            {
                ContentType = "application/json"
            };
        }
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteIndexData()
        {
            try
            {
                _logger.LogInformation("DeleteIndexData() called");
                var isDeleted = await _searchService.DeleteIndexDataAsync();

                if (isDeleted)
                {
                    var response = new { Message = "Index Data deleted successfully" };
                    return new JsonResult(response)
                    {
                        ContentType = "application/json"
                    };
                }
                else
                {
                    return BadRequest("Error while Deleting ElasticSearch Index data.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in method:DeleteIndexData: Message:{ex.Message}", ex);
                return BadRequest(new
                {
                    Error = "Error while Deleting ElasticSearch Index data."
                });
            }
        }
    }
}