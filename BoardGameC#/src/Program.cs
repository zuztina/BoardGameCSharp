using System.Data.Common;
using System.Diagnostics.Tracing;
using System.Drawing;
using System.Runtime.CompilerServices;
using BoardGameC_.Models;

namespace BoardGameC_;

// TODO solve placing players on same cells
class Program
{
    public void DisplayRules()
    {
        int tries = 0;
        string? rules = string.Empty;  // Initialize rules

        while (tries < 2)
        {
            Console.WriteLine("Přejete si slyšet pravidla hry? [ano/ne]");
            rules = Console.ReadLine()?.Trim().ToLower();  // Handle possible extra spaces and make input lowercase

            if (rules == "ano")
            {
                // Add actual rules
                Console.WriteLine("Hru začíná první zapsaný hráč v seznamu.");
                Console.WriteLine("Tah hráče začíná hodem kostkou, hráč se posune o určitý počet míst ve směru hry nebo případně na nejbližší volné pole.");
                Console.WriteLine("Hráč se může rozhodnout, jakým směrem se chce po desce hýbat. Směr L (doleva) je proti směru hodinových ručiček a P(doprava) je po směru hodinových ručiček.");
                Console.WriteLine("Poté je hráči položena otázka z dané kategorie. Pokud hráč odpoví správně, znovu hází kostkou a posouvá se na další pole, takto hráč pokračuje dokud odpovídá správně");
                Console.WriteLine("Pokud ale hráč odpoví špatně končí tím jeho tah a pokračuje další hráč stejným způsobem.");
                Console.WriteLine("Hra končí ve chvíli, kdy nějaký z hráčů nasbíral 4 správné odpovědi z každé kategorie.");
                break;  // Exit the loop once the user has seen the rules
            }
            else if (rules == "ne")
            {
                break;  // Exit the loop if the user doesn't want the rules
            }
            else
            {
                Console.WriteLine("Neplatná odpověď. Zadejte 'ano' nebo 'ne'.");
                tries++;  // Increment tries if the answer is invalid
            }
        }
        if (tries == 2)
        {
            Console.WriteLine("Příliš mnoho pokusů. Program se ukončí.");
            Environment.Exit(0);  // End the program after three invalid attempts
        }
    }

    public bool InitializePlayers(Board MainBoard, ref int playerCount)
    {
        // Player count initialization with validation
        int tries = 0;
        while (tries < 2)
        {
            Console.WriteLine("Prosím zadejte počet hráčů (2-6)...");
            playerCount = Convert.ToInt32(Console.ReadLine());
            if (playerCount >= 2 && playerCount <= 6)
            {
                Console.WriteLine($"Počet hráčů pro tuto hru: {playerCount}");
                break;
            }
            else
            {
                Console.WriteLine("Špatný počet hráčů, prosím zadejte číslo mezi 2-6...");
                tries++;
            }
        }
        if (tries == 2)
        {
            Console.WriteLine("Příliš mnoho pokusů. Program se ukončí.");
            Environment.Exit(0); // Ends the program
            return false;
        }
        // Adding players to the pool and setting starting positions
        for (int i = 0; i < playerCount; i++)
        {
            string nickname = string.Empty;
            tries = 0;
            while (tries < 2)
            {
                Console.WriteLine($"Zadejte prosím jméno hráče {i + 1}");
                nickname = Console.ReadLine()?.Trim();
                bool validPlayer = MainBoard.AddPlayer(nickname);
                if (validPlayer)
                {
                    //Console.WriteLine($"Hráč {nickname} úspěšně přidán...");
                    break;
                }
                else
                {
                    Console.WriteLine("Jméno hráče neplatné nebo již existuje, zadejte prosím nové jméno...");
                    tries++;
                }
            }
            if (tries == 2)
            {
                Console.WriteLine("Příliš mnoho pokusů. Program se ukončí.");
                Environment.Exit(0); // Ends the program
                return false;
            }
        }
        return true;
    }

