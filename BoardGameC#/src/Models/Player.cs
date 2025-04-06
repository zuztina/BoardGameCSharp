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

        public void UpdatePosition((int, int) newPosition, int newIndex)
        {
            Position = newPosition;
            PositionIndex = newIndex;
        }

    }
}