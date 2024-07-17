using Microsoft.Extensions.Options;
using PTG.NextStep.Domain;

namespace PTG.NextStep.Tenant
{
    public class MultiTenancyService : IMultiTenancyService
    {
        private string _tenant;
        private string _sessionId;
        private string _tenantCacheKey;
        private readonly AppSettings _appSettings;

        public MultiTenancyService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            SetSessionId(string.Empty);
            SetTenant(string.Empty);
        }

        public string DefaultTenant { get { return _appSettings.DefaultTenant; } }

        public string Tenant { get { return _tenant; } }

        public string SessionId { get { return _sessionId; } }

        public string TenantCacheKey { get { return _tenantCacheKey; } }

        public event IMultiTenancyService.TenantChangedEventHandler? OnTenantChanged;

        public string[] GetTenants()
        {
            return _appSettings.Tenants.Select(t => t.Id).ToArray();
        }

        public void SetSessionId(string sessionId)
        {
            _sessionId = sessionId;
            _tenantCacheKey = $"PTG:NextStep:API:session:{sessionId}:tenant";
        }

        public void SetTenant(string tenant)
        {
            _tenant = tenant;
        }

    }
}
