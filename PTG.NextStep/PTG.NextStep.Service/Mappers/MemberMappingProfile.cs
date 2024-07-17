using AutoMapper;
using PTG.NextStep.Domain.DTO;
using PTG.NextStep.Domain;
using PTG.NextStep.Domain.Models;

namespace PTG.NextStep.Service.Mappers
{
    public class MemberMappingProfile:Profile
    {
        public MemberMappingProfile()
        {
            CreateMap<MemberBasic, MemberBasicDTO>().ReverseMap();
            CreateMap<MemberContact, MemberContactDTO>().ReverseMap();
            CreateMap<MemberBeneficiary, MemberBeneficiaryDTO>().ReverseMap();
            CreateMap<MemberPowerofAttorney, MemberPowerofAttorneyDTO>().ReverseMap();
            CreateMap<MemberDependent, MemberDependentDTO>().ReverseMap();
            CreateMap<MemberStatusHistory, MemberStatusHistoryDTO>().ReverseMap();

            CreateMap<DERegister, DERegisterDTO > ().ReverseMap();
            CreateMap<MemberDedEarnsHistory, MemberDedEarnsHistoryDTO>().ReverseMap();
            CreateMap<MemberDRO, MemberDRODTO>().ReverseMap();
            CreateMap<MemberEarningsSummary, MemberEarningsSummaryDTO>().ReverseMap();
            CreateMap<MemberServiceHistory, MemberServiceHistoryDTO>().ReverseMap();

        }
    }
}
