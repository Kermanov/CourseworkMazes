using System;
using System.Collections.Generic;
using System.Text;

namespace Mazes.Generation.MazeGenerators
{
    public class PrimaModifiedGenerator : MazeGeneratorBase
    {
        private enum State: byte
        {
            External,
            Boundary,
            Internal
        }

        private State[,] cellsState;
        private uint internalCounter;
        private Random rand;

        public PrimaModifiedGenerator()
        {
            rand = new Random();
        }

        public override Maze Generate(int width, int height)
        {
            maze = new Maze(width, height);
            cellsState = new State[height, width];
            internalCounter = 0;

            var startCell = new CellPosition(0, 0);
            SetInternal(startCell);
            SetNextExternalToBoundary(startCell);

            while (internalCounter < maze.Width * maze.Height)
            {
                var randomBoundary = GetRandomBoundary();
                var randomNextInternal = GetRandomNextInternal(randomBoundary);
                RemoveWall(randomBoundary, randomNextInternal);

                SetInternal(randomBoundary);
                SetNextExternalToBoundary(randomBoundary);
            }

            return maze;
        }

        private void SetNextExternalToBoundary(CellPosition cellPosition)
        {
            var nextCells = GetNextCellPositions(cellPosition);
            foreach (var cellPos in nextCells)
            {
                if (cellsState[cellPos.Row, cellPos.Col] == State.External)
                {
                    cellsState[cellPos.Row, cellPos.Col] = State.Boundary;
                }
            }
        }

        private CellPosition GetRandomNextInternal(CellPosition cellPosition)
        {
            var nextCells = GetNextCellPositions(cellPosition);
            nextCells.RemoveAll(cell => cellsState[cell.Row, cell.Col] != State.Internal);
            var randomIndex = rand.Next(0, nextCells.Count);
            return nextCells[randomIndex];
        }

        private void SetInternal(CellPosition cellPosition)
        {
            cellsState[cellPosition.Row, cellPosition.Col] = State.Internal;
            internalCounter++;
        }

        private CellPosition GetRandomBoundary()
        {
            var boundaryCells = new List<CellPosition>();
            for (int i = 0; i < maze.Height; ++i)
            {
                for (int j = 0; j < maze.Width; ++j)
                {
                    if (cellsState[i, j] == State.Boundary)
                    {
                        boundaryCells.Add(new CellPosition(i, j));
                    }
                }
            }

            var randomIndex = rand.Next(0, boundaryCells.Count);
            return boundaryCells[randomIndex];
        }
    }
}
