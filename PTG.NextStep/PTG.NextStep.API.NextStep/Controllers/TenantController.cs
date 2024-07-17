using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using PTG.NextStep.Tenant;
using PTG.NextStep.API.NextStep.MockData;
using PTG.NextStep.Database.Search;
using PTG.NextStep.Service;
using PTG.NextStep.Domain;
using PTG.NextStep.Domain.Models;
using PTG.NextStep.Domain.DTO;
using FluentValidation;
using System.Linq;

namespace PTG.NextStep.API.NextStep.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TenantController : ControllerBase
    {
        private readonly ILogger<TenantController> _logger;
        private readonly IDistributedCache _cache;
        private readonly IMultiTenancyService _tenantService;

        public TenantController(ILogger<TenantController> logger, IDistributedCache cache, IMultiTenancyService tenantService)
        {
            _logger = logger;
            _cache = cache;
            _tenantService = tenantService;
        }

        /// <summary>
        /// Get tenants for user
        /// </summary>
        /// <returns></returns>
        [HttpGet("tenants")]
        public async Task<IActionResult> GetTenantsAsync()
        {
            try
            {
                _logger.LogInformation("GetTenantsAsync called");
                var tenants = _tenantService.GetTenants().Select<string,TenantDTO>(s => new TenantDTO { Id = s }).ToList(); 
                return Ok(tenants);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in method:GetTenantsAsync: Message: {ex.Message}", ex);
                return BadRequest(new
                {
                    Error = "Error while fetching tenants.",
                });
            }
        }

        /// <summary>
        /// Set tenant for session
        /// </summary>
        /// <param name="tenant"></param>
        /// <returns></returns>
        [HttpPut("set-tenant")]
        public async Task<IActionResult> SetTenant(string tenant)
        {
            try
            {
                _logger.LogInformation("SetTenant() called");
                await _cache.SetStringAsync(_tenantService.TenantCacheKey, tenant);
                _tenantService.SetTenant(tenant);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in method:SetTenant: Message:{ex.Message}", ex);
                return BadRequest(new
                {
                    Error = "Error while setting tenant for session."
                });
            }
        }
    }
}
