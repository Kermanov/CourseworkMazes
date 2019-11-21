using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Mazes.Generation
{
    public enum SideState : byte
    {
        Open,
        Closed
    }

    public class Cell
    {
        public SideState TopSide { get; set; }
        public SideState LeftSide { get; set; }

        public Cell()
        {
            TopSide = SideState.Closed;
            LeftSide = SideState.Closed;
        }
    }

    public class CellPosition
    {
        public int Row { get; }
        public int Col { get; }

        public CellPosition(int row, int col)
        {
            Row = row;
            Col = col;
        }
    }

    public class Maze
    {
        private Cell[,] cells;

        public int Width { get; }
        public int Height { get; }

        public Maze(int width, int height)
        {
            Width = width;
            Height = height;

            cells = new Cell[Height, Width];
            for (int i = 0; i < Height; ++i)
            {
                for (int j = 0; j < Width; ++j)
                {
                    cells[i, j] = new Cell();
                }
            }
        }

        public Cell this[CellPosition cellPosition]
        {
            get => cells[cellPosition.Row, cellPosition.Col];
            set => cells[cellPosition.Row, cellPosition.Col] = value;
        }

        public List<CellPosition> GetNextCellPositions(CellPosition cellPosition)
        {
            var nextCellPositions = new List<CellPosition>();

            if (cellPosition.Row > 0)
            {
                nextCellPositions.Add(new CellPosition(cellPosition.Row - 1, cellPosition.Col));
            }
            if (cellPosition.Row < Height - 1)
            {
                nextCellPositions.Add(new CellPosition(cellPosition.Row + 1, cellPosition.Col));
            }
            if (cellPosition.Col > 0)
            {
                nextCellPositions.Add(new CellPosition(cellPosition.Row, cellPosition.Col - 1));
            }
            if (cellPosition.Col < Width - 1)
            {
                nextCellPositions.Add(new CellPosition(cellPosition.Row, cellPosition.Col + 1));
            }

            return nextCellPositions;
        }
    }
}
