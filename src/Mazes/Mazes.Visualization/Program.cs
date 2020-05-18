using System;
using System.Collections.Generic;
using System.IO;
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
        private static Button generateButton;
        private static Slider mazeSizeSlider;
        private static Slider pathDrawingSpeedSlider;
        private static Gui gui;
        private static Button solveButton;

        private static int interfaceColumnWidth = 250;
        private static int interfaceElementHeight = 40;
        private static int horizontalSpacing = 15;

        private static Maze maze;

        static Program()
        {
            window = new RenderWindow(VideoMode.FullscreenModes[0], "Mazes", Styles.Fullscreen);
            window.Closed += (s, e) => window.Close();
            window.SetActive();
            window.SetFramerateLimit(60);
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
            IMazeSolver mazeSolver = new RandomMouseSolver();
            if (solvingCombobox?.GetSelectedItemId() == "0")
            {
                mazeSolver = new RandomMouseSolver();
            }
            else if (solvingCombobox?.GetSelectedItemId() == "1")
            {
                mazeSolver = new WallFollowerSolver(TurningDirection.Left);
            }
            else if (solvingCombobox?.GetSelectedItemId() == "2")
            {
                mazeSolver = new WallFollowerSolver(TurningDirection.Right);
            }
            else if (solvingCombobox?.GetSelectedItemId() == "3")
            {
                mazeSolver = new RecursiveBacktrackerSolver();
            }

            var solution = mazeSolver.Solve(maze, new CellPosition(0, 0), new CellPosition(maze.Height - 1, maze.Width - 1));
            drawableMaze.Path = solution.FullPath ?? new List<CellPosition>();
            drawableMaze.FinalPath = solution.Solution ?? new List<CellPosition>();
            drawableMaze.PathDrawingSpeed = (int)pathDrawingSpeedSlider.Value;
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
            InitNavigationElements();
        }

        static void InitSolveSection()
        {
            solveButton = new Button
            {
                Text = "Solve",
                Size = new Vector2f(interfaceColumnWidth, interfaceElementHeight * 1.5f),
                TextSize = 20
            };
            solveButton.Pressed += (s, e) =>
            {
                SolveMaze();   
            };
            gui.Add(solveButton);
            PlaceWidgetBelow(mazeSizeSlider, solveButton, 80);

            var solvingTypeLabel = new Label
            {
                Text = "Solving method",
                Size = new Vector2f(interfaceColumnWidth, interfaceElementHeight / 2),
                TextSize = 14
            };
            gui.Add(solvingTypeLabel);
            PlaceWidgetBelow(solveButton, solvingTypeLabel);

            solvingCombobox = new ComboBox
            {
                Size = new Vector2f(interfaceColumnWidth, interfaceElementHeight),
                TextSize = 14
            };
            solvingCombobox.AddItem("Random Mouse", "0");
            solvingCombobox.AddItem("Wall Follower (left)", "1");
            solvingCombobox.AddItem("Wall Follower (right)", "2");
            solvingCombobox.AddItem("Recursive Backtracker", "3");
            solvingCombobox.SetSelectedItemById("0");
            gui.Add(solvingCombobox);
            PlaceWidgetBelow(solvingTypeLabel, solvingCombobox, 5);

            var speedSliderLabel = new Label
            {
                Text = "Drawing speed",
                Size = new Vector2f(interfaceColumnWidth, interfaceElementHeight / 2),
                TextSize = 14
            };
            gui.Add(speedSliderLabel);
            PlaceWidgetBelow(solvingCombobox, speedSliderLabel);

            pathDrawingSpeedSlider = new Slider
            {
                Size = new Vector2f(interfaceColumnWidth, interfaceElementHeight / 2),
                Minimum = 1,
                Maximum = 100,
                Step = 1,
                InvertedDirection = true,
                Name = "Draw speed",
                Value = 50
            };
            pathDrawingSpeedSlider.ValueChanged += (s, e) =>
            {
                drawableMaze.PathDrawingSpeed = (int)e.Value;
            };
            gui.Add(pathDrawingSpeedSlider);
            PlaceWidgetBelow(speedSliderLabel, pathDrawingSpeedSlider, 5);
        }

        static void InitGenerateSection()
        {
            generateButton = new Button
            {
                Text = "Generate",
                Size = new Vector2f(interfaceColumnWidth, interfaceElementHeight * 1.5f),
                Position = new Vector2f(drawableMaze.Position.X + drawableMaze.Size.X + horizontalSpacing, 10),
                TextSize = 20
            };
            generateButton.Pressed += (s, e) =>
            {
                NewDrawableMaze();
            };
            gui.Add(generateButton);

            var generetionTypeLabel = new Label
            {
                Text = "Generation type",
                Size = new Vector2f(interfaceColumnWidth, interfaceElementHeight / 2),
                TextSize = 14
            };
            gui.Add(generetionTypeLabel);
            PlaceWidgetBelow(generateButton, generetionTypeLabel);

            generationCombobox = new ComboBox
            {
                Size = new Vector2f(interfaceColumnWidth, interfaceElementHeight),
                TextSize = 14
            };
            generationCombobox.AddItem("Aldous Broder", "0");
            generationCombobox.AddItem("Prima Modified", "1");
            generationCombobox.AddItem("Recursive Backtracker", "2");
            generationCombobox.SetSelectedItemById("0");
            gui.Add(generationCombobox);
            PlaceWidgetBelow(generetionTypeLabel, generationCombobox, 5);

            var sizeSliderLabel = new Label
            {
                Text = "Maze size",
                Size = new Vector2f(interfaceColumnWidth, interfaceElementHeight / 2),
                TextSize = 14
            };
            gui.Add(sizeSliderLabel);
            PlaceWidgetBelow(generationCombobox, sizeSliderLabel);

            mazeSizeSlider = new Slider
            {
                Size = new Vector2f(interfaceColumnWidth, interfaceElementHeight / 2),
                Minimum = 8,
                Maximum = 64,
                Step = 1,
                Value = 23,
                Name = "Maze size"
            };
            gui.Add(mazeSizeSlider);
            PlaceWidgetBelow(sizeSliderLabel, mazeSizeSlider, 5);
        }

        private static void InitNavigationElements()
        {
            var closeImageTexture = new Texture(ImageToByte(Properties.Resources.close_image));
            var closeImageFocusedTexture = new Texture(ImageToByte(Properties.Resources.close_image_focused));

            var closeButton = new BitmapButton()
            {
                Image = closeImageTexture,
                Size = new Vector2f(50, 50),
                Position = new Vector2f(window.Size.X - 50, 0)
            };
            closeButton.Clicked += (s, e) =>
            {
                window.Close();
            };
            closeButton.MouseEntered += (s, e) =>
            {
                closeButton.Image = closeImageFocusedTexture;
            };
            closeButton.MouseLeft += (s, e) =>
            {
                closeButton.Image = closeImageTexture;
            };

            gui.Add(closeButton);
        }

        private static void PlaceWidgetBelow(Widget firstWidget, Widget secondWidget, int margin = 10)
        {
            secondWidget.Position = new Vector2f
            {
                X = firstWidget.Position.X,
                Y = firstWidget.Position.Y + firstWidget.Size.Y + margin
            };
        }

        public static byte[] ImageToByte(System.Drawing.Bitmap bitmap)
        {
            using (var memoryStream = new MemoryStream())
            {
                bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                return memoryStream.ToArray();
            }
        }
    }
}
