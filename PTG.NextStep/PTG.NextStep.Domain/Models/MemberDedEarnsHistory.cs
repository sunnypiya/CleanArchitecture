using Microsoft.EntityFrameworkCore;

namespace PTG.NextStep.Domain.Models
{
    [PrimaryKey(nameof(Link), nameof(RetirementBoard), nameof(PostingNumber), nameof(Unit))]
    public class MemberDedEarnsHistory
    {
        public decimal Link { get; set; }
        public decimal AuditLink { get; set; }
        public string RetirementBoard { get; set; } = string.Empty;
        public DateOnly DeductionPostingDate { get; set; }
        public decimal PayrollAmount { get; set; }
        public decimal DeductionAmount { get; set; }
        public decimal ContributionRate { get; set; }
        public decimal DeductionIncrementalAmount { get; set; }
        public string PostingCode { get; set; } = string.Empty;
        public string PostingNumber { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public string XferRecord { get; set; } = string.Empty;        
    }
}
