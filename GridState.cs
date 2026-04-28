using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeciesDynamicsSimulator
{
    public class GridState
    {
        public int Rows { get; } = 50;
        public int Columns { get; } = 50;

        public int[,] Species { get; set; }
        public int[] Population { get; set; }
        public int[] InitialPopulation { get; set; }
        public GridState(int maxSpecies)
        {
            Species = new int[Rows, Columns];
            Population = new int[maxSpecies + 1];
            InitialPopulation = new int[maxSpecies + 1];
        }

        public GridState Clone()
        {
            var clone = new GridState(Population.Length - 1);
            Array.Copy(Species, clone.Species, Species.Length);
            return clone;
        }
    }
}
