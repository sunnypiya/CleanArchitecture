namespace PTG.NextStep.Domain.DTO
{
    public class DERegisterDTO
    {
        public string PostingNumber { get; set; } = string.Empty;
        public string MemberSSN { get; set; } = string.Empty;
        public DateOnly? DeductionPostingDate { get; set; }
        public decimal MemberLink { get; set; }
        public decimal AuditLink { get; set; }
        public decimal DeductionAmount { get; set; }
        public decimal DeductionIncrementalAmount { get; set; }

        public string MemberName { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public decimal ContributionRate { get; set; }

        public string Modified { get; set; } = string.Empty;
        public decimal OriginalDeductionAmount { get; set; }
        public decimal OrigDeductionIncrementalAmt { get; set; }
        public decimal MemberSSNSort { get; set; }
        public string MemberSSNLastFour { get; set; } = string.Empty;
        public decimal MemberNameSort { get; set; }
        public string PostingCode { get; set; } = string.Empty;
        public string PostedFlag { get; set; } = string.Empty;
        public decimal PayrollAmount { get; set; }
    }
}
