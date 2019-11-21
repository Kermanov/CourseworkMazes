using System;
using System.Collections.Generic;
using System.Text;

namespace Mazes.Generation.Interfaces
{
    public interface IMazeGenerator
    {
        Maze Generate(int width, int height);
    }
}
