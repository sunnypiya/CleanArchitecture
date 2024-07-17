using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTG.NextStep.Domain
{
    public enum TaskStatus
    {
        Pending,
        InProgress,
        Completed,
        Failed,
        Canceled
    }
}
