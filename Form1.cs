using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace SpeciesDynamicsSimulator
{
    public partial class Form1: Form
    {
        Bitmap bitmap;
        Graphics graphics;
        Civilisation civilisation;
        Civilisation initial;
        Series[] species;
        Color[] colors = { Color.DeepSkyBlue, Color.Aquamarine, Color.Turquoise };
        public static int numberofspecies;
        public static List<Law> laws = new List<Law>();
        public Form1()
        {
            InitializeComponent();
            this.BackColor = Color.FromArgb(0,6, 48);
            bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(bitmap);
            civilisation = new Civilisation(@"..\..\Species.txt");
            Civilisation.size = pictureBox1.Width / Civilisation.columns;
            Array.Clear(Civilisation.initial_population, 0, Civilisation.initial_population.Length);
            initial = new Civilisation(@"..\..\Species.txt");
        }

        private void Form1_Load(object sender, EventArgs e)
        {   
            StreamReader load = new StreamReader(@"..\..\NewLaws.txt");
            string buffer;
            while ((buffer = load.ReadLine()) != null)
            {
                Law law = new Law(buffer);
                laws.Add(law);
            }

            load.Close();

            numberofspecies = Civilisation.population.Length;
            species = new Series[numberofspecies];
            chart1.Series.Clear();
            for (int i = 0; i < numberofspecies; i++)
            {
                species[i] = new Series($"Specia {i}");
                species[i].ChartType = SeriesChartType.Line;
                species[i].BorderWidth = 3;
                species[i].Color = colors[i];
                chart1.Series.Add(species[i]);
            }
            civilisation.Draw(graphics);
            pictureBox1.Image = bitmap;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false; 
            btn_reset.Enabled = false;

            int generations = (int)numericUpDown1.Value;
            for(int i = 0; i < numberofspecies; i++)
                species[i].Points.AddXY(0, Civilisation.population[i]);

            for (int i = 1; i <= generations; i++)
            {
                textBox1.Text = $"Generația {i}" ;
                await Task.Delay(500); // Wait for 0.5 seconds before each generation
                civilisation.Transformation();
                for(int j = 0; j < numberofspecies; j++)
                    species[j].Points.AddXY(i, Civilisation.population[j]);
                civilisation.Draw(graphics);
                pictureBox1.Image = bitmap;
            }
                button1.Enabled = true; 
                btn_reset.Enabled = true;
        }

        private void btn_reset_Click(object sender, EventArgs e)
        {
            chart1.Series.Clear();
            for (int i = 0; i < numberofspecies; i++)
            {
                species[i] = new Series($"Specia {i}");
                species[i].ChartType = SeriesChartType.Line;
                species[i].BorderWidth = 3;
                species[i].Color = colors[i];
                chart1.Series.Add(species[i]);
            }
            for (int i = 0; i < Civilisation.rows; i++)
                for (int j = 0; j < Civilisation.columns; j++)
                    civilisation.species[i, j] = initial.species[i, j];

            for(int i = 0; i < numberofspecies; i++)
                Civilisation.population[i] = Civilisation.initial_population[i];

            civilisation.Draw(graphics);
            pictureBox1.Image = bitmap;
        }
    }
}
