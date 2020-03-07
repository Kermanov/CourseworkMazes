using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Framework.WpfInterop;
using MonoGame.Framework.WpfInterop.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mazes.Generation;

namespace Mazes.Visualization
{
    public enum MazeStyle: byte
    {
        Default,
        Bold
    }

    public class MazeScreen: WpfGame
    {
        private IGraphicsDeviceService _graphicsDeviceManager;
        private WpfKeyboard _keyboard;
        private WpfMouse _mouse;

        private SpriteBatch spriteBatch;
        private Texture2D pixel;

        private Maze maze;
        private int cellSize;
        private int wallThickness;
        private MazeStyle mazeStyle;
        private const int mazePixelSize = 600;

        public void SetMaze(Maze maze)
        {
            this.maze = maze;
            UpdateCellSize();
            UpdateWallThickness();
        }

        public void SetStyle(MazeStyle mazeStyle)
        {
            this.mazeStyle = mazeStyle;
            UpdateWallThickness();
        }

        protected override void Initialize()
        {
            _graphicsDeviceManager = new WpfGraphicsDeviceService(this);

            _keyboard = new WpfKeyboard(this);
            _mouse = new WpfMouse(this);

            base.Initialize();


            spriteBatch = new SpriteBatch(_graphicsDeviceManager.GraphicsDevice);

            pixel = new Texture2D(_graphicsDeviceManager.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            pixel.SetData(new[] { Color.White });

            wallThickness = 2;
        }

        protected override void Update(GameTime time)
        {
            var mouseState = _mouse.GetState();
            var keyboardState = _keyboard.GetState();
        }

        protected override void Draw(GameTime time)
        { 
            GraphicsDevice.Clear(Color.White);

            DrawMaze();

            base.Draw(time);
        }

        private void DrawCell(Cell cell, CellPosition cellPosition)
        {
            if (cell.TopSide == SideState.Closed || cell.LeftSide == SideState.Closed)
            {
                var xRealPos = cellPosition.Col * cellSize;
                var yRealPos = cellPosition.Row * cellSize;

                if (cell.TopSide == SideState.Closed)
                {
                    spriteBatch.Draw(pixel, new Rectangle(xRealPos, yRealPos, cellSize + wallThickness, wallThickness), Color.Black);
                }
                if (cell.LeftSide == SideState.Closed)
                {
                    spriteBatch.Draw(pixel, new Rectangle(xRealPos, yRealPos, wallThickness, cellSize + wallThickness), Color.Black);
                }
            }
        }

        private void DrawMaze()
        {
            if (maze != null)
            { 
                spriteBatch.Begin();

                for (int i = 0; i < maze.Height; ++i)
                {
                    for (int j = 0; j < maze.Width; ++j)
                    {
                        var cellPos = new CellPosition(i, j);
                        DrawCell(maze[cellPos], cellPos);
                    }
                }

                spriteBatch.Draw(
                    pixel,
                    new Rectangle(0, maze.Height * cellSize, maze.Width * cellSize + wallThickness, wallThickness),
                    Color.Black);

                spriteBatch.Draw(
                    pixel, new Rectangle(maze.Width * cellSize, 0, wallThickness, maze.Width * cellSize + wallThickness),
                    Color.Black);

                spriteBatch.End();
            }
        }

        private void UpdateWallThickness()
        {
            if (mazeStyle == MazeStyle.Default)
            {
                wallThickness = 2;
            }
            else if (mazeStyle == MazeStyle.Bold)
            {
                wallThickness = cellSize / 2;
            }
        }

        private void UpdateCellSize()
        {
            cellSize = (int)Math.Round((float)mazePixelSize / maze.Height);

            this.Width = cellSize * (maze.Height + 0.5);
            this.Height = cellSize * (maze.Height + 0.5);
        }
    }
}
