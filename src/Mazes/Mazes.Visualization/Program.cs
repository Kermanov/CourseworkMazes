using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mazes.Generation;
using Mazes.Generation.Interfaces;
using Mazes.Generation.MazeGenerators;
using Mazes.Solving.Interfaces;
using Mazes.Solving.MazeSolvers;
using SFML;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using TGUI;

namespace Mazes.Visualization
{
    class Program
    {
        private static RenderWindow window;
        private static DrawableMaze drawableMaze;
        private static ComboBox generationCombobox;
        private static ComboBox solvingCombobox;
        private static Grid generateSectionGrid;
        private static Grid solveSectionGrid;
        private static Button generateButton;
        private static Label sliderLabel;
        private static Slider slider;
        private static Gui gui;
        private static Button solveButton;

        private static Maze maze;

        static Program()
        {
            window = new RenderWindow(VideoMode.FullscreenModes[0], "Mazes", Styles.Fullscreen);
            window.Closed += (s, e) => window.Close();
            window.SetActive();
        }

        static void Main(string[] args)
        {
            NewDrawableMaze();
            GuiInit();

            while (window.IsOpen)
            {
                window.Clear(new Color(232, 232, 232));
                window.DispatchEvents();

                window.Draw(drawableMaze);
                gui.Draw();

                window.Display();

                KeyActions();
            }
        }

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

            var size = 40;
            if (slider != null)
            {
                size = (int)slider.Value;
            }

            maze = mazeGenerator.Generate(size, size);
            var startPos = new CellPosition(0, 0);
            var escapePos = new CellPosition(size - 1, size - 1);

            drawableMaze = new DrawableMaze(maze, (int)window.Size.Y - 20, (int)window.Size.Y - 20, 5, startPos, escapePos);
            drawableMaze.Position = new Vector2f(10, 10);
        }

        private static void SolveMaze()
        {
            IMazeSolver mazeSolver = new RandomMouseSolver();
            if (solvingCombobox?.GetSelectedItemId() == "0")
            {
                mazeSolver = new RandomMouseSolver();
            }
            else if (solvingCombobox?.GetSelectedItemId() == "1")
            {
                mazeSolver = new WallFollowerSolver(TurningDirection.Right);
            }
            else if (solvingCombobox?.GetSelectedItemId() == "2")
            {
                mazeSolver = new RecursiveBacktrackerSolver();
            }

            var solution = mazeSolver.Solve(maze, new CellPosition(0, 0), new CellPosition(maze.Height - 1, maze.Width - 1));
            drawableMaze.Path = solution.FullPath ?? new List<CellPosition>();
            drawableMaze.FinalPath = solution.Solution ?? new List<CellPosition>();
        }

        private static void KeyActions()
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.Escape))
            {
                window.Close();
            }
        }

        private static void GuiInit()
        {
            gui = new Gui(window);

            InitGenerateSection();
            InitSolveSection();
        }

        static void InitSolveSection()
        {
            solveSectionGrid = new Grid
            {
                Position = new Vector2f(generateSectionGrid.Position.X + generateSectionGrid.Size.X + 50, 10),
                Size = new Vector2f(150, 100)
            };
            gui.Add(solveSectionGrid);

            solveButton = new Button
            {
                Text = "Solve",
                Size = new Vector2f(150, 40)
            };
            solveButton.Pressed += (s, e) =>
            {
                SolveMaze();   
            };
            solveSectionGrid.AddWidget(solveButton, 0, 0);

            solvingCombobox = new ComboBox
            {
                Size = new Vector2f(150, 30)
            };
            solvingCombobox.AddItem("Random Mouse", "0");
            solvingCombobox.AddItem("Wall Follower", "1");
            solvingCombobox.AddItem("Recursive Backtracker", "2");
            solvingCombobox.SetSelectedItemById("0");
            solveSectionGrid.AddWidget(solvingCombobox, 1, 0);
            solveSectionGrid.SetWidgetPadding(solvingCombobox, new Outline(10));
        }

        static void InitGenerateSection()
        {
            generateSectionGrid = new Grid
            {
                Position = new Vector2f(window.Size.Y + 10, 10),
                Size = new Vector2f(150, 200)
            };
            gui.Add(generateSectionGrid);

            generateButton = new Button
            {
                Text = "Generate",
                Size = new Vector2f(150, 40)
            };
            generateButton.Pressed += (s, e) =>
            {
                NewDrawableMaze();
            };
            generateSectionGrid.AddWidget(generateButton, 0, 0);


            generationCombobox = new ComboBox
            {
                Size = new Vector2f(150, 30)
            };
            generationCombobox.AddItem("Aldous Broder", "0");
            generationCombobox.AddItem("Prima Modified", "1");
            generationCombobox.AddItem("Recursive Backtracker", "2");
            generationCombobox.SetSelectedItemById("0");
            generateSectionGrid.AddWidget(generationCombobox, 1, 0);
            generateSectionGrid.SetWidgetPadding(generationCombobox, new Outline(10));


            sliderLabel = new Label
            {
                Text = "Maze size",
                Size = new Vector2f(150, 30)
            };
            generateSectionGrid.AddWidget(sliderLabel, 2, 0);


            slider = new Slider
            {
                Size = new Vector2f(150, 30),
                Minimum = 5,
                Maximum = 40,
                Step = 5
            };
            generateSectionGrid.AddWidget(slider, 3, 0);
        }
    }
}
