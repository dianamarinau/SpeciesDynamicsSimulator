using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeciesDynamicsSimulator
{
    public class Condition
    {
        public int neighbour;
        public int min_count;
        public int max_count;
       
        public Condition(int neighbour, int min_count, int max_count)
        {
            this.neighbour = neighbour;
            this.min_count = min_count;
            this.max_count = max_count;
           
        }

        public Condition(string beginning)
        {
            string[] whole = beginning.Split(new char[] { '[', ']', ','}, StringSplitOptions.RemoveEmptyEntries);
            this.neighbour = int.Parse(whole[0].Trim());
            this.min_count = int.Parse(whole[1].Trim());
            this.max_count = int.Parse(whole[2].Trim());
        }
        public Condition()
        {
        }
    }
}
