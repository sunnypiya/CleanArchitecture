using System.ComponentModel.DataAnnotations;

namespace PTG.NextStep.Domain.Models
{
    public class MemberDRO
    {
        [Key]
        public decimal Link { get; set; }
		public string DROSSN { get; set; } = string.Empty;
		public string DROSSNLastFour { get; set; }
		public string DROSalutation { get; set; } = string.Empty;
		public string DROFirstName { get; set; } = string.Empty;
		public string DROMiddleName { get; set; } = string.Empty;
		public string DROLastName { get; set; } = string.Empty;
		public string DROSuffix { get; set; } = string.Empty;
		public DateOnly? DROBirthDate { get; set; }
		public string Address1 { get; set; } = string.Empty;
		public string Address2 { get; set; } = string.Empty;
		public string City { get; set; } = string.Empty;
		public string State { get; set; } = string.Empty;
		public string Zipcode { get; set; } = string.Empty;
		public string EMailAddress { get; set; } = string.Empty;
		public string Telephone { get; set; } = string.Empty;
		public string Comments { get; set; } = string.Empty;
		public string DROMarriageDate { get; set; } = string.Empty;
		public string DRODivorceDate { get; set; } = string.Empty;
		[MaxLength(1)]
		public string DROCola { get; set; } = string.Empty;
		public string DROStartDate { get; set; } = string.Empty;
		public string DROEndDate { get; set; } = string.Empty;
		public decimal? DROAmountPct { get; set; }
		public decimal? DROAmountFlat { get; set; }


	}
}
