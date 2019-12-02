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

        private async void generateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int sideSize = int.Parse(sideSizeTextBox.Text);

                IMazeGenerator mazeGenerator = null;
                if (mazeTypeComboBox.SelectedIndex == 0)
                {
                    mazeGenerator = new RecursiveBacktrackerGenerator();
                }
                else if (mazeTypeComboBox.SelectedIndex == 1)
                {
                    mazeGenerator = new PrimaModifiedGenerator();
                }
                else if (mazeTypeComboBox.SelectedIndex == 2)
                {
                    mazeGenerator = new AldousBroderGenerator();
                }

                generationProgressBar.IsIndeterminate = true;
                generateButton.IsEnabled = false;
                var maze = await Task.Run(() => mazeGenerator.Generate(sideSize, sideSize));
                generationProgressBar.IsIndeterminate = false;
                generateButton.IsEnabled = true;

                mazeScreen.SetMaze(maze);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
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
