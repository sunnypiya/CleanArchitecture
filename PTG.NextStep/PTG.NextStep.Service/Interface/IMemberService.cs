using PTG.NextStep.Domain;
using PTG.NextStep.Domain.DTO;
using PTG.NextStep.Domain.Models;

namespace PTG.NextStep.Service
{
    public interface IMemberService
    {
        Task<MemberBasicDTO> GetMemberBasicDetailsByEmployeeNumberAsync(string employeeNumber);        
        Task<bool> SaveMemberBasicInfoAsync(MemberBasicDTO memberBasicDTO, IValidationDictionary _validationDictionary);
        Task<bool> SaveMemberContactInfoAsync(MemberContactDTO memberContactDTO, IValidationDictionary _validationDictionary);
        Task<MemberContactDTO> GetMemberContactByLinkAsync(decimal link);
        Task<bool> CreateMemberContactInfoAsync(MemberContactDTO memberContactDTO, IValidationDictionary _validationDictionary);
        Task<IEnumerable<MemberStatusHistoryDTO>> GetMemberStatusHistoryByLinkAsync(decimal link);
        Task<bool> AddMemberStatusHistory(MemberStatusHistoryDTO memberStatusHistory, IValidationDictionary _validationDictionary);
        Task<IEnumerable<MemberServiceHistoryDTO>> GetMemberServiceHistoryByLinkAsync(decimal link);
        Task<bool> AddMemberServiceHistory(MemberServiceHistoryDTO memberServiceHistoryDTO, IValidationDictionary _validationDictionary);
        Task<IEnumerable<MemberDedEarnsHistoryDTO>> GetMemberDedEarnsHistoryByLinkAsync(decimal link);
        Task<IEnumerable<MemberEarningsSummaryDTO>> GetEarningsSummaryByLinkAsync(decimal link);
        Task<IEnumerable<MemberBeneficiaryDTO>> GetBeneficiaryInfoList(decimal link);
        Task<MemberPowerofAttorneyDTO> GetPowerofAttorneyByLinkAsync(decimal link);
        Task<IEnumerable<MemberDependentDTO>> GetMemberDependentsInfoListByLinkAsync(decimal link);
		Task<MemberDRODTO> GetMemberDROInfoByLinkAsync(decimal link);
	}
}
