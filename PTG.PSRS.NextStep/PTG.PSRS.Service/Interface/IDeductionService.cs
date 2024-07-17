using PTG.PSRS.Domain.DTO;

namespace PTG.PSRS.Service
{
    public interface IDeductionService
    {
        Task<List<string>> GetPostingNumbersByDate(int durationMonths);
        Task<bool> DeleteDeductionRegisterRecordsAsync(string postingNumber);

	}
}
