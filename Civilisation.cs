using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace SpeciesDynamicsSimulator
{
    public class Civilisation
    {
        public static int rows = 50;
        public static int columns = 50;
        public static int size;
        static Random rnd = new Random();
        public int[,] species = new int[rows, columns];
        public static int[] population;
        public static int[] initial_population;
        List<string> lines = new List<string>();
        public Civilisation(string FileName)
        {
            string buffer;
            TextReader load = new StreamReader(FileName);
            while ((buffer = load.ReadLine()) != null)
                lines.Add(buffer);
            load.Close();

            int maxSpecies = 0;
            foreach(string line in lines)
            {
                string[] elements = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var el in elements)
                    maxSpecies = Math.Max(maxSpecies, int.Parse(el));
            }

            population = new int[maxSpecies + 1];
            initial_population = new int[maxSpecies + 1];

            for (int i = 0; i < lines.Count; i++)
            {
                string[] elements = lines[i].Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);
                for (int j = 0; j < columns; j++)
                {
                    species[i,j] = int.Parse(elements[j]);
                    initial_population[species[i, j]]++;
                }
            }
        }
        public Civilisation()
        {
        }

        public void Draw(Graphics graphics)
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (species[i, j] == 0)
                    {
                        graphics.FillRectangle(Brushes.DeepSkyBlue, j * size, i * size, size, size);
                    }
                    else if (species[i,j] == 1)
                    {
                        graphics.FillRectangle(Brushes.Aquamarine, j * size, i * size, size, size);
                    }
                    else
                    {
                        graphics.FillRectangle(Brushes.DarkTurquoise, j * size, i * size, size, size);
                    }
                }
            }
        }
        public int[,] Clone()
        {
            int[,] clone = new int[rows, columns];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    clone[i, j] = species[i, j];
                }
            }
            return clone;
        }
        public void Transformation()
        {
            int[,] clone = Clone();
            for(int i = 0; i < population.Length; i++)
            {
                population[i] = 0;
            }
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    int[] neighbours = new int[population.Length];

                    if (i > 0) neighbours[species[i - 1, j]]++; // sus
                    if (i < rows - 1) neighbours[species[i + 1, j]]++; // jos
                    if (j > 0) neighbours[species[i, j - 1]]++; // stanga
                    if (j < columns - 1) neighbours[species[i, j + 1]]++; // dreapta

                    for (int k = 0; k < Form1.laws.Count; k++)
                   {
                        if (species[i, j] == Form1.laws[k].start)//daca tipul celulei curente e cel din lege
                        {
                            bool verified = true;
                            foreach(var condition in Form1.laws[k].conditions)//verificam daca sunt indeplinite conditiile
                            {
                                if (neighbours[condition.neighbour] < condition.min_count 
                                    || neighbours[condition.neighbour] > condition.max_count)
                                {
                                    verified = false;
                                    break;
                                }
                            }
                            if(verified)
                            {
                                clone[i,j] = Form1.laws[k].end;
                                break;
                            }

                        }
                    }
                }
            }
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    species[i, j] = clone[i, j];
                    population[species[i, j]]++;
                }
            }

        }
    }
}