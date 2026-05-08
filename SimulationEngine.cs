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

            // hasEaten[r,c] = true daca celula (r,c) a fost deja mancata in aceasta generatie
            bool[,] hasEaten = new bool[rows, columns];

            // fedPredator[r,c] = true daca pradatorul de la (r,c) a mancat deja in aceasta generatie
            bool[,] fedPredator = new bool[rows, columns];

            GridState clone = state.Clone();
            GridState result = state.Clone();

            Array.Clear(result.Species, 0, result.Species.Length);


            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    int currentSpecies = clone.Species[i, j];
                    int[] neighbours = new int[state.Population.Length];

                    // Numara vecinii (8-directional)
                    if (i > 0) neighbours[clone.Species[i - 1, j]]++;
                    if (i < rows - 1) neighbours[clone.Species[i + 1, j]]++;
                    if (j > 0) neighbours[clone.Species[i, j - 1]]++;
                    if (j < columns - 1) neighbours[clone.Species[i, j + 1]]++;
                    if (i > 0 && j > 0) neighbours[clone.Species[i - 1, j - 1]]++;
                    if (i > 0 && j < columns - 1) neighbours[clone.Species[i - 1, j + 1]]++;
                    if (i < rows - 1 && j > 0) neighbours[clone.Species[i + 1, j - 1]]++;
                    if (i < rows - 1 && j < columns - 1) neighbours[clone.Species[i + 1, j + 1]]++;

                    Law matchedLaw = null;
                    for (int k = 0; k < _laws.Count; k++)
                    {
                        var law = _laws[k];
                        if (currentSpecies != law.Start) continue;

                        bool verified = true;
                        foreach (var condition in law.Conditions)
                        {
                            if (neighbours[condition.Neighbour] < condition.MinCount ||
                                neighbours[condition.Neighbour] > condition.MaxCount)
                            {
                                verified = false;
                                break;
                            }
                        }

                        if (verified)
                            matchedLaw = law;
                    }

                    if (matchedLaw == null) continue;

                    if (!matchedLaw.IsBlock)
                    {
                        result.Species[i, j] = matchedLaw.End;
                    }
                    else
                    {
                        int eaterSpecies = matchedLaw.Eater;
                        bool eaten = false;

                        int[] dr = { -1, -1, -1, 0, 0, 1, 1, 1 };
                        int[] dc = { -1, 0, 1, -1, 1, -1, 0, 1 };

                        for (int d = 0; d < 8; d++)
                        {
                            int ni = i + dr[d];
                            int nj = j + dc[d];

                            if (ni < 0 || ni >= rows || nj < 0 || nj >= columns) continue;

                            // Vecinul trebuie sa fie pradatorul corect si sa nu fi mancat deja
                            if (clone.Species[ni, nj] == eaterSpecies && !fedPredator[ni, nj])
                            {
                                if (!hasEaten[i, j])
                                {
                                    hasEaten[i, j] = true;
                                    fedPredator[ni, nj] = true;
                                    result.Species[i, j] = matchedLaw.End; // prada moare
                                    eaten = true;
                                    break;
                                }
                            }
                        }
                        // Daca niciun pradator eligibil nu exista, celula ramane neschimbata
                    }
                }
            }
            Array.Clear(state.Population, 0, state.Population.Length);
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < columns; j++)
                {
                    state.Species[i, j] = result.Species[i, j];
                    state.Population[state.Species[i, j]]++;
                }
        }

        /*private void CheckEating(GridState state, bool[,] hasEaten, ref int timesEaten, int i, int j, int targetSpecies)
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
        }*/
    }
}
