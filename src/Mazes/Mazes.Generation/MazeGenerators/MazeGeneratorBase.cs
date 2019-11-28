using Mazes.Generation.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mazes.Generation.MazeGenerators
{
    abstract public class MazeGeneratorBase : IMazeGenerator
    {
        protected Maze maze;

        abstract public Maze Generate(int width, int height);

        protected List<CellPosition> GetNextCellPositions(CellPosition cellPosition)
        {
            var nextCellPositions = new List<CellPosition>();

            if (cellPosition.Row > 0)
            {
                nextCellPositions.Add(new CellPosition(cellPosition.Row - 1, cellPosition.Col));
            }
            if (cellPosition.Row < maze.Height - 1)
            {
                nextCellPositions.Add(new CellPosition(cellPosition.Row + 1, cellPosition.Col));
            }
            if (cellPosition.Col > 0)
            {
                nextCellPositions.Add(new CellPosition(cellPosition.Row, cellPosition.Col - 1));
            }
            if (cellPosition.Col < maze.Width - 1)
            {
                nextCellPositions.Add(new CellPosition(cellPosition.Row, cellPosition.Col + 1));
            }

            return nextCellPositions;
        }

        protected void RemoveWall(CellPosition cellPos1, CellPosition cellPos2)
        {
            if (cellPos1.Row < cellPos2.Row)
            {
                maze[cellPos1].BottomSide = SideState.Open;
                maze[cellPos2].TopSide = SideState.Open;
            }
            if (cellPos1.Row > cellPos2.Row)
            {
                maze[cellPos1].TopSide = SideState.Open;
                maze[cellPos2].BottomSide = SideState.Open;
            }
            if (cellPos1.Col < cellPos2.Col)
            {
                maze[cellPos1].RightSide = SideState.Open;
                maze[cellPos2].LeftSide = SideState.Open;
            }
            if (cellPos1.Col > cellPos2.Col)
            {
                maze[cellPos1].LeftSide = SideState.Open;
                maze[cellPos2].RightSide = SideState.Open;
            }
        }
    }
}
