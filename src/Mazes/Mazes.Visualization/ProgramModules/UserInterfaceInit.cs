using System.IO;
using Mazes.Visualization.DrawableElements;
using SFML.Graphics;
using SFML.System;
using TGUI;

namespace Mazes.Visualization
{
    public partial class Program
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
            solvingCombobox.AddItem("Wall Follower (left)", "0");
            solvingCombobox.AddItem("Wall Follower (right)", "1");
            solvingCombobox.AddItem("Recursive Backtracker", "2");
            solvingCombobox.AddItem("Random Mouse", "3");
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
