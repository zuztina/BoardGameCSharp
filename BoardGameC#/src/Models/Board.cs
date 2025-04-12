

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
            Console.WriteLine($"posledno red: {Width-3}");
                for (int j = 2; j < Width-2; j++)
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
                    //tmpPlayer.DisplayPlayer();
                    // update cell to occupied and add index to occupancy list
                    UpdateCellOccupation(randomPosition.X, randomPosition.Y);
                    OccupiedPositionIndex.Add(randomIndex);
                    return true;
                }
                else
                {
                    // find new position
                    int tmpIndex = FindNextPosition(randomIndex);
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
            Console.WriteLine($"occupied indexes");
            foreach (var element in OccupiedPositionIndex)
            {
                Console.WriteLine(element);
            }
            int moveIndex = DiceRoll();
            int maxPosition = StartingPositions.Count();
            // new index
            int newIndex = PlayerPool[playerIndex].PositionIndex + moveIndex;
            if (newIndex >= maxPosition)
            {
                newIndex = newIndex - maxPosition;
            }
            // check position availibility
            if (OccupiedPositionIndex.Contains(newIndex))
            {
                // find new position
                newIndex = FindNextPosition(newIndex);
                Console.WriteLine($"closest to new position index after dice roll: {newIndex}");

            }
            var newPosition = StartingPositions[newIndex];
            Console.WriteLine($"old position index: {PlayerPool[playerIndex].PositionIndex}, position {StartingPositions[PlayerPool[playerIndex].PositionIndex]}");
            Console.WriteLine($"new position index: {newIndex}, position {newPosition}");
            //
            Console.WriteLine("updating palyers postion...");
            /*Console.WriteLine("old player status");
            PlayerPool[playerIndex].DisplayPlayer();
            */
            OccupiedPositionIndex.Remove(PlayerPool[playerIndex].PositionIndex);
            // update old cell
            UpdateCellOccupation(PlayerPool[playerIndex].Position.Item1, PlayerPool[playerIndex].Position.Item2);
            Console.WriteLine("new player status");
            PlayerPool[playerIndex].UpdatePosition(newPosition, newIndex);
            //PlayerPool[playerIndex].DisplayPlayer();
            // update new cell
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