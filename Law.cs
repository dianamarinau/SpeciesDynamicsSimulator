using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SpeciesDynamicsSimulator
{
    public class Law
    {
        public int Start { get; set; }
        public int End { get; set; }
        public bool IsActive { get; set; }
        public List<Condition> Conditions { get; set; }

        public Law(int start, int end, bool isActive, List<Condition> conditions)
        {
            Start = start;
            End = end;
            IsActive = isActive;
            Conditions = conditions;
        }
    }
}
