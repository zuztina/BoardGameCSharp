using System.Data.Common;
using System.Diagnostics.Tracing;
using BoardGameC_.Models;

namespace BoardGameC_;

// TODO solve placing players on same cells
class Program
{
    public void DisplayRules()
    {
        int tries = 0;
        string? rules = string.Empty;  // Initialize rules

        while (tries < 3)
        {
            Console.WriteLine("Přejete si slyšet pravidla hry? [ano/ne]");
            rules = Console.ReadLine()?.Trim().ToLower();  // Handle possible extra spaces and make input lowercase

            if (rules == "ano")
            {
                // Add actual rules
                Console.WriteLine("Žlutá kategorie představuje otázky z oblasti: Zahraniční jídla");
                Console.WriteLine("Červená kategorie představuje otázky z oblasti: Česká jídla");
                Console.WriteLine("Modrá kategorie představuje otázky z oblasti: Nápoje");
                Console.WriteLine("Zelená kategorie představuje otázky z oblasti: Zajímavosti");
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

        if (tries == 3)
        {
            Console.WriteLine("Příliš mnoho pokusů. Program se ukončí.");
            Environment.Exit(0);  // End the program after three invalid attempts
        }
    }

    public bool InitializePlayers(Board MainBoard, ref int playerCount)
    {
        // Player count initialization with validation
        int tries = 0;
        while (tries < 3)
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

        if (tries == 3)
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
            while (tries < 3)
            {
                Console.WriteLine($"Zadejte prosím jméno hráče {i + 1}");
                nickname = Console.ReadLine()?.Trim();
                bool validPlayer = MainBoard.AddPlayer(nickname);

                if (validPlayer)
                {
                    Console.WriteLine($"Hráč {nickname} úspěšně přidán...");
                    break;
                }
                else
                {
                    Console.WriteLine("Jméno hráče neplatné nebo již existuje, zadejte prosím nové jméno...");
                    tries++;
                }
            }
            if (tries == 3)
            {
                Console.WriteLine("Příliš mnoho pokusů. Program se ukončí.");
                Environment.Exit(0); // Ends the program
                return false;
            }
        }

        return true;
    }
    static void Main(string[] args)
    {
        Program main = new Program();
        // basic game  initialization
        Console.WriteLine("Vítejte! Probíhá příprava hry...");
        Board MainBoard = new Board(8, 10);
        // adding colored gamecells to board
        MainBoard.AddYellowCells();
        MainBoard.AddGreenCells();
        MainBoard.AddRedCells();
        MainBoard.AddBlueCells();
        // display board when ready
        MainBoard.ShowGameCells();
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
        // Starting the game
        Console.WriteLine("Hra může začít...");
        MainBoard.DisplayBoard();

        for (int i = 0; i < playerCount; i++)
        {
            Console.WriteLine($"current player before: {MainBoard.PlayerPool[i].Nickname} ");
            MainBoard.DisplayBoardPlayer(MainBoard.PlayerPool[i]);
            MainBoard.MovePlayer(i);
            Console.WriteLine($"current player after move: {MainBoard.PlayerPool[i].Nickname} ");
            MainBoard.DisplayBoardPlayer(MainBoard.PlayerPool[i]);
        }
        MainBoard.DisplayBoard();
    }
}
