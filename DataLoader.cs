using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SpeciesDynamicsSimulator
{
    public static class DataLoader
    {
        public static GridState LoadGrid(string fileName)
        {
            var lines = File.ReadAllLines(fileName);
            int maxspecies = 0;

            foreach (var line in lines)
            {
                string[] elements = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var element in elements)
                {
                    maxspecies = Math.Max(maxspecies, int.Parse(element));
                }
            }

            var state = new GridState(maxspecies);

            for (int i = 0; i < state.Rows && i < lines.Length; i++)
            {
                string[] elements = lines[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                for (int j = 0; j < state.Columns && j < elements.Length; j++)
                {
                    int currentSpecies = int.Parse(elements[j]);
                    state.Species[i, j] = currentSpecies;

                    state.InitialPopulation[currentSpecies]++;
                    state.Population[currentSpecies]++;
                }
            }

            return state;
        }


        public static List<Law> LoadLaws(string fileName)
        {
            var laws = new List<Law>();
            var lines = File.ReadAllLines(fileName);
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                string[] elements = line.Split(new char[] { '{', '}', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                int start = int.Parse(elements[0].Trim());
                int end = int.Parse(elements[2].Trim());
                bool isActive = (elements.Length > 3 && elements[3].Trim() == "block");

                var conditionsList = new List<Condition>();
                string[] middle = elements[1].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string element in middle)
                {
                    string[] whole = element.Split(new char[] { '[', ']', ',' }, StringSplitOptions.RemoveEmptyEntries);
                    int neighbour = int.Parse(whole[0].Trim());
                    int minCount = int.Parse(whole[1].Trim());
                    int maxCount = int.Parse(whole[2].Trim());

                    conditionsList.Add(new Condition(neighbour, minCount, maxCount));
                }

                laws.Add(new Law(start, end, isActive, conditionsList));
            }
            return laws;
        }
    }
}
