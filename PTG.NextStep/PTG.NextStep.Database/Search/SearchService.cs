using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Nest;
using PTG.NextStep.Domain;
using PTG.NextStep.Domain.Models;
using PTG.NextStep.Tenant;

namespace PTG.NextStep.Database.Search
{
    public class SearchService : ISearchService
    {
        private IElasticSearchService _elasticSearchService;
        private IMultiTenancyService _tenantService;
        private string _index;
        private readonly IDbContextFactory<ApplicationDbContext> _dbFactory;

        public SearchService(IElasticSearchService elasticSearchService, IMultiTenancyService tenantService, IOptions<AppSettings> appSettings, IDbContextFactory<ApplicationDbContext> dbFactory)
        {
            _elasticSearchService = elasticSearchService;
            _tenantService = tenantService;
            _index = $"{appSettings.Value.ElasticSearch.IndexPrefix}-{_tenantService.Tenant}";
            _dbFactory = dbFactory;
        }

        public async Task IndexData()
        {
            using (var context = _dbFactory.CreateDbContext())
            {
                var data = await context.MemberBasic.ToListAsync();

                await _elasticSearchService.Client.IndexManyAsync<MemberBasic>(data, index: _index);
            }
        }

        public async Task<IReadOnlyCollection<MemberBasic>> SearchData(string keyword)
        {
            var searchData = await _elasticSearchService.Client.SearchAsync<MemberBasic>(s => s
             .Index(_index)
             .Query(q => q
                 .MultiMatch(m => m
                     .Fields(f => f
                         .Field(ff => ff.FirstName)
                         .Field(ff => ff.LastName)
                         .Field(ff => ff.SSN)
                         .Field(ff => ff.SSNLastFour)
                         .Field(ff => ff.Suffix)
                         .Field(ff => ff.PriorName)
                         .Field(ff => ff.MiddleName)
                         .Field(ff => ff.EmployeeNumber)
                         ).Query(keyword)
                         .Fuzziness(Fuzziness.Auto)
                         )));
            return searchData.Documents;
        }

        public async Task<bool> DeleteIndexDataAsync()
        {
            var deleteIndexResponse = await _elasticSearchService.Client.Indices.DeleteAsync(_index);
            return deleteIndexResponse.IsValid;
        }
    }
}
