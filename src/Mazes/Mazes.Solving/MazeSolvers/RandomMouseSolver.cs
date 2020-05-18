using Mazes.Generation;
using Mazes.Solving.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mazes.Solving.MazeSolvers
{
    public class RandomMouseSolver : MazeSolverBase
    {
        private CellPosition currentPosition;
        private readonly Random random;
        private List<CellPosition> path;
        private readonly int movesLimit;

        public RandomMouseSolver(int movesLimit = int.MaxValue)
        {
            random = new Random();
            path = new List<CellPosition>();
            this.movesLimit = movesLimit;
        }

        public override MazeSolution Solve(Maze maze, CellPosition startCellPosition, CellPosition escapeCellPosition)
        {
            this.maze = maze;
            this.currentPosition = startCellPosition;

            path.Add(currentPosition);
            while (path.Count < movesLimit && currentPosition != escapeCellPosition)
            {
                Move();
            }

            return new MazeSolution
            {
                FullPath = path,
                Solution = new List<CellPosition>()
            };
        }

        private void Move()
        {
            var nextCells = GetNextOpenCellPositions(currentPosition);

            if (nextCells.Count == 1)
            {
                currentPosition = nextCells[0];
            }
            else
            {
                if (path.Count > 1)
                {
                    nextCells.Remove(path[path.Count - 2]);
                }

                var randomNextCell = nextCells[random.Next(0, nextCells.Count)];
                currentPosition = randomNextCell;
            }

            path.Add(currentPosition);
        }
    }
}
