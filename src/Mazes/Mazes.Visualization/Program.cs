using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mazes.Generation;
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
        static void Main(string[] args)
        {
            window = new RenderWindow(VideoMode.FullscreenModes[0], "Mazes", Styles.Fullscreen);
            window.Closed += (s, e) => window.Close();
            window.SetActive();

            NewDrawableMaze();

            var gui = new Gui(window);

            var generateButton = new Button
            {
                Text = "Generate",
                Size = new Vector2f(window.Size.X / 10, window.Size.Y / 10),
                Position = new Vector2f(window.Size.X * 9 / 10, 10)
            };
            generateButton.Pressed += (s, e) =>
            {
                NewDrawableMaze();
            };

            gui.Add(generateButton);


            while (window.IsOpen)
            {
                window.Clear(new Color(232, 232, 232));
                window.DispatchEvents();

                window.Draw(drawableMaze);
                gui.Draw();

                window.Display();
            }
        }

        private static void NewDrawableMaze()
        {
            var mazeGenerator = new PrimaModifiedGenerator();
            var maze = mazeGenerator.Generate(40, 40);
            drawableMaze = new DrawableMaze(maze, (int)window.Size.Y - 20, (int)window.Size.Y - 20, 5);
            drawableMaze.Position = new Vector2f(10, 10);
        }
    }
}
