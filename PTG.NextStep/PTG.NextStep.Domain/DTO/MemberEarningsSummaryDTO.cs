namespace PTG.NextStep.Domain.DTO
{
    public class MemberEarningsSummaryDTO
    {
        public decimal Link { get; set; }
        public decimal AuditLink { get; set; }
        public decimal Year { get; set; }
        public decimal Earnings { get; set; }
        public decimal EarningsNonPensionable { get; set; }
    }
}
