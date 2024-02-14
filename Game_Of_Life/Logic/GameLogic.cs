using Game_Of_Life.Models;

namespace Game_Of_Life.Logic
{
    public class GameLogic
    {
        private int gridSize;
        private Cell[,] currentGeneration;
        private Cell[,] nextGeneration;

        public GameLogic(int size)
        {
            gridSize = size;
            currentGeneration = new Cell[size, size];
            nextGeneration = new Cell[size, size];

            // Initialize the grid with dead cells
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    currentGeneration[i, j] = new Cell();
                    nextGeneration[i, j] = new Cell();
                }
            }
        }

        public Cell[,] GetGrid()
        {
            return currentGeneration;
        }

        public void CalculateNextGeneration()
        {
            // Iterate through each cell in the grid
            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    int liveNeighbors = CountLiveNeighbors(i, j);

                    // Apply the rules of the Game of Life
                    if (currentGeneration[i, j].IsAlive)
                    {
                        // Cell is alive
                        nextGeneration[i, j].IsAlive = liveNeighbors == 2 || liveNeighbors == 3;
                    }
                    else
                    {
                        // Cell is dead
                        nextGeneration[i, j].IsAlive = liveNeighbors == 3;
                    }
                }
            }

            // Update the current generation with the next generation
            SwapGenerations();
        }

        private int CountLiveNeighbors(int row, int col)
        {
            int liveNeighbors = 0;

            for (int i = row - 1; i <= row + 1; i++)
            {
                for (int j = col - 1; j <= col + 1; j++)
                {
                    // Skip the current cell
                    if (i == row && j == col)
                        continue;

                    // Check boundaries
                    if (i >= 0 && i < gridSize && j >= 0 && j < gridSize)
                    {
                        if (currentGeneration[i, j].IsAlive)
                        {
                            liveNeighbors++;
                        }
                    }
                }
            }

            return liveNeighbors;
        }

        private void SwapGenerations()
        {
            // Swap current and next generations
            Cell[,] temp = currentGeneration;
            currentGeneration = nextGeneration;
            nextGeneration = temp;
        }
    }
}
