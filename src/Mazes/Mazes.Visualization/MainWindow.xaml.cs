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
using Mazes.Generation;
using Mazes.Generation.Interfaces;
using Mazes.Generation.MazeGenerators;

namespace Mazes.Visualization
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void generateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int sideSize = int.Parse(sideSizeTextBox.Text);

                IMazeGenerator mazeGenerator = null;
                if (mazeTypeComboBox.SelectedIndex == 0)
                {
                    mazeGenerator = new RecursiveBacktrackerGenerator();
                }

                var maze = mazeGenerator.Generate(sideSize, sideSize);
                mazeScreen.SetMaze(maze);
            }
            catch (Exception ex)
            {

            }
        }

        private void mazeStyleComboBox_DropDownClosed(object sender, EventArgs e)
        {
            if (mazeStyleComboBox.SelectedIndex == 0)
            {
                mazeScreen.SetStyle(MazeStyle.Default);
            }
            else if (mazeStyleComboBox.SelectedIndex == 1)
            {
                mazeScreen.SetStyle(MazeStyle.Bold);
            }
        }
    }
}
