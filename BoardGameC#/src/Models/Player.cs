using Microsoft.VisualBasic;

namespace BoardGameC_.Models
{

    public class Player
    {

        public (int, int) Position;
        public int PositionIndex;
        public string Nickname { get; set; }
        public int BlueRight;
        public int RedRight;
        public int GreenRight;
        public int YellowRight;

        public int BlueCards;
        public int RedCards;
        public int GreenCards;
        public int YellowCards;

        public int CurrentStreak;
        public int MaxStreak;
        public Player(string nick, (int, int) randomPosition, int index)
        {

            Nickname = nick;
            Position = randomPosition;
            PositionIndex = index;

            BlueCards = 0;
            BlueRight = 0;

            RedCards = 0;
            RedRight = 0;

            GreenCards = 0;
            GreenRight = 0;

            YellowCards = 0;
            YellowRight = 0;

            CurrentStreak = 0;
            MaxStreak = 0;
        }

        public void DisplayPlayer()
        {
            // Display the player's nickname and position
            Console.WriteLine($"Nickname: {Nickname}");
            Console.WriteLine($"Position: ({Position}, {PositionIndex})");

            // Display the player's stats (cards and rights for each color category)
            Console.WriteLine("Cards and Rights:");
            Console.WriteLine($"  Blue Cards: {BlueCards}, Blue Rights: {BlueRight}");
            Console.WriteLine($"  Red Cards: {RedCards}, Red Rights: {RedRight}");
            Console.WriteLine($"  Green Cards: {GreenCards}, Green Rights: {GreenRight}");
            Console.WriteLine($"  Yellow Cards: {YellowCards}, Yellow Rights: {YellowRight}");

            // Display streak stats
            Console.WriteLine($"Current Streak: {CurrentStreak}");
            Console.WriteLine($"Max Streak: {MaxStreak}");
        }

        public void DisplayPlayerStats()
        {
            // Display the player's nickname and position
            Console.WriteLine($"Jméno hráče: {Nickname}");
            Console.WriteLine($"Nejdelší streak správných odpovědí: {MaxStreak}");
            // Display the player's stats (cards and rights for each color category)
            // Success percentage for each category
            PrintCategory("Modrá", BlueCards, BlueRight);
            PrintCategory("Červená", RedCards, RedRight);
            PrintCategory("Zelená", GreenCards, GreenRight);
            PrintCategory("Žlutá", YellowCards, YellowRight);
        }
         // update players position
        public void UpdatePosition((int, int) newPosition, int newIndex)
        {
            Position = newPosition;
            PositionIndex = newIndex;
        }
        // print the success percentage for a category
        public void PrintCategory(string color, int totalCards, int correctAnswers)
        {
            if (totalCards == 0)
            {
                Console.WriteLine($"{color}: žádné otázky v této kategorii.");
                return;
            }

            double successPercentage = (double)correctAnswers / totalCards * 100;
            Console.WriteLine($"{color} kategorie:");
            Console.WriteLine($"    Karty celkem: {totalCards}");
            Console.WriteLine($"    Správné odpovědi: {correctAnswers}");
            Console.WriteLine($"    úspěšnost: {successPercentage:F2}%");
            Console.WriteLine();
        }

        public void DisplayPlayerStatsToFile(StreamWriter writer)
        {
            // Write the player's nickname and position
            writer.WriteLine($"Jméno hráče: {Nickname}");
            writer.WriteLine($"Nejdelší streak správných odpovědí: {MaxStreak}");

            // Write the player's stats (cards and rights for each color category)
            // Success percentage for each category
            WriteCategoryToFile(writer, "Modrá", BlueCards, BlueRight);
            WriteCategoryToFile(writer, "Červená", RedCards, RedRight);
            WriteCategoryToFile(writer, "Zelená", GreenCards, GreenRight);
            WriteCategoryToFile(writer, "Žlutá", YellowCards, YellowRight);
        }

        public void WriteCategoryToFile(StreamWriter writer, string category, int totalCards, int correctAnswers)
        {
            if (totalCards == 0)
            {
                writer.WriteLine($"{category}: žádné otázky v této kategorii.");
                return;
            }

            double successPercentage = (double)correctAnswers / totalCards * 100;
            writer.WriteLine($"{category} kategorie:");
            writer.WriteLine($"    Karty celkem: {totalCards}");
            writer.WriteLine($"    Správné odpovědi: {correctAnswers}");
            writer.WriteLine($"    úspěšnost: {successPercentage:F2}%");
            writer.WriteLine();
        }
    }
}