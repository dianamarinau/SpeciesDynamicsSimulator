using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace SpeciesDynamicsSimulator
{
    public partial class Form1 : Form
    {
        private Bitmap _bitmap;
        private Graphics _graphics;

        // Noile noastre piese de arhitectură
        private GridState _currentState;
        private GridState _initialState;
        private SimulationEngine _engine;
        private GridRender _render;

        private Series[] _speciesSeries;
        private Color[] _colors = { Color.DarkGreen, Color.LightBlue, Color.Blue, Color.DarkBlue };

        public Form1()
        {
            InitializeComponent();
            this.BackColor = Color.FromArgb(0, 6, 48);

            _bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            _graphics = Graphics.FromImage(_bitmap);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var laws = DataLoader.LoadLaws(@"..\..\PreyPredatorPredatorCompetitionLaws.txt");
            _currentState = DataLoader.LoadGrid(@"..\..\3SpeciesMatrix.txt");
            _initialState = _currentState.Clone();

            _engine = new SimulationEngine(laws);
            int cellSize = pictureBox1.Width / _currentState.Columns;
            _render = new GridRender(cellSize);

            int numberOfSpecies = _currentState.Population.Length;
            _speciesSeries = new Series[numberOfSpecies];
            chart1.Series.Clear();

            for (int i = 1; i < numberOfSpecies; i++)
            {
                _speciesSeries[i] = new Series($"Specia {i}");
                _speciesSeries[i].ChartType = SeriesChartType.Line;
                _speciesSeries[i].BorderWidth = 3;
                _speciesSeries[i].Color = _colors[i];
                chart1.Series.Add(_speciesSeries[i]);
            }

            UpdateUI(0);
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            btn_reset.Enabled = false;

            int generations = (int)numericUpDown1.Value;

            for (int i = 1; i <= generations; i++)
            {
                textBox1.Text = $"Generația {i}";
                await Task.Delay(500);

                _engine.ComputeNextGeneration(_currentState);

                UpdateUI(i);
            }

            button1.Enabled = true;
            btn_reset.Enabled = true;
        }

        private void btn_reset_Click(object sender, EventArgs e)
        {
            _currentState = _initialState.Clone();
            for (int i = 1; i < _speciesSeries.Length; i++)
            {
                _speciesSeries[i].Points.Clear();
            }

            textBox1.Text = "Generația 0";
            UpdateUI(0);
        }

        private void UpdateUI(int generationIndex)
        {
            _render.Draw(_graphics, _currentState);
            pictureBox1.Image = _bitmap;
            pictureBox1.Refresh();

            for (int j = 1; j < _speciesSeries.Length; j++)
            {
                _speciesSeries[j].Points.AddXY(generationIndex, _currentState.Population[j]);
            }
        }
    }
}