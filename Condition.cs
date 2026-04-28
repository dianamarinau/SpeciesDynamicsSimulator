using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeciesDynamicsSimulator
{
    public class Condition
    {
        public int Neighbour { get; set; }
        public int MinCount { get; set; }
        public int MaxCount { get; set; }
        public Condition(int neighbour, int minCount, int maxCount)
        {
            Neighbour = neighbour;
            MinCount = minCount;
            MaxCount = maxCount;
        }
    }
}
