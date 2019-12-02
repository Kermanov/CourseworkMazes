using System;
using System.Collections.Generic;
using System.Text;

namespace Mazes.Generation.MazeGenerators
{
    public class AldousBroderGenerator : MazeGeneratorBase
    {
        private Random rand;
        private uint visitedCounter;

        public AldousBroderGenerator()
        {
            rand = new Random();
        }

        public override Maze Generate(int width, int height)
        {
            maze = new Maze(width, height);
            visitedCounter = 0;
            var currentCell = new CellPosition(
                rand.Next(0, height), rand.Next(0, width)
            );
            visitedCounter++;

            while (visitedCounter < maze.Width * maze.Height)
            {
                var randomCell = GetNextRandomCellPos(currentCell);
                if (!IsVisited(randomCell))
                {
                    RemoveWall(randomCell, currentCell);
                    visitedCounter++;
                }
                currentCell = randomCell;
            }

            return maze;
        }

        private bool IsVisited(CellPosition cellPosition)
        {
            var cell = maze[cellPosition];
            return cell.TopSide == SideState.Open || 
                cell.RightSide == SideState.Open ||
                cell.BottomSide == SideState.Open || 
                cell.LeftSide == SideState.Open;
        }

        private CellPosition GetNextRandomCellPos(CellPosition cellPosition)
        {
            var nextCellPositions = GetNextCellPositions(cellPosition);
            var randomIndex = rand.Next(0, nextCellPositions.Count);
            return nextCellPositions[randomIndex];
        }
    }
}
