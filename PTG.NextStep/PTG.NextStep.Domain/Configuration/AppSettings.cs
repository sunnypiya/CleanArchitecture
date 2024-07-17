using PTG.NextStep.Domain.DTO;

namespace PTG.NextStep.Domain
{
    public class ElasticSearch
    {
        public string Url { get; set; } = string.Empty;
        public string IndexPrefix { get; set; } = string.Empty;

    }

    public class AppSettings
    { 
        public ElasticSearch ElasticSearch { get; set; } = new ElasticSearch();
        public string DefaultTenant { get; set; } = "demomass";

        public List<TenantDTO> Tenants { get; set; } = new List<TenantDTO>();
    }
}