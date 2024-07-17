using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PTG.NextStep.Tenant
{
    public class MultiTenancyMiddleware
    {
        private readonly RequestDelegate _next;

        public MultiTenancyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, IDistributedCache cache, IMultiTenancyService tenantService)
        {
            DetermineAndSetTenant(httpContext, cache, tenantService);

            await _next(httpContext);
        }

        private async void DetermineAndSetTenant(HttpContext context, IDistributedCache cache, IMultiTenancyService tenantService)
        {
            //var user = context.User;
            //if (user.HasClaim(c => c.Type == "tenant"))
            //{
            //    return user.FindFirst("tenant")?.Value ?? string.Empty;
            //}
            //string tenant = context.Request.Query["tenant"].ToString();
            string userId = context.Request.Headers["userid"].ToString();
            tenantService.SetSessionId(userId);
            string tenant = await cache.GetStringAsync(tenantService.TenantCacheKey) ?? string.Empty;
            if (string.IsNullOrWhiteSpace(tenant))
            {
                tenant = tenantService.DefaultTenant;
                await cache.SetStringAsync(tenantService.TenantCacheKey, tenant);
            }
            tenantService.SetTenant(tenant);
        }
    }
 
    public static class TenantMiddlewareExtensions
    {
        public static IApplicationBuilder UseMultiTenancy(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MultiTenancyMiddleware>();
        }
    }

}
