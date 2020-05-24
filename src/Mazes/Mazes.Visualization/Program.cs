using SFML.Graphics;
using SFML.Window;

namespace Mazes.Visualization
{
    public partial class Program
    {
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
            }
        }
    }
}