    public bool DisplayCardAndCollectAnswer(Card[] cardPool, int cardIndex, ConsoleColor color, int height, int width)
    {
        // Display the card's question
        int maxIndex = cardPool.Count();
        if (cardIndex == maxIndex)
        {
            cardIndex = 0;
        }
        Card tmp = cardPool[cardIndex];
        tmp.DisplayCardQuestion(height, width, color);

        // Collect the answer
        Console.WriteLine("Prosím zvolte vaši odpověď...");
        string answer = Console.ReadLine()?.Trim().ToUpper();
        // Display the right answer
        tmp.DisplayRightAnswer(color);
        // Check if the answer is correct
        return tmp.RightAns == answer[0];
    }
    public void ExportStatsToFile(Board MainBoard, string winnerNickname)
    {
        // Get the current date and time, formatted as "yyyyMMdd_HHmmss"
        string dateTime = DateTime.Now.ToString("yyyyMMdd_HHmmss");

        // Specify the file path with the date and time in the name
        string filePath = $"BoardGameC#/gameStats/player_stats_{dateTime}.txt";

        // Create a StreamWriter to write to the file
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            // Write the winner to the file
            writer.WriteLine($"Vítězem je {winnerNickname}");
            writer.WriteLine();  // Add extra space after the winner's name

            // Loop through each player and write their stats to the file
            foreach (Player currentPlayer in MainBoard.PlayerPool)
            {
                currentPlayer.DisplayPlayerStatsToFile(writer);
            }
        }
        Console.WriteLine($"Stats have been exported to {filePath}.");
    }

    static void Main(string[] args)
    {
        Program main = new Program();
        // basic game  initialization
        Console.WriteLine("Vítejte! Probíhá příprava hry...");
        int boardHeight = 8;
        int boardWidth = 10;
        Board MainBoard = new Board(boardHeight, boardWidth);
        // read cards
        CardManager CardPool = new CardManager();
        // display board when ready
        Console.WriteLine("Vše připraveno!");
        MainBoard.DisplayBoard();
        Console.WriteLine("Žlutá kategorie představuje otázky z oblasti: Zahraniční jídla");
        Console.WriteLine("Červená kategorie představuje otázky z oblasti: Česká jídla");
        Console.WriteLine("Modrá kategorie představuje otázky z oblasti: Nápoje");
        Console.WriteLine("Zelená kategorie představuje otázky z oblasti: Zajímavosti");
        // possible rule explanation
        main.DisplayRules();
        // Player initialization
        int playerCount = 0;
        main.InitializePlayers(MainBoard, ref playerCount);
        Console.WriteLine();
        Console.WriteLine("Zadani hraci:");
        foreach (Player currentPlayer in MainBoard.PlayerPool)
        {
            Console.WriteLine($"{currentPlayer.Nickname}");
        }
        Console.WriteLine();
        // Starting the game
        Console.WriteLine("Hra může začít...");
        MainBoard.DisplayBoard();

        //samotna hra
        int gameIndex = 0;
        int redCardIndex = 0;
        int blueCardIndex = 0;
        int greenCardIndex = 0;
        int yellowCardIndex = 0;
        int winnerIndex = -1;
        while (true)
        {
            //moving palyer around
            Player currentPlayer = MainBoard.PlayerPool[gameIndex];
            Console.WriteLine($"Hráč na tahu: {currentPlayer.Nickname}");
            Console.WriteLine($"Červené karty: {currentPlayer.RedRight}, Modré karty {currentPlayer.BlueRight}, Zelené karty: {currentPlayer.GreenRight}, Žluté karty {currentPlayer.YellowRight}");
            MainBoard.MovePlayer(gameIndex);
            MainBoard.DisplayBoardPlayer(currentPlayer);
            // display card
            int currentRow = currentPlayer.Position.Item1;
            int currentColumn = currentPlayer.Position.Item2;
            var currentCellColor = MainBoard.BoardCells[currentRow, currentColumn].Color;
            bool isCorrect = false;

            if (currentCellColor == Colors.Blue)
            {
                if (main.DisplayCardAndCollectAnswer(CardPool.BlueCards, blueCardIndex, Cell.ColorMap[currentCellColor], boardHeight, boardWidth))
                {

                    currentPlayer.BlueCards++;
                    currentPlayer.BlueRight++;
                    currentPlayer.CurrentStreak++;
                    //currentPlayer.DisplayPlayer();
                    isCorrect = true;
                }
                else
                {

                    currentPlayer.BlueCards++;
                }
                blueCardIndex++;
            }
            else if (currentCellColor == Colors.Red)
            {
                if (main.DisplayCardAndCollectAnswer(CardPool.RedCards, redCardIndex, Cell.ColorMap[currentCellColor], boardHeight, boardWidth))
                {

                    currentPlayer.RedCards++;
                    currentPlayer.RedRight++;
                    currentPlayer.CurrentStreak++;
                    //currentPlayer.DisplayPlayer();
                    isCorrect = true;
                }
                else
                {

                    currentPlayer.RedCards++;
                }
                redCardIndex++;
            }
            else if (currentCellColor == Colors.Green)
            {
                if (main.DisplayCardAndCollectAnswer(CardPool.GreenCards, greenCardIndex, Cell.ColorMap[currentCellColor], boardHeight, boardWidth))
                {

                    currentPlayer.GreenCards++;
                    currentPlayer.GreenRight++;
                    currentPlayer.CurrentStreak++;
                    //currentPlayer.DisplayPlayer();
                    isCorrect = true;
                }
                else
                {

                    currentPlayer.GreenCards++;
                }
                greenCardIndex++;
            }
            else if (currentCellColor == Colors.Yellow)
            {
                if (main.DisplayCardAndCollectAnswer(CardPool.YellowCards, yellowCardIndex, Cell.ColorMap[currentCellColor], boardHeight, boardWidth))
                {

                    currentPlayer.YellowCards++;
                    currentPlayer.YellowRight++;
                    currentPlayer.CurrentStreak++;

                    isCorrect = true;
                }
                else
                {

                    currentPlayer.YellowCards++;
                }
                yellowCardIndex++;
            }
            // check if player didint win
            if (currentPlayer.BlueRight >= 4 && currentPlayer.RedRight >= 4 &&
                currentPlayer.YellowRight >= 4 && currentPlayer.GreenRight >= 4)
            {
                Console.WriteLine($"Hráč {currentPlayer.Nickname} vyhrál!");
                if (currentPlayer.CurrentStreak > currentPlayer.MaxStreak)
                {
                    currentPlayer.MaxStreak = currentPlayer.CurrentStreak;
                    currentPlayer.DisplayPlayer();
                }
                winnerIndex = gameIndex;
                break;
            }
            if (isCorrect)
            {
                isCorrect = false;
            }
            else
            {
                if (currentPlayer.CurrentStreak > currentPlayer.MaxStreak)
                {
                    currentPlayer.MaxStreak = currentPlayer.CurrentStreak;
                    currentPlayer.CurrentStreak = 0;
                }
                gameIndex++;
                if (gameIndex == playerCount)
                {
                    gameIndex = 0;
                }
                Console.WriteLine($"Další hráč na tahu {MainBoard.PlayerPool[gameIndex].Nickname}");
            }
        }

        // statisticke okenko
        Console.WriteLine($"Vítězem je {MainBoard.PlayerPool[winnerIndex].Nickname}");
        Console.WriteLine();
        foreach (Player currentPlayer in MainBoard.PlayerPool)
        {
            currentPlayer.DisplayPlayerStats();
        }
        main.ExportStatsToFile(MainBoard, MainBoard.PlayerPool[winnerIndex].Nickname);
    }
}
