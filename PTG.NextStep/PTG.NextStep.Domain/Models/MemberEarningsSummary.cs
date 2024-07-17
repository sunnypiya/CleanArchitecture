using Microsoft.EntityFrameworkCore;

namespace PTG.NextStep.Domain.Models
{
    [PrimaryKey(nameof(Link), nameof(Year))]
    public class MemberEarningsSummary
    {
        public decimal Link { get; set; }
        public decimal AuditLink { get; set; }
        public decimal Year{ get; set; }
        public decimal Earnings { get; set; }
        public decimal EarningsNonPensionable { get; set; }
    }
}
