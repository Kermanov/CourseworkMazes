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
        public SideState RightSide { get; set; }
        public SideState BottomSide { get; set; }
        public SideState LeftSide { get; set; }

        public Cell()
        {
            TopSide = SideState.Closed;
            RightSide = SideState.Closed;
            BottomSide = SideState.Closed;
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
    }
}
