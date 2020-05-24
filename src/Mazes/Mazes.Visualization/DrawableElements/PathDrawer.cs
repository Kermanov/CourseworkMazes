using Mazes.Generation;
using SFML.Graphics;
using SFML.System;
using System.Collections.Generic;
using System.Timers;

namespace Mazes.Visualization.DrawableElements
{
    public class PathDrawer : Drawable
    {
        private readonly Maze maze;
        private readonly List<CellPosition> path;
        private readonly Color color;
        private readonly float cellWidth;
        private readonly float cellHeight;
        private readonly Vector2f position;
        private int currentPathIndex;
        private Timer timer;

        public PathDrawer(
            Maze maze,
            List<CellPosition> path,
            Color color,
            float cellWidth,
            float cellHeight,
            Vector2f position)
        {
            this.maze = maze;
            this.path = path;
            this.color = color;
            this.cellWidth = cellWidth;
            this.cellHeight = cellHeight;
            this.position = position;

            timer = new Timer(50);
            timer.Elapsed += (s, e) =>
            {
                if (currentPathIndex < path.Count)
                {
                    currentPathIndex++;
                }
                else
                {
                    timer.Stop();
                }
            };
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        public int Delay
        {
            set => timer.Interval = value;
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            for (int i = 0; i < currentPathIndex; ++i)
            {
                if (i < currentPathIndex - 1)
                {
                    DrawPathCell(path[i], color, target);
                }
                else
                {
                    DrawPathCell(path[i], new Color(255, 250, 107), target);
                }
            }
        }

        private void DrawPathCell(CellPosition cell, Color color, RenderTarget target)
        {
            var rect = new RectangleShape
            {
                Position = new Vector2f(cell.Col * cellWidth + position.X, cell.Row * cellHeight + position.Y),
                Size = new Vector2f(cellWidth, cellHeight),
                FillColor = color
            };
            target.Draw(rect);
        }
    }
}
