using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mazes.Generation;
using Mazes.Generation.Interfaces;
using Mazes.Generation.MazeGenerators;
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
        private static ComboBox comboBox;
        private static Gui gui;

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
            if (comboBox?.GetSelectedItemId() == "0")
            {
                mazeGenerator = new AldousBroderGenerator();
            }
            else if (comboBox?.GetSelectedItemId() == "1")
            {
                mazeGenerator = new PrimaModifiedGenerator();
            }
            else if (comboBox?.GetSelectedItemId() == "2")
            {
                mazeGenerator = new RecursiveBacktrackerGenerator();
            }

            var maze = mazeGenerator.Generate(40, 40);
            drawableMaze = new DrawableMaze(maze, (int)window.Size.Y - 20, (int)window.Size.Y - 20, 5);
            drawableMaze.Position = new Vector2f(10, 10);
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

            var generateButton = new Button
            {
                Text = "Generate",
                Size = new Vector2f(150, 40),
                Position = new Vector2f(drawableMaze.Position.X + drawableMaze.Size.X + 10, 10)
            };
            generateButton.Pressed += (s, e) =>
            {
                NewDrawableMaze();
            };
            gui.Add(generateButton);


            comboBox = new ComboBox
            {
                Position = new Vector2f(
                    drawableMaze.Position.X + drawableMaze.Size.X + 10,
                    generateButton.Position.Y + generateButton.Size.Y + 10
                ),
                Size = new Vector2f(150, 30)
            };
            comboBox.AddItem("Aldous Broder", "0");
            comboBox.AddItem("Prima Modified", "1");
            comboBox.AddItem("Recursive Backtracker", "2");
            comboBox.SetSelectedItemById("0");
            gui.Add(comboBox);
        }
    }
}
