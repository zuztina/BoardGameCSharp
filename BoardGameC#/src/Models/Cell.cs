using System.Data;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace BoardGameC_.Models

{

    public enum Colors { Yellow, Blue, Red, Green, None }

    public class Cell{
    
        public Colors Color {get; private set;} // color of the cell from enum Colors
        public bool IsOccupied {get; private set;} // flag to determin if cell is currently occupised by a player
        public int Row {get; private set;} // number of row in Board matrix
        public int Column {get; private set;} // number of column in Board matrix


        public Cell(Colors initColor= Colors.Red, int initRow=0, int initColumn=0){
            Color = initColor;
            Row = initRow;
            Column = initColumn;
            IsOccupied = false;
        }

        public virtual void Display(){
            Console.ForegroundColor = ColorMap[Color];
            Console.BackgroundColor = ConsoleColor.DarkGray;
            if (!IsOccupied){
                Console.Write("⬤ ");
            }
            else{
                Console.Write("◯");
            }
            Console.ResetColor();
        }

        public virtual void PrintCell(){
            Console.WriteLine($"Cell color: {Color}");
            Console.WriteLine($"Cell row: {Row}");
            Console.WriteLine($"Cell column: {Column}");
            Console.WriteLine($"Cell state: {IsOccupied}");
        }

        public void ChangeOccupation(){
            IsOccupied=!IsOccupied;
        }

        private static readonly Dictionary<Colors, ConsoleColor> ColorMap = new() {
            { Colors.Yellow, ConsoleColor.Yellow },
            { Colors.Blue, ConsoleColor.Cyan },
            { Colors.Red, ConsoleColor.DarkRed },
            { Colors.Green, ConsoleColor.Green }
        }; 
    }

    public class EmptyCell : Cell {
        
        public EmptyCell(int Row, int Column) : base(Colors.None, Row, Column) {}

        public override void Display(){
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.Write("  ");
            Console.ResetColor();
        }

        public override void PrintCell()
        {
            Console.WriteLine($"Empty cell at row {Row}, column {Column}");
        }
        
    }

}