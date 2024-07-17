using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTG.NextStep.Domain.DTO
{
    public class EarningsDeductionsPostingNumberDTO
    {
        public string PostingNumber { get; set; } = string.Empty;
        public DateOnly DeductionPostingDate { get; set; }
    }
}
