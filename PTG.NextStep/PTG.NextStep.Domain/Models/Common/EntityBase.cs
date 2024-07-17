using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTG.NextStep.Domain.Models
{
    public abstract class EntityBase
    {
        [Key]
        public double Link { get; set; }

    }
}
