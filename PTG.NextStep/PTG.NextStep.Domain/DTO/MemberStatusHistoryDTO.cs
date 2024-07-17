namespace PTG.NextStep.Domain.DTO
{
    public class MemberStatusHistoryDTO
    {
        public decimal Link { get; set; }
        public decimal AuditLink { get; set; }
        public DateOnly StatusDate { get; set; }
        public string RetirementBoard { get; set; } = string.Empty;
        public string StatusEvent { get; set; } = string.Empty;
        public string XferRecord { get; set; } = string.Empty;
    }
}
