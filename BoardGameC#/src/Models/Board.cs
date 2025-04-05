

namespace BoardGameC_.Models
{

    public class Board
    {

        public int Height { get; private set; }
        public int Width { get; private set; }

        private Cell[,] BoardCells;
        private Player[] Pool;

        public List<(int X, int Y)> StartingPositions;

        public Board(int initHeight, int initWidth)
        {
            Height = initHeight;
            Width = initWidth;
            BoardCells = new Cell[Height, Width];
            StartingPositions = new List<(int X, int Y)>();

            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    BoardCells[i, j] = new EmptyCell(i, j);  // Create an empty cell
                }
            }
        }
        // Display the board on the console
        public void DisplayBoard()
        {
            //Console.WriteLine();
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    BoardCells[i, j].Display();  // Call the Display method for each cell
                }
                Console.WriteLine(); // Move to the next line after each row
            }
        }

        public void AddYellowCells()
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    if (j == 1 && i >= 1 && i < Height - 1)
                    {
                        BoardCells[i, j] = new Cell(Colors.Yellow, i, j);
                        StartingPositions.Add((i, j));
                    }
                }
            }
        }
        public void AddBlueCells()
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    if (j == Width - 2 && i >= 1 && i < Height - 1)
                    {
                        BoardCells[i, j] = new Cell(Colors.Blue, i, j);
                        StartingPositions.Add((i, j));
                    }
                }
            }
        }
        public void AddRedCells()
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    if (i == 1 && j >= 2 && j < Width - 3)
                    {
                        BoardCells[i, j] = new Cell(Colors.Red, i, j);
                        StartingPositions.Add((i, j));
                    }
                }
            }
        }

        public void AddGreenCells()
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    if (i == Height - 2 && j >= 2 && j < Width - 3)
                    {
                        BoardCells[i, j] = new Cell(Colors.Green, i, j);
                        StartingPositions.Add((i, j));
                    }
                }
            }
        }

        public void UpdateCell(int cellRow, int cellColumn)
        {
            BoardCells[cellRow, cellColumn].ChangeOccupation();
        }

        public void ShowGameCells()
        {
            Console.WriteLine("Starting Positions:");
            foreach (var position in StartingPositions)
            {
                // Print each tuple directly
                Console.WriteLine($"({position.Item1}, {position.Item2})");
            }
        }
    }


}