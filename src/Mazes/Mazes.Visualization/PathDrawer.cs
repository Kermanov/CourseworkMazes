using Mazes.Generation;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mazes.Visualization
{
    public class PathDrawer : Drawable
    {
        private readonly Maze maze;
        private readonly List<CellPosition> path;
        private readonly Color color;
        private readonly float cellWidth;
        private readonly float cellHeight;
        private readonly float lineThickness;
        private readonly Vector2f position;
        private int currentPathIndex;

        public PathDrawer(
            Maze maze,
            List<CellPosition> path,
            Color color,
            float cellWidth,
            float cellHeight,
            float lineThickness,
            Vector2f position)
        {
            this.maze = maze;
            this.path = path;
            this.color = color;
            this.cellWidth = cellWidth;
            this.cellHeight = cellHeight;
            this.lineThickness = lineThickness;
            this.position = position;
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            for (int i = 0; i < currentPathIndex; ++i)
            {
                DrawPathCell(path[i], target);
            }

            if (currentPathIndex < path.Count)
            {
                currentPathIndex++;
            }
        }

        private void DrawPathCell(CellPosition cell, RenderTarget target)
        {
            var width = cellWidth;
            if (maze[cell].RightSide == SideState.Closed)
            {
                width -= lineThickness;
            }

            var height = cellHeight;
            if (maze[cell].BottomSide == SideState.Closed)
            {
                height -= lineThickness;
            }

            var rect = new RectangleShape
            {
                Position = new Vector2f(cell.Col * cellWidth + position.X + lineThickness, cell.Row * cellHeight + position.Y + lineThickness),
                Size = new Vector2f(width, height),
                FillColor = color
            };
            target.Draw(rect);
        }
    }
}
