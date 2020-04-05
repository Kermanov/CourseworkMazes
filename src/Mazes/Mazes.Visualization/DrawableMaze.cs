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
    class DrawableMaze : Drawable
    {
        private readonly Maze maze;
        private readonly float cellWidth;
        private readonly float cellHeight;
        private readonly float lineThickness;

        public Vector2f Position { get; set; }
        public Vector2f Size { get; }

        public DrawableMaze(Maze maze, int pixelWidth, int pixelHeight, float lineThickness)
        {
            this.maze = maze;
            this.lineThickness = lineThickness;
            cellWidth = pixelWidth / maze.Width;
            cellHeight = pixelHeight / maze.Height;

            Size = new Vector2f(pixelWidth, pixelHeight);
        }

        private void DrawCell(int row, int col, RenderTarget target)
        {
            var cell = maze[new CellPosition(row, col)];

            if (cell.TopSide == SideState.Closed)
            {
                var rect = new RectangleShape
                {
                    Position = new Vector2f(col * cellWidth + Position.X, row * cellHeight + Position.Y),
                    Size = new Vector2f(cellWidth + lineThickness, lineThickness),
                    FillColor = Color.Black
                };
                target.Draw(rect);
            }
            if (cell.LeftSide == SideState.Closed)
            {
                var rect = new RectangleShape
                {
                    Position = new Vector2f(col * cellWidth + Position.X, row * cellHeight + Position.Y),
                    Size = new Vector2f(lineThickness, cellHeight + lineThickness),
                    FillColor = Color.Black
                };
                target.Draw(rect);
            }
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            var background = new RectangleShape
            {
                Position = Position,
                Size = new Vector2f(cellWidth * maze.Width, cellHeight * maze.Height),
                FillColor = Color.White
            };
            target.Draw(background);

            for (int i = 0; i < maze.Height; ++i)
            {
                for (int j = 0; j < maze.Width; ++j)
                {
                    DrawCell(i, j, target);
                }
            }

            var bottomLine = new RectangleShape
            {
                Position = new Vector2f(Position.X, cellHeight * maze.Height + Position.Y),
                Size = new Vector2f(cellWidth * maze.Width + lineThickness, lineThickness),
                FillColor = Color.Black
            };
            target.Draw(bottomLine);

            var rightLine = new RectangleShape
            {
                Position = new Vector2f(cellWidth * maze.Width + Position.X, Position.Y),
                Size = new Vector2f(lineThickness, cellHeight * maze.Height + lineThickness),
                FillColor = Color.Black
            };
            target.Draw(rightLine);
        }
    }
}
