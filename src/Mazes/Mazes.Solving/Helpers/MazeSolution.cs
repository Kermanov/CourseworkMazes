using Mazes.Generation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mazes.Solving.Helpers
{
    public class MazeSolution
    {
        public List<CellPosition> FullPath { get; set; }
        public List<CellPosition> Solution { get; set; }
    }
}
