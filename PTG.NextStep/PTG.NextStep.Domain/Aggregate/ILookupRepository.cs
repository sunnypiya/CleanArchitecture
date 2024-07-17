using PTG.NextStep.Domain.DTO;

namespace PTG.NextStep.Domain
{
    public interface ILookupRepository
    {
        Task<IEnumerable<PostingCodesDTO>> GetPostingCodesAsync();
    }
}
