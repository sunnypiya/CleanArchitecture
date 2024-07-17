using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using PTG.NextStep.Domain;

namespace PTG.NextStep.Tenant
{
    public class MultiTenancyMiddlewareBackgroundService: IBackgroundServiceMiddleware
    {        
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMultiTenancyService _tenantService;
        private readonly IDistributedCache _cache;

        public MultiTenancyMiddlewareBackgroundService(IHttpContextAccessor httpContextAccessor
            , IMultiTenancyService tenantService
            ,IDistributedCache cache)
        {
            _httpContextAccessor = httpContextAccessor;  
            _tenantService = tenantService;
            _cache = cache;
        }   
        public async Task InvokeAsync(CancellationToken cancellationToken, Func<CancellationToken, Task> next)
        {
            DetermineAndSetTenant();

            await next(cancellationToken);
        }

        private async void DetermineAndSetTenant()
        {            
            string userId = _httpContextAccessor.HttpContext?.Request.Headers["userid"].ToString();
            _tenantService.SetSessionId(userId);
            string tenant = await _cache.GetStringAsync(_tenantService.TenantCacheKey) ?? string.Empty;            
            if (string.IsNullOrWhiteSpace(tenant))
            {
                tenant = _tenantService.DefaultTenant;
                
            }
            _tenantService.SetTenant(tenant);
        }

    }
}
