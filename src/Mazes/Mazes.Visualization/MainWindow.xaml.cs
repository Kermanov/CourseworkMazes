using Mazes.Generation;
using Mazes.Generation.Interfaces;
using Mazes.Generation.MazeGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Mazes.Visualization
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MazeCanvas mazeCanvas;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void generateButton_Click(object sender, RoutedEventArgs e)
        {
            IMazeGenerator mazeGenerator = null;
            if (mazeTypeComboBox.SelectedIndex == 0)
            {
                mazeGenerator = new PerfectMazeGenerator();
            }

            try
            {
                int width = int.Parse(widthTextBox.Text);
                int height = int.Parse(heightTextBox.Text);

                var maze = await Task.Run(() => mazeGenerator.Generate(width, height));

                if (mazeCanvas == null)
                {
                    mazeCanvas = new MazeCanvas(maze);
                    mainGrid.Children.Add(mazeCanvas);
                }
                else
                {
                    mazeCanvas.Maze = maze;
                }
            }
            catch
            {

            }
        }

        private void mazeStyleComboBox_DropDownClosed(object sender, EventArgs e)
        {
            if (mazeCanvas != null)
            {
                if (mazeStyleComboBox.SelectedIndex == 0)
                {
                    mazeCanvas.MazeStyle = MazeStyle.Default;
                }
                else if (mazeStyleComboBox.SelectedIndex == 1)
                {
                    mazeCanvas.MazeStyle = MazeStyle.Bold;
                }
            }
        }
    }
}
