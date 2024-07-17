using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTG.NextStep.Domain.DTO
{
    public class CreateRegisterRecordDTO
    {
        public string PostingNumberToPullFrom { get; set; } = string.Empty;
        public string PostingNumber { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public bool AllActivities { get; set; }        
        public DateOnly DeductionPostingDate { get; set; }
    }
}
