using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTG.NextStep.Domain.DTO
{
	public class DEImportResultsDTO
	{
		public decimal? AuditLink { get; set; }
		public string PostingNumber { get; set; } = string.Empty; 
		public decimal ResultsID { get; set; }
		public string DISSN { get; set; } = string.Empty; 
		public string Message { get; set; } = string.Empty; 
		public string DILastName { get; set; } = string.Empty; 
		public string DIFirstName { get; set; } = string.Empty; 
		public string InterfaceIDAndRowNumber { get; set; } = string.Empty; 
		public string SSNLastFour { get; set; } = string.Empty; 
		public DateOnly? PostingDate { get; set; }
		public string ErrorWarningFlag { get; set; } = string.Empty; 
	}

}
