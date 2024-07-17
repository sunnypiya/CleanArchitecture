using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTG.NextStep.Domain
{
    public interface IValidationDictionary
    {
        void AddError(string key, string errorMessage);
        void AddWarning(string key, string message);
        bool IsValid { get; }
        Dictionary<string, string> Errors { get; }
        Dictionary<string, string> Warnings { get; }
    }
}
