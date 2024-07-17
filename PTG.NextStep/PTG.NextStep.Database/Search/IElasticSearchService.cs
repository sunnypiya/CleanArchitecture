using Nest;

namespace PTG.NextStep.Database.Search
{
    public interface IElasticSearchService
    {
        IElasticClient Client { get; }
    }
}
