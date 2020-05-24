using Mazes.Generation;
using Mazes.Generation.Interfaces;
using Mazes.Generation.MazeGenerators;
using Mazes.Solving.Interfaces;
using Mazes.Solving.MazeSolvers;
using Mazes.Visualization.DrawableElements;
using SFML.System;
using System.Collections.Generic;

namespace Mazes.Visualization
{
    public partial class Program
    {
        private static Maze maze;

        private static void NewDrawableMaze()
        {
            IMazeGenerator mazeGenerator = new AldousBroderGenerator();
            if (generationCombobox?.GetSelectedItemId() == "0")
            {
                mazeGenerator = new AldousBroderGenerator();
            }
            else if (generationCombobox?.GetSelectedItemId() == "1")
            {
                mazeGenerator = new PrimaModifiedGenerator();
            }
            else if (generationCombobox?.GetSelectedItemId() == "2")
            {
                mazeGenerator = new RecursiveBacktrackerGenerator();
            }

            var size = 23;
            if (mazeSizeSlider != null)
            {
                size = (int)mazeSizeSlider.Value;
            }

            maze = mazeGenerator.Generate(size, size);
            var startPos = new CellPosition(0, 0);
            var escapePos = new CellPosition(size - 1, size - 1);

            float mazeSize = window.Size.Y - 15;
            var mazeXPosition = (window.Size.X - (mazeSize + horizontalSpacing + interfaceColumnWidth)) / 2;

            drawableMaze = new DrawableMaze(maze, mazeSize, mazeSize, 5, startPos, escapePos);
            drawableMaze.Position = new Vector2f(mazeXPosition, 5);
        }

        private static void SolveMaze()
        {
            IMazeSolver mazeSolver = new WallFollowerSolver(TurningDirection.Left);
            if (solvingCombobox?.GetSelectedItemId() == "0")
            {
                mazeSolver = new WallFollowerSolver(TurningDirection.Left);
            }
            else if (solvingCombobox?.GetSelectedItemId() == "1")
            {
                mazeSolver = new WallFollowerSolver(TurningDirection.Right);
            }
            else if (solvingCombobox?.GetSelectedItemId() == "2")
            {
                mazeSolver = new RecursiveBacktrackerSolver();
            }
            else if (solvingCombobox?.GetSelectedItemId() == "3")
            {
                mazeSolver = new RandomMouseSolver();
            }

            var solution = mazeSolver.Solve(maze, new CellPosition(0, 0), new CellPosition(maze.Height - 1, maze.Width - 1));
            drawableMaze.Path = solution.FullPath ?? new List<CellPosition>();
            drawableMaze.FinalPath = solution.Solution ?? new List<CellPosition>();
            drawableMaze.PathDrawingSpeed = (int)pathDrawingSpeedSlider.Value;
        }
    }
}
