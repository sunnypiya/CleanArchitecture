using Microsoft.EntityFrameworkCore;

namespace PTG.NextStep.Domain.Models
{
    [PrimaryKey(nameof(ShortName), nameof(Unit))]
    public class LuPostingCodes
    {
        public decimal AuditLink { get; set; }
        public string ShortName { get; set; } = string.Empty;
        public string LongName { get; set; } = string.Empty;
        public string PostTo { get; set; } = string.Empty;
        public string PostAction { get; set; } = string.Empty;
        public string Taxable { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public string PensionablePay { get; set; } = string.Empty;
    }
}
