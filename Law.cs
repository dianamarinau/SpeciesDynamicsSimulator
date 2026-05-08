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

        // IsBlock inlocuieste IsActive - true daca legea e de tip "block" (mancat)
        public bool IsBlock { get; set; }

        // Eater = specia care mananca (relevant doar cand IsBlock = true)
        // -1 daca nu e o lege block
        public int Eater { get; set; }

        public List<Condition> Conditions { get; set; }

        public Law(int start, int end, bool isBlock, int eater, List<Condition> conditions)
        {
            Start = start;
            End = end;
            IsBlock = isBlock;
            Eater = eater;
            Conditions = conditions;
        }
    }
}
