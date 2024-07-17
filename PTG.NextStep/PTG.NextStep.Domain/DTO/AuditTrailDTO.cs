using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTG.NextStep.Domain.DTO
{
	public class AuditTrailDTO
	{
		public string ID { get; set; } = string.Empty; 
		public decimal? CrossLink { get; set; }
		public decimal? CrossAuditLink { get; set; }
		public string Screen { get; set; } = string.Empty; 
		public string Field { get; set; } = string.Empty; 
		public string Username { get; set; } = string.Empty; 
		public DateOnly? ChangeDate { get; set; } 
		public decimal? ChangeTime { get; set; }
		public string ProcessName { get; set; } = string.Empty; 
		public string OldValue { get; set; } = string.Empty; 
		public string NewValue { get; set; } = string.Empty; 
		public decimal? DescriptionKey { get; set; }
	}

}
