using Microsoft.Extensions.Options;
using Nest;
using PTG.NextStep.Domain;

namespace PTG.NextStep.Database.Search
{
    public class ElasticSearchService : IElasticSearchService
    {
        private readonly IElasticClient _client;        
        private readonly AppSettings _appSettings;

        public ElasticSearchService(IOptions<AppSettings> appSettings)
        {
            var settings = new ConnectionSettings(new Uri(appSettings.Value.ElasticSearch.Url)).EnableApiVersioningHeader();
            _appSettings = appSettings.Value;
            _client = new ElasticClient(settings);
        }

        public IElasticClient Client => _client;
    }
}
