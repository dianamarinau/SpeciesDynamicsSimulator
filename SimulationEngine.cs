using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeciesDynamicsSimulator
{
    public class SimulationEngine
    {
        private readonly List<Law> _laws;

        public SimulationEngine(List<Law> laws)
        {
            _laws = laws;
        }

        public void ComputeNextGeneration(GridState state)
        {
            int rows = state.Rows;
            int columns = state.Columns;

            bool[,] hasEaten = new bool[rows, columns];
            GridState clone = state.Clone();

            Array.Clear(state.Population, 0, state.Population.Length);

            for(int i = 0; i < rows; i++)
            {
                for(int j = 0; j < columns; j++)
                {
                    int currentSpecies = clone.Species[i, j];
                    int[] neighbours = new int[state.Population.Length];

                    if (i > 0) neighbours[state.Species[i - 1, j]]++; // sus
                    if (i < rows - 1) neighbours[state.Species[i + 1, j]]++; // jos
                    if (j > 0) neighbours[state.Species[i, j - 1]]++; // stânga
                    if (j < columns - 1) neighbours[state.Species[i, j + 1]]++; // dreapta
                    if (i > 0 && j > 0) neighbours[state.Species[i - 1, j - 1]]++; // sus-stânga
                    if (i > 0 && j < columns - 1) neighbours[state.Species[i - 1, j + 1]]++; // sus-dreapta
                    if (i < rows - 1 && j > 0) neighbours[state.Species[i + 1, j - 1]]++; // jos-stânga
                    if (i < rows - 1 && j < columns - 1) neighbours[state.Species[i + 1, j + 1]]++; // jos-dreapta

                    for(int k = 0; k < _laws.Count; k++)
                    {
                        var law = _laws[k];
                        if (currentSpecies == law.Start)
                        {
                            bool verified = true;
                            foreach (var  condition in law.Conditions)
                            {
                                if (neighbours[condition.Neighbour] < condition.MinCount || neighbours[condition.Neighbour] > condition.MaxCount)
                                {
                                    verified = false;
                                    break;
                                }
                            }
                            if(verified)
                            {
                                if(law.IsActive)
                                {
                                    int timesEaten = 0;
                                    if (currentSpecies == 2)
                                    {
                                        CheckEating(state, hasEaten, ref timesEaten, i, j, targetSpecies: 3);
                                    }
                                    else if (currentSpecies == 1)
                                    {
                                        CheckEating(state, hasEaten, ref timesEaten, i, j, targetSpecies: 2);
                                    }

                                    if (timesEaten == 1)
                                        clone.Species[i, j] = law.End;
                                }
                                else
                                    clone.Species[i, j] = law.End;
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    state.Species[i, j] = clone.Species[i, j];
                    state.Population[state.Species[i, j]]++;
                }
            }
        }

        private void CheckEating(GridState state, bool[,] hasEaten, ref int timesEaten, int i, int j, int targetSpecies)
        {
            int rows = state.Rows;
            int columns = state.Columns;

            if (i > 0 && state.Species[i - 1, j] == targetSpecies && !hasEaten[i - 1, j] && timesEaten < 1) 
            { 
                hasEaten[i - 1, j] = true; 
                timesEaten++; 
            }
            if (i < rows - 1 && state.Species[i + 1, j] == targetSpecies && !hasEaten[i + 1, j] && timesEaten < 1) 
            { 
                hasEaten[i + 1, j] = true; 
                timesEaten++; 
            }
            if (j > 0 && state.Species[i, j - 1] == targetSpecies && !hasEaten[i, j - 1] && timesEaten < 1) 
            { 
                hasEaten[i, j - 1] = true;
                timesEaten++; 
            }
            if (j < columns - 1 && state.Species[i, j + 1] == targetSpecies && !hasEaten[i, j + 1] && timesEaten < 1) 
            { 
                hasEaten[i, j + 1] = true; 
                timesEaten++; 
            }
            if (i > 0 && j > 0 && state.Species[i - 1, j - 1] == targetSpecies && !hasEaten[i - 1, j - 1] && timesEaten < 1) 
            { 
                hasEaten[i - 1, j - 1] = true; 
                timesEaten++; 
            }
            if (i > 0 && j < columns - 1 && state.Species[i - 1, j + 1] == targetSpecies && !hasEaten[i - 1, j + 1] && timesEaten < 1) 
            {
                hasEaten[i - 1, j + 1] = true; 
                timesEaten++; 
            }
            if (i < rows - 1 && j > 0 && state.Species[i + 1, j - 1] == targetSpecies && !hasEaten[i + 1, j - 1] && timesEaten < 1) 
            { 
                hasEaten[i + 1, j - 1] = true; 
                timesEaten++; 
            }
            if (i < rows - 1 && j < columns - 1 && state.Species[i + 1, j + 1] == targetSpecies && !hasEaten[i + 1, j + 1] && timesEaten < 1) 
            { 
                hasEaten[i + 1, j + 1] = true; 
                timesEaten++; 
            }
        }
    }
}
