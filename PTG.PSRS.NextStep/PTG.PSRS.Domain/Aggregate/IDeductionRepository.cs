namespace PTG.PSRS.Domain
{
    public interface IDeductionRepository
    {
        Task<List<string>> GetPostingNumbersByDate(DateOnly fromDate);
        Task DeleteDeductionRegisterRecordsAsync(string postingNumber);

	}
}
