using PTG.NextStep.Domain.DTO;
using PTG.NextStep.Domain.Models;

namespace PTG.NextStep.Domain
{
    public interface IDeductionRepository
    {
        Task<List<string>> GetPostingNumbersByDateAsync(DateOnly fromDate, bool excludeAllPosted = false);
        Task<List<EarningsDeductionsPostingNumberDTO>> GetEarningsDeductionsPostingNumbersAsync(DateOnly fromDate);
        Task<IEnumerable<DERegister>> GetDERegisterByPostingNumberAsync(string postingNumber);
        Task DeleteDeductionRegisterRecordsAsync(string postingNumber);

        Task<bool> SaveDERegisterAsync(DERegister dERegister);
        Task<bool> CreateRegisterRecord(CreateRegisterRecordDTO createRegisterRecordDTO);
        Task<bool> GetRegisterRecordByPostingNumberAsync(string postingNumber);
    }
}
