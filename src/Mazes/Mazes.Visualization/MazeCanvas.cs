using Mazes.Generation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Mazes.Visualization
{
    public enum MazeStyle: byte
    {
        Default,
        Bold
    }

    public class MazeCanvas: Canvas
    {
        private double cellSize;
        private Pen cellPen;
        private double defaultPenThickness;
        private Maze maze;
        private MazeStyle mazeStyle;

        public Maze Maze
        {
            get => maze;
            set
            {
                maze = value;
                this.InvalidateVisual();
            }
        }

        public MazeStyle MazeStyle
        {
            get => mazeStyle;
            set
            {
                if (value != mazeStyle)
                {
                    mazeStyle = value;
                    this.InvalidateVisual();
                }
            }
        }

        public MazeCanvas(Maze maze, MazeStyle mazeStyle = MazeStyle.Default): base()
        {
            Maze = maze;
            defaultPenThickness = 2;
            cellPen = new Pen(Brushes.Black, defaultPenThickness);
            MazeStyle = mazeStyle;
        }

        protected override void OnRender(DrawingContext dc)
        {
            cellSize = this.ActualHeight / Maze.Height;

            if (MazeStyle == MazeStyle.Bold)
            {
                cellPen.Thickness = cellSize / 2;
            }
            else if (MazeStyle == MazeStyle.Default)
            {
                cellPen.Thickness = defaultPenThickness;
            }

            this.Margin = new Thickness(cellPen.Thickness / 2);

            for (int i = 0; i < Maze.Height; ++i)
            {
                for (int j = 0; j < Maze.Width; ++j)
                {
                    DrawCell(new CellPosition(i, j), dc);
                }
            }
        }

        private void DrawCell(CellPosition cellPos, DrawingContext dc)
        {
            var realPos = new Point(cellPos.Col * cellSize, cellPos.Row * cellSize);
            var halfThickness = cellPen.Thickness / 2;

            if (Maze[cellPos].TopSide == SideState.Closed)
            {
                var point1 = new Point(realPos.X - halfThickness, realPos.Y);
                var point2 = new Point(realPos.X + cellSize + halfThickness, realPos.Y);
                dc.DrawLine(cellPen, point1, point2);
            }
            if (Maze[cellPos].LeftSide == SideState.Closed)
            {
                var point1 = new Point(realPos.X, realPos.Y - halfThickness);
                var point2 = new Point(realPos.X, realPos.Y + cellSize + halfThickness);
                dc.DrawLine(cellPen, point1, point2);
            }

            if (cellPos.Col == Maze.Width - 1)
            {
                var point1 = new Point(realPos.X + cellSize, realPos.Y - halfThickness);
                var point2 = new Point(realPos.X + cellSize, realPos.Y + cellSize + halfThickness);
                dc.DrawLine(cellPen, point1, point2);
            }
            if (cellPos.Row == Maze.Height - 1)
            {
                var point1 = new Point(realPos.X - halfThickness, realPos.Y + cellSize);
                var point2 = new Point(realPos.X + cellSize + halfThickness, realPos.Y + cellSize);
                dc.DrawLine(cellPen, point1, point2);
            }
        }
    }
}
