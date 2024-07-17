using Microsoft.EntityFrameworkCore;

namespace PTG.NextStep.Domain.Models
{
    [PrimaryKey(nameof(Link), nameof(RetirementBoard))]
    public class MemberServiceHistory
    {
        public decimal Link  { get; set; }
        public decimal AuditLink { get; set; }
        public string RetirementBoard { get; set; } = string.Empty;
        public DateOnly BeginDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string AdjustmentDescription { get; set; } = string.Empty;
        public decimal ServiceAdjustment { get; set; }
        public decimal Service { get; set; }
        public string Open { get; set; } = string.Empty;
        public string BuybackMakeupFlag { get; set; } = string.Empty;
        public string ServiceType { get; set; } = string.Empty;
        public string ServicePercentage { get; set; } = string.Empty;
        public string XferRecord { get; set; } = string.Empty;
        public string MemberGroup { get; set; } = string.Empty;
    }
}
