namespace PTG.NextStep.Domain.DTO
{
    public class MemberDependentDTO
    {
        public decimal Link { get; set; }
        public decimal AuditLink { get; set; }
        public decimal DepID { get; set; }
        public string DepFirstName { get; set; } = string.Empty;
        public string DepLastName { get; set; } = string.Empty;
        public string DepMiddleName { get; set; } = string.Empty;
        public string DepSuffix { get; set; } = string.Empty;
        public DateOnly? DepBirthDate { get; set; }
        public string DepType { get; set; } = string.Empty;
        public string DepSSN { get; set; } = string.Empty;
        public string DepSSNLastFour { get; set; } = string.Empty;
        public DateOnly? DepDeathDate { get; set; }
        public string DepSalutation { get; set; } = string.Empty;
        public string DepPERAPersonID { get; set; } = string.Empty;
        public DateOnly? StudentCertDate { get; set; }
        public string Comments { get; set; } = string.Empty;
        public string PaymentTermReason { get; set; } = string.Empty;
        public DateOnly? PaymentTermDate { get; set; }
    }
}
