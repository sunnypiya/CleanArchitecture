using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTG.NextStep.Service
{
    public class StringHelper
    {
        public string GetLast4Digits(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            return input.Length <= 4 ? input : input.Substring(input.Length - 4);
        }
    }
}
