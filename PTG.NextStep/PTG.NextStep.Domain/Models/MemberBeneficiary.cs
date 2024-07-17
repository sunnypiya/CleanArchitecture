using Microsoft.EntityFrameworkCore;

namespace PTG.NextStep.Domain.Models
{
    [PrimaryKey(nameof(Link), nameof(BeneID))]
    public class MemberBeneficiary
    {        
        public decimal Link { get; set; }
        public decimal AuditLink { get; set; }
        public decimal BeneID { get; set; }
        public string BeneSSN { get; set; } = string.Empty;
        public string BeneFirstName { get; set; } = string.Empty;
        public string BeneLastName { get; set; } = string.Empty;
        public string BeneSSNLastFour { get; set; } = string.Empty;
        public DateOnly? BeneBirthDate { get; set; }
        public string BeneSalutation { get; set; } = string.Empty;
        public string BeneMiddleName { get; set; } = string.Empty;
        public string BeneSuffix { get; set; } = string.Empty;
        public string Address1 { get; set; } = string.Empty;
        public string Address2 { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Zipcode { get; set; } = string.Empty;
        public string ForeignAddress { get; set; } = string.Empty;
        public decimal BeginMonth { get; set; }
        public decimal BeginDay { get; set; }
        public decimal EndMonth { get; set; }
        public decimal EndDay { get; set; }
        public string BeneFullName { get; set; } = string.Empty;
        public string Comments { get; set; } = string.Empty;
        public DateOnly? LastUpdated { get; set; }
        public string EMailAddress1 { get; set; } = string.Empty;
        public string EMailAddress2 { get; set; } = string.Empty;
        public string Telephone1 { get; set; } = string.Empty;
        public string Telephone2 { get; set; } = string.Empty;
        public string BeneType { get; set; } = string.Empty;
        public string MaritalStatus { get; set; } = string.Empty;
        public string MarriageCertificate { get; set; } = string.Empty;
        public string BeneGender { get; set; } = string.Empty;
        public string OptionDFlag { get; set; } = string.Empty;
        public string SCAddress1 { get; set; } = string.Empty;
        public string SCAddress2 { get; set; } = string.Empty;
        public string SCForeignAddress { get; set; } = string.Empty;
        public string SCCity { get; set; } = string.Empty;
        public string SCState { get; set; } = string.Empty;
        public string SCZipcode { get; set; } = string.Empty;
        public string BenePrimary { get; set; } = string.Empty;
        public decimal BenePercentage { get; set; }
        public DateOnly? DeathDate { get; set; }
        public DateOnly? PopupDate { get; set; }
        public string BirthCertificateOnFile { get; set; } = string.Empty;
        public string BeneEIN { get; set; } = string.Empty;
        public string BenePERAPersonID { get; set; } = string.Empty;
        public string BenePERAEmpID { get; set; } = string.Empty;
        public string BenePERAPayeeID { get; set; } = string.Empty;
        public string BenePERAPayrollID2 { get; set; } = string.Empty;
        public string BenePERAPayrollID3 { get; set; } = string.Empty;
        public string NorwoodMasID { get; set; } = string.Empty;
    }
}
