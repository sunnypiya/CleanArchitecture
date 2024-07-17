using PTG.NextStep.Domain;
using PTG.NextStep.Domain.DTO;

namespace PTG.NextStep.Service
{
    public interface IDeductionService
    {
        Task<List<string>> GetPostingNumbersByDate(int durationMonths, bool excludeAllPosted = false);
        Task<List<EarningsDeductionsPostingNumberDTO>> GetEarningsDeductionsPostingNumbers(int durationMonths);
        Task<IEnumerable<DERegisterDTO>> GetDERegisterByPostingNumberAsync(string postingNumber);
        Task<bool> DeleteDeductionRegisterRecordsAsync(string postingNumber, IValidationDictionary validationDictionary);
        Task<bool> CreateRegisterRecord(CreateRegisterRecordDTO createRegisterRecordDTO, IValidationDictionary _validationDictionary);
        Task<bool> GetRegisterRecordByPostingNumber(string postingNumber);
    }
}
