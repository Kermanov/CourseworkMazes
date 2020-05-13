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
        public SideState TopSide 
        {
            get => sides[0];
            set => sides[0] = value; 
        }
        public SideState RightSide 
        {
            get => sides[1];
            set => sides[1] = value;
        }
        public SideState BottomSide 
        {
            get => sides[2];
            set => sides[2] = value;
        }
        public SideState LeftSide 
        {
            get => sides[3];
            set => sides[3] = value;
        }

        private readonly SideState[] sides;

        public Cell()
        {
            sides = new SideState[]
            {
                SideState.Closed,
                SideState.Closed,
                SideState.Closed,
                SideState.Closed
            };
        }

        public SideState GetSideByNumber(int number)
        {
            return sides[(number + 4) % 4];
        }
    }

    public struct CellPosition
    {
        public int Row { get; }
        public int Col { get; }

        public CellPosition(int row, int col)
        {
            Row = row;
            Col = col;
        }

        public static bool operator ==(CellPosition left, CellPosition right)
        {
            return left.Row == right.Row && left.Col == right.Col;
        }

        public static bool operator !=(CellPosition left, CellPosition right)
        {
            return !(left == right);
        }

        public override string ToString()
        {
            return $"({Row}, {Col})";
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
