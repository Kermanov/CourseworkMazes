using Mazes.Generation;
using Mazes.Solving.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Mazes.Solving.Helpers;

namespace Mazes.Solving.MazeSolvers
{
    public enum TurningDirection : byte
    {
        Left,
        Right
    }

    public class WallFollowerSolver : MazeSolverBase
    {
        private readonly TurningDirection turningDirection;
        private CellPosition currentPosition;
        private int deltaCol;
        private int deltaRow;
        private int directionNumber;
        private List<CellPosition> visited;

        public WallFollowerSolver(TurningDirection turningDirection)
        {
            this.turningDirection = turningDirection;
            visited = new List<CellPosition>();
        }

        public override MazeSolution Solve(Maze maze, CellPosition startCellPosition, CellPosition escapeCellPosition)
        {
            this.maze = maze;
            currentPosition = startCellPosition;
            deltaCol = 0;
            deltaRow = 1;
            directionNumber = 2;

            while (currentPosition != escapeCellPosition)
            {
                Move();
            }
            visited.Add(new CellPosition(maze.Height - 1, maze.Width - 1));

            return new MazeSolution
            {
                FullPath = visited,
                Solution = FinalPath()
            };
        }

        private void Move()
        {
            visited.Add(currentPosition);

            if (CheckTurningDirection())
            {
                if (turningDirection == TurningDirection.Right)
                {
                    TurnRight();
                }
                else
                {
                    TurnLeft();
                }
            }
            else if (CheckAhead())
            {

            }
            else if (CheckAnotherDirection())
            {
                if (turningDirection == TurningDirection.Right)
                {
                    TurnLeft();
                }
                else
                {
                    TurnRight();
                }
            }
            else if (CheckBack())
            {
                TurnBack();
            }

            currentPosition = new CellPosition(currentPosition.Row + deltaRow, currentPosition.Col + deltaCol);
        }

        private bool CheckTurningDirection()
        {
            if (turningDirection == TurningDirection.Left)
            {
                return maze[currentPosition].GetSideByNumber((directionNumber - 1) % 4) == SideState.Open;
            }
            else
            {
                return maze[currentPosition].GetSideByNumber((directionNumber + 1) % 4) == SideState.Open;
            }
        }

        private bool CheckAhead()
        {
            return maze[currentPosition].GetSideByNumber(directionNumber % 4) == SideState.Open;
        }

        private bool CheckAnotherDirection()
        {
            if (turningDirection == TurningDirection.Left)
            {
                return maze[currentPosition].GetSideByNumber((directionNumber + 1) % 4) == SideState.Open;
            }
            else
            {
                return maze[currentPosition].GetSideByNumber((directionNumber - 1) % 4) == SideState.Open;
            }
        }

        private bool CheckBack()
        {
            return maze[currentPosition].GetSideByNumber((directionNumber + 2) % 4) == SideState.Open;
        }

        private void TurnRight()
        {
            int newDeltaX = -deltaRow;
            int newDeltaY = deltaCol;

            deltaCol = newDeltaX;
            deltaRow = newDeltaY;

            directionNumber = (directionNumber + 1) % 4;
        }

        private void TurnLeft()
        {
            int newDeltaX = deltaRow;
            int newDeltaY = -deltaCol;

            deltaCol = newDeltaX;
            deltaRow = newDeltaY;

            directionNumber = (directionNumber - 1) % 4;
        }

        private void TurnBack()
        {
            deltaCol = -deltaCol;
            deltaRow = -deltaRow;

            directionNumber = (directionNumber + 2) % 4;
        }

        private List<CellPosition> FinalPath()
        {
            var finalPath = new List<CellPosition>();
            for (int i = 0; i < visited.Count; ++i)
            {
                var cell = visited[i];
                finalPath.Add(cell);
                if (visited.FindAll(c => c == cell).Count > 1)
                {
                    var nextCells = GetNextOpenCellPositions(cell);
                    nextCells.Remove(finalPath.Last());
                    var validCellIndex = nextCells.Max(c => visited.IndexOf(c));
                    i = validCellIndex - 1;
                }
            }

            return finalPath;
        }
    }
}
