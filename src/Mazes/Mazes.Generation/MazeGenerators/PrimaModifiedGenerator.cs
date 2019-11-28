using System;
using System.Collections.Generic;
using System.Text;

namespace Mazes.Generation.MazeGenerators
{
    public class PrimaModifiedGenerator : MazeGeneratorBase
    {
        private List<CellPosition> internalCells;
        private List<CellPosition> boundaryCells;
        private List<CellPosition> externalCells;
        private Random rand;

        public PrimaModifiedGenerator()
        {
            internalCells = new List<CellPosition>();
            boundaryCells = new List<CellPosition>();
            externalCells = new List<CellPosition>();
            rand = new Random();
        }

        public override Maze Generate(int width, int height)
        {
            maze = new Maze(width, height);

            for (int i = 0; i < maze.Height; ++i)
            {
                for (int j = 0; j < maze.Width; ++j)
                {
                    externalCells.Add(new CellPosition(i, j));
                }
            }

            var startCell = new CellPosition(0, 0);
            externalCells.Remove(startCell);
            internalCells.Add(startCell);
            SetNextExternalToBoundary(startCell);

            while (boundaryCells.Count > 0)
            {
                var randomBoundary = GetRandom(boundaryCells);
                var randomNextInternal = GetRandomNextInternal(randomBoundary);
                RemoveWall(randomBoundary, randomNextInternal);

                boundaryCells.Remove(randomBoundary);
                internalCells.Add(randomBoundary);
                SetNextExternalToBoundary(randomBoundary);
            }

            internalCells.Clear();

            return maze;
        }

        private void SetNextExternalToBoundary(CellPosition cellPosition)
        {
            var nextCells = GetNextCellPositions(cellPosition);
            foreach (var cellPos in nextCells)
            {
                if (externalCells.Contains(cellPos))
                {
                    externalCells.Remove(cellPos);
                    boundaryCells.Add(cellPos);
                }
            }
        }

        private CellPosition GetRandom(List<CellPosition> list)
        {
            if (list.Count > 0)
            {
                var randomIndex = rand.Next(0, list.Count);
                return list[randomIndex];
            }
            else
            {
                return null;
            }
        }

        private CellPosition GetRandomNextInternal(CellPosition cellPosition)
        {
            var nextCells = GetNextCellPositions(cellPosition);
            nextCells.RemoveAll(cell => !internalCells.Contains(cell));
            var randomNextInternal = GetRandom(nextCells);
            return randomNextInternal;
        }
    }
}
