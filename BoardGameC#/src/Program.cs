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

        while (tries < 2)
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
                    Console.WriteLine($"Hráč {nickname} úspěšně přidán...");
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
    static void Main(string[] args)
    {
        /*tmp.DisplayCardQuestion(8,10,ConsoleColor.Cyan);
        Console.WriteLine();
        tmp.DisplayCardAnswers(8,10,ConsoleColor.Cyan, false);
        Console.WriteLine();
        Console.WriteLine("Stiskněte mezerník pro zobrazeni spravne odpovedi...");
        //Wait for spacebar
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(intercept: true); // Do not show key in console
            } while (key.Key != ConsoleKey.Spacebar);
            Console.BackgroundColor = ConsoleColor.DarkGray;
        tmp.DisplayCardAnswers(8,10,ConsoleColor.Cyan, true);
        */
        Program main = new Program();
        // basic game  initialization
        Console.WriteLine("Vítejte! Probíhá příprava hry...");
        Board MainBoard = new Board(8, 10);
        Card tmp = new Card("This is a question that will be wrapped into multiple lines due to its length.", "A: totototototototot    toto je odpoved pro A", "B: toto je odpoved pro A", "C: toto je odpoved pro A", 'A');

        Console.WriteLine();
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
        // Starting the game
        Console.WriteLine("Hra může začít...");
        MainBoard.DisplayBoard();

        for (int i = 0; i < playerCount; i++)
        {
            Console.WriteLine($"current player before: {MainBoard.PlayerPool[i].Nickname} ");
            MainBoard.DisplayBoardPlayer(MainBoard.PlayerPool[i]);
            int tmpRow = MainBoard.PlayerPool[i].Position.Item1;
            int tmpColumn = MainBoard.PlayerPool[i].Position.Item2;
            Console.WriteLine($"current player row: {tmpRow}, column: {tmpColumn}");
            var tmpColor = MainBoard.BoardCells[tmpRow, tmpColumn].Color;
            Console.WriteLine($"current cell color: {tmpColor}");
            Console.WriteLine();
            int boardHeight = MainBoard.Height;
            int boardWidht = MainBoard.Width;
            tmp.DisplayCard(boardHeight, boardWidht, Cell.ColorMap[tmpColor]);
            //MainBoard.MovePlayer(i);
            //Console.WriteLine($"current player after move: {MainBoard.PlayerPool[i].Nickname} ");
            //MainBoard.DisplayBoardPlayer(MainBoard.PlayerPool[i]);
        }
        MainBoard.DisplayBoard();
    }
}
