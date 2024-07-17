using AutoMapper;
using Microsoft.Extensions.Logging;
using PTG.NextStep.Domain;
using PTG.NextStep.Domain.DTO;
using PTG.NextStep.Domain.Models;
using PTG.NextStep.Service.Validators;

namespace PTG.NextStep.Service
{
    public class DeductionService : IDeductionService
    {
        private IDeductionRepository _deductionRepository;
        private IMemberRepository _memberRepository;
        private IMapper _mapper;
        private readonly ILogger<DeductionService> _logger;
        public DeductionService(IDeductionRepository deductionRepository
            , IMapper mapper
            , ILogger<DeductionService> logger
            , IMemberRepository memberRepository)
        {
            _deductionRepository = deductionRepository;
            _mapper = mapper;
            _logger = logger;
            _memberRepository = memberRepository;
        }
        public async Task<List<string>> GetPostingNumbersByDate(int durationMonths, bool excludeAllPosted = false)
        {
            try
            {
                if (durationMonths <= 0)
                    durationMonths = 6;

                var fromDate = DateTime.Now.AddMonths(-durationMonths);
                return await _deductionRepository.GetPostingNumbersByDateAsync(DateOnly.FromDateTime(fromDate.Date), excludeAllPosted);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while fetching Posting Numbers By Date: {ex.Message}", ex);
                throw;
            }
        }
        public async Task<List<EarningsDeductionsPostingNumberDTO>> GetEarningsDeductionsPostingNumbers(int durationMonths)
        {
            try
            {
                if (durationMonths <= 0)
                    durationMonths = 6;

                var fromDate = DateTime.Now.AddMonths(-durationMonths);
                return await _deductionRepository.GetEarningsDeductionsPostingNumbersAsync(DateOnly.FromDateTime(fromDate.Date));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while fetching Earnings Deductions Posting Numbers: {ex.Message}", ex);
                throw;
            }
        }
        public async Task<IEnumerable<DERegisterDTO>> GetDERegisterByPostingNumberAsync(string postingNumber)
        {
            try
            {
                var dERegisters = await _deductionRepository.GetDERegisterByPostingNumberAsync(postingNumber);
                return _mapper.Map<IEnumerable<DERegisterDTO>>(dERegisters);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while fetching DERegisterByPostingNumber: {ex.Message}", ex);
                throw;
            }
        }
        public async Task<bool> DeleteDeductionRegisterRecordsAsync(string postingNumber, IValidationDictionary validationDictionary)
        {
            bool result = false;
            try
            {
                var existingRecords = await _deductionRepository.GetDERegisterByPostingNumberAsync(postingNumber);

                if (existingRecords.Any())
                {
                    await _deductionRepository.DeleteDeductionRegisterRecordsAsync(postingNumber);
                    result = true;
                }
                else
                {
                    validationDictionary.AddError("Message", $"DERegister not found for PostingNumber: {postingNumber}");
                }

                if (!validationDictionary.IsValid)
                    result = false;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in DeleteDeductionRegisterRecordsAsync : {ex.Message}", ex);
                throw;
            }
            return result;
        }

        public async Task<bool> CreateRegisterRecord(CreateRegisterRecordDTO createRegisterRecordDTO, IValidationDictionary _validationDictionary)
        {
            try
            {                
                await _deductionRepository.CreateRegisterRecord(createRegisterRecordDTO);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while Creating Register Records: {ex.Message}", ex);
                throw;
            }
        }

        public async Task<bool> GetRegisterRecordByPostingNumber(string postingNumber)
        {
            try
            {
                return await _deductionRepository.GetRegisterRecordByPostingNumberAsync(postingNumber);                
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while Gettting Register Records by posting number: {ex.Message}", ex);
                throw;
            }
        }
    }
}
