namespace BoardGameC_.Models {

    public class Player{

        public int currentRow; public int currentColumn;

        public required string Nickname;
        public int BlueRight;
        public int RedRight;
        public int GreenRight;
        public int YellowRight;

        public int BlueCards;
        public int RedCards;
        public int GreenCards;
        public int YellowCards;

        public int currentStreak;
        public int maxStreak;
        public Player(string Nick, int randomRow, int randomColumn){
            Nickname=Nick;
            currentRow = randomRow;
            currentColumn = randomColumn;

            BlueCards=0;
            BlueRight=0;

            RedCards=0;
            RedRight=0;

            GreenCards=0;
            GreenRight=0;

            YellowCards=0;
            YellowRight=0;

            currentStreak=0;
            maxStreak=0;
        }

    }
}