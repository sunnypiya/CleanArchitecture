using PTG.NextStep.Domain;
using PTG.NextStep.Domain.DTO;
using AutoMapper;
using PTG.NextStep.Service.Extensions;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Threading.Channels;
using PTG.NextStep.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace PTG.NextStep.Service
{
    public class MemberService: IMemberService
    {
        private IMemberRepository _memberRepository;
        private IMapper _mapper;
        private readonly ILogger<MemberService> _logger;

        public MemberService(IMemberRepository memberRepository
            , IMapper mapper
            , IValidationDictionary validationDictionary
            , ILogger<MemberService> logger)
        {
            _memberRepository = memberRepository;
            _mapper = mapper;       
            _logger = logger;
        }
        public async Task<MemberBasicDTO> GetMemberBasicDetailsByEmployeeNumberAsync(string employeeNumber)
        {
            try
            {
                var member = await _memberRepository.GetMemberByEmployeeNumberAsync(employeeNumber);
                return _mapper.Map<MemberBasicDTO>(member);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while fetching Member Basic details by Employee Number: {ex.Message}",ex);
                throw;
            }
        }
        public async Task<bool> SaveMemberBasicInfoAsync(MemberBasicDTO memberBasicDTO, IValidationDictionary _validationDictionary)
        {
            try
            {
                var memberBasicToUpdate = await _memberRepository.GetMemberByEmployeeNumberAsync(memberBasicDTO.EmployeeNumber);
            if (memberBasicToUpdate == null)
            {
                _validationDictionary.AddError("EmployeeNumber","Member Basic detail record is not available");
                return false;
            }
            if (!memberBasicToUpdate.ArePropertiesEqual(memberBasicDTO, "EmployeeNumber"))
            {
                _validationDictionary.AddError("EmployeeNumber", "Employee Number is maintained by the system. It can not be changed.");
            }
            if (!memberBasicToUpdate.ArePropertiesEqual(memberBasicDTO, "MembershipDateCurrBoard"))
            {
                _validationDictionary.AddWarning("MembershipDateCurrBoard", "Current Board Membership Date should be changed from the Member Status History screen by changing the earliest Enrolled status record for the current board.");
            }
            if (!memberBasicToUpdate.ArePropertiesEqual(memberBasicDTO, "MembershipDateOrigBoard"))
            {
                _validationDictionary.AddWarning("MembershipDateOrigBoard", "Original Board Membership Date should be changed from the Member Status History screen by changing the earliest Enrolled status record.");
            }
            if (!memberBasicToUpdate.ArePropertiesEqual(memberBasicDTO, "TermDate"))
            {
                _validationDictionary.AddError("TermDate", "Termination Date should be changed from the Member Status History screen by entering a new or changing the most recent existing Terminated status record for the current board.");
            }
            if (!memberBasicToUpdate.ArePropertiesEqual(memberBasicDTO, "DeathDate"))
            {
                _validationDictionary.AddError("DeathDate", "Death Date should be changed from the Member Status History screen by entering a new or changing the existing Death status record.");
            }
            if (!memberBasicToUpdate.ArePropertiesEqual(memberBasicDTO, "MostRecentStatusEvent"))
            {
                _validationDictionary.AddError("MostRecentStatusEvent", "Most Recent Status Event is maintained by the system and should not be changed by the user.");
            }
            if (!memberBasicToUpdate.ArePropertiesEqual(memberBasicDTO, "MostRecentStatusEventDate"))
            {
                _validationDictionary.AddError("MostRecentStatusEventDate", "Most Recent Status Event Date is maintained by the system and should not be changed by the user.");
            }
            if (!memberBasicToUpdate.ArePropertiesEqual(memberBasicDTO, "CreditableServiceToDate"))
            {
                _validationDictionary.AddError("CreditableServiceToDate", "Creditable Service To Date can not be changed by the user. The system will calculate this field when a member is loaded or when a Status History record is changed or added.");
            }
            if (!memberBasicToUpdate.ArePropertiesEqual(memberBasicDTO, "AccumulatedDeductions"))
            {
                _validationDictionary.AddError("AccumulatedDeductions", "Accumulated Deductions can not be changed by the user. The system will calculate this field when a member is loaded or when a Annuity Savings Detail record is changed or added.");
            }            
            if (!memberBasicToUpdate.ArePropertiesEqual(memberBasicDTO, "YTDEarnings"))
            {
                _validationDictionary.AddError("YTDEarnings", "YTD Earnings can not be changed by the user. The system will calculate this field when a member is loaded or when a Deductions/Earnings History record is changed or added.");
            }
            if (!memberBasicToUpdate.ArePropertiesEqual(memberBasicDTO, "MemberGroup"))
            {
                _validationDictionary.AddError("MemberGroup", "Please remember to update the Service History screen for the member's change in position/Group.");
            }
            if (!memberBasicToUpdate.ArePropertiesEqual(memberBasicDTO, "CurrentBoard"))
            {
                _validationDictionary.AddError("CurrentBoard", "Current Board must be DefaultBoard."); // We still need to search where the data for default board comes from
            }
            if (!memberBasicToUpdate.ArePropertiesEqual(memberBasicDTO, "SSN"))
            {
                var link = await _memberRepository.CheckDuplicateSSNLink(memberBasicDTO.SSN, "MemberBeneficiary");
                if (link > 0)
                {
                    var memberBasicRecord = await _memberRepository.GetMemberBasicByLinkAsync(link);
                    if (memberBasicRecord != null)
                        _validationDictionary.AddError("SSN", $"SSN already in database for Beneficiary of {memberBasicRecord.FirstName}  {memberBasicRecord.LastName} - ( {memberBasicRecord.SSN} )");
                }
                else
                {
                    link = await _memberRepository.CheckDuplicateSSNLink(memberBasicDTO.SSN, "MemberDRO");
                    if (link > 0)
                    {
                        var memberBasicRecord = await _memberRepository.GetMemberBasicByLinkAsync(link);
                        if (memberBasicRecord != null)
                            _validationDictionary.AddError("SSN", $"SSN already in database for DRO of {memberBasicRecord.FirstName}  {memberBasicRecord.LastName} - ( {memberBasicRecord.SSN} )");
                    }
                }
                var _stringService = new StringHelper();
                memberBasicDTO.SSNLastFour = _stringService.GetLast4Digits(memberBasicDTO.SSN);
            }
            if (!memberBasicToUpdate.ArePropertiesEqual(memberBasicDTO, "CurrentUnit"))
            {
                var postingNumber = await _memberRepository.GetDERegisterBySSN(memberBasicDTO.SSN);
                if (!string.IsNullOrEmpty(postingNumber))
                    _validationDictionary.AddError("CurrentUnit", $"Member's Current Unit can not be changed. A record exists (with the member's unit) in the Deduction Register for this member that is unposted. The Posting Number is {postingNumber}.");
            }

            if (!_validationDictionary.IsValid)
                return false;

            _mapper.Map(memberBasicDTO,memberBasicToUpdate);
            await _memberRepository.SaveMemberBasicInfoAsync(memberBasicToUpdate);
            return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while saving Member Basic details by Employee Number: Message: {ex.Message}", ex);
                throw;
            }
        }

        public async Task<MemberContactDTO> GetMemberContactByLinkAsync(decimal link)
        {
            try
            {
                var memberContact = await _memberRepository.GetMemberContactByLinkAsync(link);
                return _mapper.Map<MemberContactDTO>(memberContact);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while fetching Member Contact details by Link: {ex.Message}", ex);
                throw;
            }
        }

        public async Task<bool> CreateMemberContactInfoAsync(MemberContactDTO memberContactDTO, IValidationDictionary _validationDictionary)
        {
            try
            {
                var memberContact = _mapper.Map<MemberContact>(memberContactDTO);

                var existingMemberContactRecord = await _memberRepository.GetMemberContactByLinkAsync(memberContactDTO.Link);
                if (existingMemberContactRecord != null)
                {
                    _validationDictionary.AddError("Link", "Member Contact record already exists with the given link");
                    return false;
                }

                await _memberRepository.CreateMemberContact(memberContact);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while creating Member Contact record: {ex.Message}", ex);
                throw;
            }
        }
        public async Task<bool> SaveMemberContactInfoAsync(MemberContactDTO memberContactDTO,IValidationDictionary _validationDictionary)
        {
            try
            {
                var memberContactToUpdate = await _memberRepository.GetMemberContactByLinkAsync(memberContactDTO.Link);
                if (memberContactToUpdate == null)
                {
                    _validationDictionary.AddError("Link", "Member Contact detail record is not available");
                    return false;
                }


                if (!_validationDictionary.IsValid)
                    return false;

                _mapper.Map(memberContactDTO, memberContactToUpdate);
                await _memberRepository.SaveMemberContactInfoAsync(memberContactToUpdate);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while updating Member Contact record: {ex.Message}", ex);
                throw;
            }
        }
        public async Task<IEnumerable<MemberStatusHistoryDTO>> GetMemberStatusHistoryByLinkAsync(decimal link)
        {
            try
            {
                var memberStatusHistory = await _memberRepository.GetMemberStatusHistoryByLinkAsync(link);
                return _mapper.Map<IEnumerable<MemberStatusHistoryDTO>>(memberStatusHistory);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while fetching Member Status History record: {ex.Message}", ex);
                throw;
            }
        }
        public async Task<bool> AddMemberStatusHistory(MemberStatusHistoryDTO memberStatusHistoryDTO, IValidationDictionary _validationDictionary)
        {
            try
            {
                var memberStatusHistory = _mapper.Map<MemberStatusHistory>(memberStatusHistoryDTO);
                await _memberRepository.AddMemberStatusHistory(memberStatusHistory);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while adding Member Status History record: {ex.Message}", ex);
                throw;
            }
        }
        public async Task<IEnumerable<MemberServiceHistoryDTO>> GetMemberServiceHistoryByLinkAsync(decimal link)
        {
            try
            {
                var memberServiceHistory = await _memberRepository.GetMemberServiceHistoryByLinkAsync(link);
                return _mapper.Map<IEnumerable<MemberServiceHistoryDTO>>(memberServiceHistory);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while fetching Member Service History record: {ex.Message}", ex);
                throw;
            }
        }
        public async Task<bool> AddMemberServiceHistory(MemberServiceHistoryDTO memberServiceHistoryDTO, IValidationDictionary _validationDictionary)
        {
            try
            {
                var memberServiceHistory = _mapper.Map<MemberServiceHistory>(memberServiceHistoryDTO);
                await _memberRepository.AddMemberServiceHistory(memberServiceHistory);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while adding Member Service History record: {ex.Message}", ex);
                throw;
            }
        }
        public async Task<IEnumerable<MemberDedEarnsHistoryDTO>> GetMemberDedEarnsHistoryByLinkAsync(decimal link)
        {
            try
            {
                var memberDedEarnsHistory = await _memberRepository.GetMemberDedEarnsHistoryByLinkAsync(link);
                return _mapper.Map<IEnumerable<MemberDedEarnsHistoryDTO>>(memberDedEarnsHistory);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while fetching Member Deductions and Earnings History record: {ex.Message}", ex);
                throw;
            }
        }
        public async Task<IEnumerable<MemberEarningsSummaryDTO>> GetEarningsSummaryByLinkAsync(decimal link)
        {
            try
            {
                var memberEarningsSummary = await _memberRepository.GetEarningsSummaryByLinkAsync(link);
                return _mapper.Map<IEnumerable<MemberEarningsSummaryDTO>>(memberEarningsSummary);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while fetching Member Earning Summary record: {ex.Message}", ex);
                throw;
            }
        }
        public async Task<IEnumerable<MemberBeneficiaryDTO>> GetBeneficiaryInfoList(decimal link)
        {
            try
            {
                var beneficiaryList = await _memberRepository.GetBeneficiaryInfoList(link);
                return _mapper.Map<IEnumerable<MemberBeneficiaryDTO>>(beneficiaryList);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while fetching Member Beneficiary record: {ex.Message}", ex);
                throw;
            }
        }

        public async Task<MemberPowerofAttorneyDTO> GetPowerofAttorneyByLinkAsync(decimal link)
        {
            try
            {
                var member = await _memberRepository.GetPowerofAttorneyByLinkAsync(link);
                return _mapper.Map<MemberPowerofAttorneyDTO>(member);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while fetching Power of Attorney record: {ex.Message}", ex);
                throw;
            }
        }
        public async Task<IEnumerable<MemberDependentDTO>> GetMemberDependentsInfoListByLinkAsync(decimal link)
        {
            try
            {
                var beneficiaryList = await _memberRepository.GetMemberDependentsInfoList(link);
                return _mapper.Map<IEnumerable<MemberDependentDTO>>(beneficiaryList);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while fetching Member Dependents Info record: {ex.Message}", ex);
                throw;
            }
        }

		public async Task<MemberDRODTO> GetMemberDROInfoByLinkAsync(decimal link)
		{
			try
			{
				var memberDROInfo = await _memberRepository.GetMemberDROInfoByLinkAsync(link);
                if(memberDROInfo != null)
                {
					return _mapper.Map<MemberDRODTO>(memberDROInfo);
				}
                else
                {
                    return null;
                }
				
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error while fetching Member DRO Info record: {ex.Message}", ex);
				throw;
			}
		}
	}
}
