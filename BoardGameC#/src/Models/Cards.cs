namespace BoardGameC_.Models
{

    class Card
    {

        public string Question { get; set; }
        public string AnswerA { get; set; }
        public string AnswerB { get; set; }
        public string AnswerC { get; set; }

        public char RightAns { get; set; }


        public Card(string initQuestion, string initA, string initB, string initC, char rightAns)
        {
            Question = initQuestion;
            AnswerA = initA;
            AnswerB = initB;
            AnswerC = initC;
            RightAns = rightAns;
        }

        public void DisplayCard()
        {

            int lineWidth = 29;
            int rows = 8;
            for (int i = 0; i < rows; i++)
            {
                Console.BackgroundColor = ConsoleColor.Cyan;
                // Print a row of spaces (29 characters wide)
                Console.WriteLine(new string(' ', lineWidth));
                Console.ResetColor();
            }

        }

        public void DisplayCardQuestion(int boardRows, int boardColumns)
        {
            int lineWidth = 3 * (boardColumns - 1) + 2;  // Total width for each line (including padding), each column 3 spaces, column 1 only 2 spaces
            int padding = 2;     // Padding on both sides of the question
            int contentWidth = lineWidth - 2 * padding;  // The content width, accounting for the padding
            int totalRows = boardRows;   // Total number of rows for displaying the question

            List<string> wrappedQuestion = WrapTextToLines(Question, contentWidth);  // Wrap the question to fit the lines
            int questionLines = wrappedQuestion.Count;  // Get the number of lines needed for the question
            int startRow = (totalRows - questionLines) / 2;  // Calculate starting row to center the question
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.ForegroundColor = ConsoleColor.Red;

            // Print blank lines before the question
            for (int i = 0; i < startRow; i++)
            {
                Console.WriteLine(new string(' ', lineWidth));  // Print the blank rows with padding
            }

            // Print the wrapped question
            foreach (var line in wrappedQuestion)
            {
                string paddedLine = line.PadLeft(padding + line.Length).PadRight(lineWidth);  // Add padding on both sides
                Console.WriteLine(paddedLine);  // Print the line with padding
            }

            // Print blank lines after the question
            for (int i = startRow + questionLines; i < totalRows; i++)
            {
                Console.WriteLine(new string(' ', lineWidth));  // Print the blank rows with padding
            }
            Console.ResetColor();
        }

        public void DisplayCardAnswers(int boardRows, int boardColumns)
        {
            int lineWidth = 3 * (boardColumns - 1) + 2;  // Total width for each line (including padding), each column 3 spaces, column 1 only 2 spaces
            int padding = 2;     // Padding on both sides of the question
            int contentWidth = lineWidth - 2 * padding;  // The content width, accounting for the padding
            int totalRows = boardRows;   // Total number of rows for displaying the question

            List<string> wrappedA = WrapTextToLines(AnswerA, contentWidth);
            List<string> wrappedB = WrapTextToLines(AnswerB, contentWidth);
            List<string> wrappedC = WrapTextToLines(AnswerC, contentWidth);
            Console.BackgroundColor = ConsoleColor.DarkGray;
            // Calculate the number of lines for the answers
            int lineNum = wrappedA.Count() + wrappedB.Count() + wrappedC.Count();
            int startRow = (totalRows - lineNum) / 2;  // Calculate starting row to center 

            // Print blank lines before the question
            for (int i = 0; i < startRow; i++)
            {
                Console.WriteLine(new string(' ', lineWidth));  // Print the blank rows with padding
            }
            // Print the wrapped question
            foreach (var line in wrappedA)
            {
                string paddedLine = line.PadLeft(padding + line.Length).PadRight(lineWidth);  // Add padding on both sides
                Console.WriteLine(paddedLine);  // Print the line with padding
            }
            // Print the wrapped question
            foreach (var line in wrappedB)
            {
                string paddedLine = line.PadLeft(padding + line.Length).PadRight(lineWidth);  // Add padding on both sides
                Console.WriteLine(paddedLine);  // Print the line with padding
            }
            // Print the wrapped question
            foreach (var line in wrappedC)
            {
                string paddedLine = line.PadLeft(padding + line.Length).PadRight(lineWidth);  // Add padding on both sides
                Console.WriteLine(paddedLine);  // Print the line with padding
            }

            // Print blank lines after the question
            for (int i = startRow + lineNum; i < totalRows; i++)
            {
                Console.WriteLine(new string(' ', lineWidth));  // Print the blank rows with padding
            }
            Console.ResetColor();
        }

        public void DisplayCardRight(int boardRows, int boardColumns)
        {
            int lineWidth = 3 * (boardColumns - 1) + 2;  // Total width for each line (including padding), each column 3 spaces, column 1 only 2 spaces
            int padding = 2;     // Padding on both sides of the question
            int contentWidth = lineWidth - 2 * padding;  // The content width, accounting for the padding
            int totalRows = boardRows;   // Total number of rows for displaying the question

            List<string> wrappedA = WrapTextToLines(AnswerA, contentWidth);
            List<string> wrappedB = WrapTextToLines(AnswerB, contentWidth);
            List<string> wrappedC = WrapTextToLines(AnswerC, contentWidth);
            Console.BackgroundColor = ConsoleColor.DarkGray;
            // Calculate the number of lines for the answers
            int lineNum = wrappedA.Count() + wrappedB.Count() + wrappedC.Count();
            int startRow = (totalRows - lineNum) / 2;  // Calculate starting row to center 

            // Print blank lines before the question
            for (int i = 0; i < startRow; i++)
            {
                Console.WriteLine(new string(' ', lineWidth));  // Print the blank rows with padding
            }
            // Print the wrapped question
            foreach (var line in wrappedA)
            {
                if (RightAns == 'A')
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    string paddedLine = line.PadLeft(padding + line.Length).PadRight(lineWidth);  // Add padding on both sides
                    Console.WriteLine(paddedLine);  // Print the line with padding
                    Console.ResetColor();
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                }
                else
                {
                    string paddedLine = line.PadLeft(padding + line.Length).PadRight(lineWidth);  // Add padding on both sides
                    Console.WriteLine(paddedLine);  // Print the line with padding
                }
            }
            // Print the wrapped question
            foreach (var line in wrappedB)
            {
                string paddedLine = line.PadLeft(padding + line.Length).PadRight(lineWidth);  // Add padding on both sides
                Console.WriteLine(paddedLine);  // Print the line with padding
            }
            // Print the wrapped question
            foreach (var line in wrappedC)
            {
                string paddedLine = line.PadLeft(padding + line.Length).PadRight(lineWidth);  // Add padding on both sides
                Console.WriteLine(paddedLine);  // Print the line with padding
            }

            // Print blank lines after the question
            for (int i = startRow + lineNum; i < totalRows; i++)
            {
                Console.WriteLine(new string(' ', lineWidth));  // Print the blank rows with padding
            }
            Console.ResetColor();
        }

        private List<string> WrapTextToLines(string text, int maxLineWidth)
        {
            List<string> wrappedLines = new List<string>();
            string[] words = text.Split(' ');  // Split the question into words
            string currentLine = "";

            foreach (var word in words)
            {
                // If adding this word exceeds the max line width, start a new line
                if (currentLine.Length + word.Length + (currentLine.Length > 0 ? 1 : 0) > maxLineWidth)
                {
                    wrappedLines.Add(currentLine);  // Add the current line to the list
                    currentLine = word;  // Start a new line with the current word
                }
                else
                {
                    // Add the word to the current line
                    if (currentLine.Length > 0)
                        currentLine += " ";  // Add a space between words
                    currentLine += word;
                }
            }

            // Add the last line if there's any remaining text
            if (!string.IsNullOrEmpty(currentLine))
                wrappedLines.Add(currentLine);

            return wrappedLines;
        }
    }
}