using System.ComponentModel.DataAnnotations;

namespace PTG.NextStep.Domain.Models
{
    public class MemberContact
    {
        [Key]
        public decimal Link  { get; set; }
        public string PCAddress1 { get; set; } = string.Empty;
        public string PCAddress2 { get; set; } = string.Empty;
        public string PCCity { get; set; } = string.Empty;
        public string PCState { get; set; } = string.Empty;
        public string PCZipcode { get; set; } = string.Empty;
        public string PCTelephone1 { get; set; } = string.Empty;
        public string PCTelephone2 { get; set; } = string.Empty;
        public decimal BeginMonth { get; set; }
        public decimal BeginDay { get; set; }
        public decimal EndMonth { get; set; }
        public decimal EndDay { get; set; }
        public string SCAddress1 { get; set; } = string.Empty;
        public string SCAddress2 { get; set; } = string.Empty;
        public string SCCity { get; set; } = string.Empty;
        public string SCState { get; set; } = string.Empty;
        public string SCZipcode { get; set; } = string.Empty;
        public string SCTelephone1 { get; set; } = string.Empty;
        public string SCTelephone2 { get; set; } = string.Empty;
        public string PCEMailAddress1 { get; set; } = string.Empty;
        public string PCEMailAddress2 { get; set; } = string.Empty;
        public string SCEMailAddress1 { get; set; } = string.Empty;
        public string SCEMailAddress2 { get; set; } = string.Empty;
        public string PCForeignAddress { get; set; } = string.Empty;
        public string SCForeignAddress { get; set; } = string.Empty;
        public string PCTelephone3 { get; set; } = string.Empty;
        public string PCTelephone4 { get; set; } = string.Empty;
        public string Comments { get; set; } = string.Empty;
        public string PCTelephone5 { get; set; } = string.Empty;
    }
}
