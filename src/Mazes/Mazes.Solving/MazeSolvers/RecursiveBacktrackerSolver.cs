using Mazes.Generation;
using Mazes.Solving.Helpers;
using Mazes.Solving.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mazes.Solving.MazeSolvers
{
    public class RecursiveBacktrackerSolver : MazeSolverBase
    {
        private CellPosition startCellPosition;
        private CellPosition escapeCellPosition;
        private List<CellPosition> path;
        private HashSet<CellPosition> visitedCells;
        private List<CellPosition> solution;

        public RecursiveBacktrackerSolver()
        {
            path = new List<CellPosition>();
            visitedCells = new HashSet<CellPosition>();
            solution = new List<CellPosition>();
        }

        public override MazeSolution Solve(Maze maze, CellPosition startCellPosition, CellPosition escapeCellPosition)
        {
            this.maze = maze;
            this.startCellPosition = startCellPosition;
            this.escapeCellPosition = escapeCellPosition;

            RecursiveSolve(startCellPosition);

            solution.Reverse();

            return new MazeSolution
            {
                FullPath = path,
                Solution = solution
            };
        }

        private bool RecursiveSolve(CellPosition cell)
        {
            path.Add(cell);

            if (cell == escapeCellPosition)
            {
                solution.Add(cell);
                return true;
            }
            if (IsDeadEnd(cell))
            {
                return false;
            }

            visitedCells.Add(cell);

            var nextCells = GetNextOpenCellPositions(cell);
            foreach (var nextCell in nextCells)
            {
                if (!visitedCells.Contains(nextCell))
                {
                    if (RecursiveSolve(nextCell))
                    {
                        solution.Add(cell);
                        return true;
                    }
                }
            }

            return false;
        }

        private bool IsDeadEnd(CellPosition cell)
        {
            if (cell == startCellPosition && !visitedCells.Contains(cell))
            {
                return false;
            }

            var nClosedSides = 0;
            for (int i = 0; i < 4; ++i)
            {
                if (maze[cell].GetSideByNumber(i) == SideState.Closed)
                {
                    nClosedSides++;
                }
            }

            return nClosedSides >= 3;
        }
    }
}
