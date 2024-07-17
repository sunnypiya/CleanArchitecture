using PTG.NextStep.Domain;
using PTG.NextStep.Domain.Models;

namespace PTG.NextStep.Database.Search
{
    public interface ISearchService
    {
        Task IndexData();
        Task<bool> DeleteIndexDataAsync();
        Task<IReadOnlyCollection<MemberBasic>> SearchData(string keyword);
    }
}
