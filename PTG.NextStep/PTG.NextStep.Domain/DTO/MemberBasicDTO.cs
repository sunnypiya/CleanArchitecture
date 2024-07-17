namespace PTG.NextStep.Domain.DTO
{
    public class MemberBasicDTO
    {
        public decimal Link { get; set; }
        public string EmployeeNumber { get; set; } = string.Empty;
        public string SSN { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string CurrentBoard { get; set; } = string.Empty;
        public string Salutation { get; set; } = string.Empty;
        public string MemberGroup { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string MaritalStatus { get; set; } = string.Empty;

        public string SSNLastFour { get; set; } = string.Empty;
        public decimal CreditableServiceToDate { get; set; }
        public string CurrentUnit { get; set; } = string.Empty;
        public string CurrentDepartment { get; set; } = string.Empty;
        public string Suffix { get; set; } = string.Empty;
        public decimal YTDEarnings { get; set; }

        public string MostRecentStatusEvent { get; set; } = string.Empty;
        public string ContributionRate { get; set; } = string.Empty;
        public string TwoPercentContribution { get; set; } = string.Empty;
        public string DROFlag { get; set; } = string.Empty;
        public string VeteranService { get; set; } = string.Empty;
        public decimal MemberNumber { get; set; }
        public string MiddleName { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;

        public string Section28NFlag { get; set; } = string.Empty;
        public decimal FederalWithholding { get; set; }
        public decimal RolloverAmount { get; set; }
        public string RolloverTo { get; set; } = string.Empty;
        public decimal AccumulatedDeductions { get; set; }

        public string SSNTieTo { get; set; } = string.Empty;
        public string TACSRetiree { get; set; } = string.Empty;
        public decimal TACSRetireeNumber { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string BannerStatus { get; set; } = string.Empty;
        public string BannerPayeeName { get; set; } = string.Empty;
        public string CurrentServicePercentage { get; set; } = string.Empty;
        public decimal CreditableServiceGroupCalc { get; set; }

        public string TenYearTransferFlag { get; set; } = string.Empty;
        public string PowerOfAttorney { get; set; } = string.Empty;
        public string ElectedOfficialNoCredSvc { get; set; } = string.Empty;
        public string EnrolledAfter2012 { get; set; } = string.Empty;
        public string ESSCollectedData { get; set; } = string.Empty;
        public string BoardUseFlag { get; set; } = string.Empty;
        public string CollectBargainPosition { get; set; } = string.Empty;
        public string DataNationalStatus { get; set; } = string.Empty;

        public string UnionCode { get; set; } = string.Empty;
        public string PriorName { get; set; } = string.Empty;
        public string ActiveMilitary { get; set; } = string.Empty;
        public string WorkersComp { get; set; } = string.Empty;
        public string ValidationNeeded { get; set; } = string.Empty;

        // Date Members
        public DateOnly? BirthDate { get; set; }
        public DateOnly? DeathDate { get; set; }
        public DateOnly? TermDate { get; set; }
        public DateOnly? MembershipDateCurrBoard { get; set; }
        public DateOnly? PayrollCreatedDate { get; set; }
        public DateOnly? ESSRetirementDate { get; set; }
        public DateOnly? MostRecentStatusEventDate { get; set; }
        public DateOnly? HireDate { get; set; }
        public DateOnly? MembershipDateOrigBoard { get; set; }
    }
}
