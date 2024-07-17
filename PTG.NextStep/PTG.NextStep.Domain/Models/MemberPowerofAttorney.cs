using System.ComponentModel.DataAnnotations;

namespace PTG.NextStep.Domain.Models
{
    public class MemberPowerofAttorney
    {
        [Key]
        public decimal Link { get; set; }
        public DateOnly EffectiveDate { get; set; }
        public string Revoked { get; set; } = string.Empty;
        public string PowerOfAttorney { get; set; } = string.Empty;
        public string POAAddress1 { get; set; } = string.Empty;
        public string POAAddress2 { get; set; } = string.Empty;
        public string POACity { get; set; } = string.Empty;
        public string POAState { get; set; } = string.Empty;
        public string POAZipcode { get; set; } = string.Empty;
        public string POATelephone { get; set; } = string.Empty;
        public string POAEMailAddress { get; set; } = string.Empty;
        public string Comments { get; set; } = string.Empty;
    }
}
