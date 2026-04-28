using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace SpeciesDynamicsSimulator
{
    public class GridRender
    {
        private readonly Dictionary<int, Brush> _speciesColors;
        private readonly int _cellSize;

        public GridRender(int cellSize)
        {
            _cellSize = cellSize;

            _speciesColors = new Dictionary<int, Brush>
            {
                { 0, Brushes.DarkGreen },
                { 1, Brushes.LightBlue },
                { 2, Brushes.Blue },
                { 3, Brushes.DarkBlue }
            };
        }

        public void Draw(Graphics g, GridState state)
        {
            for (int i = 0; i < state.Rows; i++)
            {
                for (int j = 0; j < state.Columns; j++)
                {
                    int speciesID = state.Species[i, j];
                    Brush brush = _speciesColors.ContainsKey(speciesID) ? _speciesColors[speciesID] : Brushes.Gray;
                    g.FillRectangle(brush, j * _cellSize, i * _cellSize, _cellSize, _cellSize);
                }
            }
        }
    }
}
