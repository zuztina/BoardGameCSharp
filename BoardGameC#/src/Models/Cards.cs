using System.Drawing;
using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;


namespace BoardGameC_.Models
{
    public class CardManager
    {
        public Card[]? RedCards { get; private set; }
        public Card[]? YellowCards { get; private set; }
        public Card[]? BlueCards { get; private set; }
        public Card[]? GreenCards { get; private set; }

        public CardManager()
        {
            RedCards = LoadCardsFromJson("/home/tinka/C#Projekt/BoardGameC#/src/Cards/RedCards.json");
            YellowCards = LoadCardsFromJson("/home/tinka/C#Projekt/BoardGameC#/src/Cards/YellowCards.json");
            BlueCards = LoadCardsFromJson("/home/tinka/C#Projekt/BoardGameC#/src/Cards/BlueCards.json");
            GreenCards = LoadCardsFromJson("/home/tinka/C#Projekt/BoardGameC#/src/Cards/GreenCards.json");
        }
        public Card[] LoadCardsFromJson(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"File not found: {filePath}");
                return Array.Empty<Card>();
            }

            try
            {
                // Read the JSON data from the file
                string jsonString = File.ReadAllText(filePath);

                // Deserialize the JSON into a dynamic object (without additional classes)
                var jsonDoc = JsonSerializer.Deserialize<JsonElement>(jsonString);

                if (jsonDoc.ValueKind != JsonValueKind.Object || !jsonDoc.TryGetProperty("cards", out var cardsArray))
                {
                    Console.WriteLine("Invalid JSON format.");
                    return Array.Empty<Card>();
                }

                // Create a list to hold the Card objects
                List<Card> cards = new List<Card>();

                // Iterate through the cards in the JSON array and map the data to Card objects
                foreach (var cardElement in cardsArray.EnumerateArray())
                {
                    string question = cardElement.GetProperty("question").GetString();
                    string answerA = cardElement.GetProperty("answers").GetProperty("A").GetString();
                    string answerB = cardElement.GetProperty("answers").GetProperty("B").GetString();
                    string answerC = cardElement.GetProperty("answers").GetProperty("C").GetString();
                    char rightAns = cardElement.GetProperty("rightAnswer").GetString()[0];  // Assuming the right answer is a single letter

                    // Create the Card object and add it to the list
                    Card card = new Card(question, answerA, answerB, answerC, rightAns);
                    cards.Add(card);
                }

                // Return the array of Card objects
                Shuffle(cards);
                return cards.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading cards: {ex.Message}");
                return Array.Empty<Card>();
            }
        }
        public void PrintAllCards(Card[] Category, ConsoleColor consoleColor, int width, int height)
        {

            foreach (Card card in Category)
            {
                Console.ResetColor();
                card.DisplayCardQuestion(height, width, consoleColor);
                Console.ResetColor();
                card.DisplayAnswers(consoleColor);
            }
        }

        // https://stackoverflow.com/questions/273313/randomize-a-listt
        public static void Shuffle<T>(List<T> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1); 
                T temp = list[k];
                list[k] = list[n];
                list[n] = temp;
            }
        } 
    }
    public class Card
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


        // display solid color matrix as a back of the card
        public void DisplayCard(int boardRows, int boardColumns, ConsoleColor color)
        {
            int lineWidth = 3 * (boardColumns - 1) + 2;  // Total width for each line (including padding), each column 3 spaces, 1. column  only 2 spaces;
            int rows = boardRows;
            for (int i = 0; i < rows; i++)
            {
                Console.BackgroundColor = color;
                // Print a row of spaces (29 characters wide - for 8x10 matrix)
                Console.Write(new string(' ', lineWidth));
                Console.ResetColor();
                Console.WriteLine(); // Finish the line with no color
            }
        }

        // Function to display the question on the card with specified background color
        public void DisplayCardQuestion(int boardRows, int boardColumns, ConsoleColor color)
        {
            int lineWidth = 3 * (boardColumns - 1) + 2;  // Total width for each line (including padding)
            int padding = 2;
            int contentWidth = lineWidth - 2 * padding;  // line(text) width considering padding
            int totalRows = boardRows;

            // prepare the question lines and a starting row
            List<string> wrappedQuestion = WrapTextToLines(Question, contentWidth);
            int questionLines = wrappedQuestion.Count;
            int startRow = (totalRows - questionLines) / 2;

            // Display card back first
            Console.WriteLine("Pro zobrazení otázky a odpovědí stikněte menezerník...");
            DisplayCard(boardRows, boardColumns, color);
            Console.WriteLine();  // New line after the card background
            // Wait for spacebar
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(intercept: true); // Do not show key in console
            } while (key.Key != ConsoleKey.Spacebar);

            // Print blank lines before the question
            PrintBlankLines(startRow, lineWidth);
            // Print question
            PrintWrappedText(wrappedQuestion, padding, lineWidth, ConsoleColor.DarkGray);
            // Print blank lines after the question
            PrintBlankLines(totalRows - (startRow + questionLines), lineWidth);

            // Reset the console colors after printing
            Console.ResetColor();
            Console.WriteLine();
            DisplayAnswers(color);
        }

        // print a specific number of blank lines with the specified line width
        private void PrintBlankLines(int startRow, int lineWidth)
        {
            for (int i = 0; i < startRow; i++)
            {
                Console.BackgroundColor = ConsoleColor.DarkGray;
                Console.Write(new string(' ', lineWidth));  // Print an empty line with the background color
                Console.ResetColor();
                Console.WriteLine(); // Finish the line with no color
            }
        }

        // Function to print the wrapped question text on the card with proper padding and background color
        private void PrintWrappedText(List<string> wrappedText, int padding, int lineWidth, ConsoleColor bgColor)
        {
            foreach (var line in wrappedText)
            {
                Console.BackgroundColor = bgColor;
                string paddedLine = line.PadLeft(padding + line.Length).PadRight(lineWidth);
                Console.Write(paddedLine);  // Print the line with padding and background color
                Console.ResetColor();
                Console.WriteLine(); // Finish the line with no color
            }
        }

        // Function to wrap the question text to fit the available width
        public List<string> WrapTextToLines(string text, int maxLineWidth)
        {
            // list of individual lines of text
            List<string> wrappedLines = new List<string>();
            // split the text to words
            string[] words = text.Split(' ');
            string currentLine = "";

            foreach (var word in words)
            {
                // adding this word exceeds the max line width, start a new line
                if (currentLine.Length + word.Length + (currentLine.Length > 0 ? 1 : 0) > maxLineWidth)
                {
                    wrappedLines.Add(currentLine);  // Add current line to the list
                    currentLine = word;  // Start a new line with current word
                }
                else
                {
                    // Add the word to the current line
                    if (currentLine.Length > 0)
                        currentLine += " ";  // Add a space between words
                    currentLine += word;
                }
            }
            // Add the last line 
            if (!string.IsNullOrEmpty(currentLine))
                wrappedLines.Add(currentLine);

            return wrappedLines;
        }

        public void DisplayAnswers(ConsoleColor color)
        {
            // print the options
            //a)
            Console.ForegroundColor = color;
            Console.Write("a)");
            Console.ResetColor();
            Console.WriteLine($" {AnswerA}");
            //b)
            Console.ForegroundColor = color;
            Console.Write("b)");
            Console.ResetColor();
            Console.WriteLine($" {AnswerB}");
            //c)
            Console.ForegroundColor = color;
            Console.Write("c)");
            Console.ResetColor();
            Console.WriteLine($" {AnswerC}");
        }

        public void DisplayRightAnswer(ConsoleColor color)
        {
            if (RightAns == 'A')
            {
                Console.Write($"Správná odpověď je za ");
                Console.ForegroundColor = color;
                Console.Write("a)");
                Console.WriteLine($" {AnswerA}");
                Console.ResetColor();
            }
            if (RightAns == 'B')
            {
                Console.ForegroundColor = color;
                Console.Write("b)");
                Console.WriteLine($" {AnswerB}");
                Console.ResetColor();
            }
            if (RightAns == 'C')
            {
                Console.ForegroundColor = color;
                Console.Write("c)");
                Console.WriteLine($" {AnswerC}");
                Console.ResetColor();
            }
        }
    }
}