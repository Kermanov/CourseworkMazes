using Mazes.Generation;
using Mazes.Solving.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mazes.Solving.Interfaces
{
    public interface IMazeSolver
    {
        MazeSolution Solve(Maze maze, CellPosition startCellPosition, CellPosition escapeCellPosition);
    }
}
