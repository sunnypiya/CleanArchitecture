using PTG.NextStep.Domain.DTO;

namespace PTG.NextStep.Service
{
    public interface ILookupService
    {
        Task<IEnumerable<PostingCodesDTO>> GetPostingCodesAsync();
    }
}
