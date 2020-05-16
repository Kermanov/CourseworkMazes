using Mazes.Generation;
using Mazes.Solving.Helpers;
using Mazes.Solving.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mazes.Solving.MazeSolvers
{
    public abstract class MazeSolverBase : IMazeSolver
    {
        protected Maze maze;

        public abstract MazeSolution Solve(Maze maze, CellPosition startCellPosition, CellPosition escapeCellPosition);

        protected List<CellPosition> GetNextOpenCellPositions(CellPosition cellPosition)
        {
            var nextCellPositions = new List<CellPosition>();

            if (cellPosition.Row > 0 && maze[cellPosition].TopSide == SideState.Open)
            {
                nextCellPositions.Add(new CellPosition(cellPosition.Row - 1, cellPosition.Col));
            }
            if (cellPosition.Col < maze.Width - 1 && maze[cellPosition].RightSide == SideState.Open)
            {
                nextCellPositions.Add(new CellPosition(cellPosition.Row, cellPosition.Col + 1));
            }
            if (cellPosition.Row < maze.Height - 1 && maze[cellPosition].BottomSide == SideState.Open)
            {
                nextCellPositions.Add(new CellPosition(cellPosition.Row + 1, cellPosition.Col));
            }
            if (cellPosition.Col > 0 && maze[cellPosition].LeftSide == SideState.Open)
            {
                nextCellPositions.Add(new CellPosition(cellPosition.Row, cellPosition.Col - 1));
            }

            return nextCellPositions;
        }
    }
}
