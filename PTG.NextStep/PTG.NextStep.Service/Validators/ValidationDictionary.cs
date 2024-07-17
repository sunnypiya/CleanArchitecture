using PTG.NextStep.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTG.NextStep.Service.Validators
{
    public class ValidationDictionary : IValidationDictionary
    {
        private readonly Dictionary<string, string> _errors = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _warnings = new Dictionary<string, string>();

        public void AddError(string key, string errorMessage)
        {
            if (!_errors.ContainsKey(key))
            {
                _errors.Add(key, errorMessage);
            }
        }

        public void AddWarning(string key, string message)
        {
            if (!_warnings.ContainsKey(key))
            {
                _warnings.Add(key, message);
            }
        }

        public bool IsValid => !_errors.Any();

        public Dictionary<string, string> Errors => _errors;
        public Dictionary<string, string> Warnings => _warnings;
    }

}
