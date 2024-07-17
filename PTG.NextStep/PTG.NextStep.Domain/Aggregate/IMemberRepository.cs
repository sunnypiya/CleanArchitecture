using PTG.NextStep.Domain.DTO;
using PTG.NextStep.Domain.Models;
using System.Linq.Expressions;

namespace PTG.NextStep.Domain
{
    public interface IMemberRepository
    {
        Task<IEnumerable<MemberBasic>> GetMembersAsync(Expression<Func<MemberBasic, bool>>? predicate = null);
        
        Task<MemberBasic> GetMemberBasicByLinkAsync(decimal link);
        Task<MemberBasic> GetMemberByEmployeeNumberAsync(string employeeNumber);
        Task<MemberContact> GetMemberContactByLinkAsync(decimal link);
        Task<IEnumerable<MemberStatusHistory>> GetMemberStatusHistoryByLinkAsync(decimal link);
        Task<IEnumerable<MemberServiceHistory>> GetMemberServiceHistoryByLinkAsync(decimal link);
        Task<IEnumerable<MemberDedEarnsHistory>> GetMemberDedEarnsHistoryByLinkAsync(decimal link);
        Task<bool> AddMemberStatusHistory(MemberStatusHistory memberStatusHistory);

        Task<bool> AddMemberServiceHistory(MemberServiceHistory memberServiceHistory);
        Task<bool> CreateMemberContact(MemberContact memberContact);        
        Task SaveMemberBasicInfoAsync(MemberBasic memberBasic);
        Task SaveMemberContactInfoAsync(MemberContact memberContact);

        Task<IEnumerable<MemberEarningsSummary>> GetEarningsSummaryByLinkAsync(decimal link);
        Task<decimal> CheckDuplicateSSNLink(string SSN, string tableToCheck);
        Task<string> GetDERegisterBySSN(string SSN);
        Task<IEnumerable<MemberBeneficiary>> GetBeneficiaryInfoList(decimal link);
        Task<MemberPowerofAttorney> GetPowerofAttorneyByLinkAsync(decimal link);
        Task<IEnumerable<MemberDependent>> GetMemberDependentsInfoList(decimal link);
		Task<MemberDRO> GetMemberDROInfoByLinkAsync(decimal link);
	}
}
