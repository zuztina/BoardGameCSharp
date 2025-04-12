

namespace BoardGameC_.Models
{

    public class Board
    {

        public int Height { get; private set; }
        public int Width { get; private set; }

        public Cell[,] BoardCells;
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
            // adding colored gamecells to board
            AddRedCells();
            AddBlueCells();
            AddGreenCells();
            AddYellowCells();
            StartingPositions.Remove((1, 1));
            StartingPositions.Insert(0, (1, 1));
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

        public void DisplayBoardPlayer(Player currentPlayer)
        {
            //Console.WriteLine();
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    // players position
                    if (i == currentPlayer.Position.Item1 && j == currentPlayer.Position.Item2)
                    {
                        BoardCells[i, j].DisplayPlayerPosition();
                    }
                    else
                    {
                        BoardCells[i, j].Display();  // Call the Display method for each cell
                    }
                }
                Console.WriteLine(); // Move to the next line after each row
            }
        }

        public void AddYellowCells()
        {
            for (int i = Height - 1; i >= 1; i--)
            {
                if (i < Height - 1)
                {
                    BoardCells[i, 1] = new Cell(Colors.Yellow, i, 1);
                    StartingPositions.Add((i, 1));
                }

            }
        }
        public void AddBlueCells()
        {
            for (int i = 1; i < Height - 1; i++)
            {
                BoardCells[i, Width - 2] = new Cell(Colors.Blue, i, Width - 2);
                StartingPositions.Add((i, Width - 2));
            }
        }
        public void AddRedCells()
        {
            for (int j = 2; j < Width - 2; j++)
            {
                BoardCells[1, j] = new Cell(Colors.Red, 1, j);
                StartingPositions.Add((1, j));
            }

        }

        public void AddGreenCells()
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = Width - 1; j >= 0; j--)
                {
                    if (i == Height - 2 && j >= 2 && j < Width - 2)
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
            for (int i = 0; i < StartingPositions.Count; i++)
            {
                var position = StartingPositions[i];
                Console.WriteLine($"({position.X}, {position.Y}, index: {i})");
            }
        }

        // Method to validate the nickname (checks if it's non-empty and unique)
        public bool IsValidNickname(string nickname)
        {
            return !string.IsNullOrEmpty(nickname) && !UniqueNicknames.Contains(nickname);
        }
        public int FindNextPosition(int positionIndex, int direction)
        {
            if (direction == 0)
                throw new ArgumentException("Direction cannot be zero.");

            int maxPosition = StartingPositions.Count;
            int newIndex = positionIndex;

            while (true)
            {
                newIndex = (newIndex + direction + maxPosition) % maxPosition;

                if (!OccupiedPositionIndex.Contains(newIndex))
                {
                    return newIndex;
                }

                // Stop if we looped around the entire board
                if (newIndex == positionIndex)
                {
                    break;
                }
            }

            throw new Exception("No available starting positions in that direction.");
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
                //Console.WriteLine($"random position index: {randomIndex}, position {randomPosition}");
                if (!OccupiedPositionIndex.Contains(randomIndex))
                {
                    // create new player instance
                    Player tmpPlayer = new Player(nick, randomPosition, randomIndex);
                    // add new Player to Pool
                    PlayerPool.Add(tmpPlayer);
                    //tmpPlayer.DisplayPlayer();
                    // update cell to occupied and add index to occupancy list
                    UpdateCellOccupation(randomPosition.X, randomPosition.Y);
                    OccupiedPositionIndex.Add(randomIndex);
                    DisplayBoardPlayer(tmpPlayer);
                    return true;
                }
                else
                {
                    // find new position
                    int tmpIndex = FindNextPosition(randomIndex, 1);
                    var tmpPosition = StartingPositions[tmpIndex];
                    //Console.WriteLine($"closest to random position index: {tmpIndex}, position {tmpPosition}");
                    // create new player instance
                    Player tmpPlayer = new Player(nick, tmpPosition, randomIndex);
                    // add new Player to Pool
                    PlayerPool.Add(tmpPlayer);
                    //tmpPlayer.DisplayPlayer();
                    // update cell to occupied and add index to occupancy list
                    UpdateCellOccupation(tmpPosition.X, tmpPosition.Y);
                    OccupiedPositionIndex.Add(randomIndex);
                    DisplayBoardPlayer(tmpPlayer);
                    return true;
                }
            }
            return false;
        }

        public void MovePlayer(int playerIndex)
        {
            int moveIndex = DiceRoll();
            Console.WriteLine("Jakým směrem po desce se chcete posunout? [L/P]");
            string directionInput = Console.ReadLine()?.Trim().ToUpper();

            if (string.IsNullOrEmpty(directionInput) || (directionInput[0] != 'L' && directionInput[0] != 'P'))
            {
                Console.WriteLine("Neplatný směr. Hráč zůstává na místě.");
                return;
            }

            int direction = directionInput[0] == 'L' ? -1 : 1;
            int maxPosition = StartingPositions.Count;

            // Calculate new index with wrapping
            int newIndex = (PlayerPool[playerIndex].PositionIndex + direction * moveIndex + maxPosition) % maxPosition;

            // If position is occupied, find nearest available in given direction
            if (OccupiedPositionIndex.Contains(newIndex))
            {
                newIndex = FindNextPosition(newIndex, direction);
                Console.WriteLine($"Pozice obsazena. Nejbližší dostupná pozice: {newIndex}");
            }

            // Move the player
            var newPosition = StartingPositions[newIndex];
            OccupiedPositionIndex.Remove(PlayerPool[playerIndex].PositionIndex);
            UpdateCellOccupation(PlayerPool[playerIndex].Position.Item1, PlayerPool[playerIndex].Position.Item2);

            PlayerPool[playerIndex].UpdatePosition(newPosition, newIndex);

            UpdateCellOccupation(PlayerPool[playerIndex].Position.Item1, PlayerPool[playerIndex].Position.Item2);
            OccupiedPositionIndex.Add(newIndex);
        }

        public int DiceRoll()
        {
            Console.WriteLine("Stiskněte mezerník pro hod kostkou...");

            // Wait for spacebar
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(intercept: true); // Do not show key in console
            } while (key.Key != ConsoleKey.Spacebar);

            // Simulate dice roll
            Random random = new Random();
            int rollAns = random.Next(1, 6);
            Console.WriteLine($"Výsledek tvého hodu {rollAns}");
            return rollAns;
        }
    }


}