using PTG.NextStep.Domain.DTO;

namespace PTG.NextStep.Service
{
    public interface ITenantService
    {
        Task<IEnumerable<TenantDTO>> GetTenantsAsync();
        Task<bool> SetTenantAsync(string tenant);

    }
}
