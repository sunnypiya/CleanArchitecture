namespace PTG.NextStep.Tenant
{
    public interface IMultiTenancyService
    {
        string DefaultTenant { get; }
        string Tenant { get; }
        string SessionId { get; }
        string TenantCacheKey { get; }

        void SetSessionId(string sessionId);
        void SetTenant(string tenant);

        string[] GetTenants();

        delegate void TenantChangedEventHandler(object? sender, TenantChangedEventArgs e);

        event TenantChangedEventHandler OnTenantChanged;

    }
}
