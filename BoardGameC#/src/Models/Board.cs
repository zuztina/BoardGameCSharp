

namespace BoardGameC_.Models
{

    public class Board
    {

        public int Height { get; private set; }
        public int Width { get; private set; }

        private Cell[,] BoardCells;
        public List<Player> PlayerPool;
        public HashSet<string> UniqueNicknames;
        public List<(int X, int Y)> StartingPositions;
        public HashSet<int> OccupiedPositionIndex; // store used indices

        public Board(int initHeight, int initWidth)
        {
            Height = initHeight;
            Width = initWidth;
            BoardCells = new Cell[Height, Width];
            StartingPositions = new List<(int X, int Y)>();
            OccupiedPositionIndex = new HashSet<int>();
            PlayerPool = new List<Player>();
            UniqueNicknames = new HashSet<string>();

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

        public void UpdateCellOccupation(int cellRow, int cellColumn)
        {
            BoardCells[cellRow, cellColumn].ChangeOccupation();
        }

        public void ShowGameCells()
        {
            Console.WriteLine("Starting Positions:");
            foreach (var position in StartingPositions)
            {
                // Print each tuple directly
                Console.WriteLine($"({position.X}, {position.Y})");
            }
        }

        // Method to validate the nickname (checks if it's non-empty and unique)
        public bool IsValidNickname(string nickname)
        {
            return !string.IsNullOrEmpty(nickname) && !UniqueNicknames.Contains(nickname);
        }
        public int FindNextPosition(int positionIndex)
        {
            int maxPosition = StartingPositions.Count();
            int forwardDistance = int.MaxValue;
            int backwardDistance = int.MaxValue;
            int forwardIndex = -1;
            int backwardIndex = -1;

            for (int i = positionIndex + 1; i < maxPosition; i++)
            {
                if (!OccupiedPositionIndex.Contains(i))
                {
                    forwardDistance = i - positionIndex;
                    forwardIndex = i;
                    break;
                }
            }
            for (int i = positionIndex - 1; i >= 0; i--)
            {
                if (!OccupiedPositionIndex.Contains(i))
                {
                    backwardDistance = positionIndex - i;
                    backwardIndex = i;
                    break;
                }
            }

            // Decide which index to return
            if (forwardDistance <= backwardDistance && forwardIndex != -1)
            {
                return forwardIndex;
            }
            else if (backwardIndex != -1)
            {
                return backwardIndex;
            }

            // If no index is available
            throw new Exception("No available starting positions.");

        }
        public bool AddPlayer(string? nick)
        {
            if (IsValidNickname(nick))
            {
                // add player nickname to unique list
                UniqueNicknames.Add(nick);
                // get starting positions
                Random random = new Random();
                int randomIndex = random.Next(StartingPositions.Count);
                var randomPosition = StartingPositions[randomIndex];
                //check position occuapncy
                Console.WriteLine($"random position index: {randomIndex}, position {randomPosition}");
                if (!OccupiedPositionIndex.Contains(randomIndex))
                {
                    // create new player instance
                    Player tmpPlayer = new Player(nick, randomPosition, randomIndex);
                    // add new Player to Pool
                    PlayerPool.Add(tmpPlayer);
                    tmpPlayer.DisplayPlayer();
                    // update cell to occupied and add index to occupancy list
                    UpdateCellOccupation(randomPosition.X, randomPosition.Y);
                    OccupiedPositionIndex.Add(randomIndex);
                    return true;
                }
                else
                {
                    // find new position
                    int tmpIndex =  FindNextPosition(randomIndex);
                    var tmpPosition = StartingPositions[tmpIndex];
                    Console.WriteLine($"closest to random position index: {tmpIndex}, position {tmpPosition}");
                    // create new player instance
                    Player tmpPlayer = new Player(nick, tmpPosition, randomIndex);
                    // add new Player to Pool
                    PlayerPool.Add(tmpPlayer);
                    tmpPlayer.DisplayPlayer();
                    // update cell to occupied and add index to occupancy list
                    UpdateCellOccupation(tmpPosition.X, tmpPosition.Y);
                    OccupiedPositionIndex.Add(randomIndex);
                    return true;
                }
            }
            return false;
        }

        public void MovePlayer(int playerIndex)
        {
            Console.WriteLine("updating palyers postion...");
            Random random = new Random();
            int randomIndex = random.Next(StartingPositions.Count);
            var tmpPosition = StartingPositions[randomIndex];
            Console.WriteLine($"new position index: {randomIndex}, position {tmpPosition}");
            Console.WriteLine("old player status");
            PlayerPool[playerIndex].DisplayPlayer();
            OccupiedPositionIndex.Remove(PlayerPool[playerIndex].PositionIndex);
            // update old cell
            UpdateCellOccupation(PlayerPool[playerIndex].Position.Item1, PlayerPool[playerIndex].Position.Item2);
            Console.WriteLine("new player status");
            PlayerPool[playerIndex].UpdatePosition(tmpPosition, randomIndex);
            PlayerPool[playerIndex].DisplayPlayer();
            // update new cell
            UpdateCellOccupation(PlayerPool[playerIndex].Position.Item1, PlayerPool[playerIndex].Position.Item2);
            OccupiedPositionIndex.Add(randomIndex);
        }
    }


}