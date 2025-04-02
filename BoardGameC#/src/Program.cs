using BoardGameC_.Models;

namespace BoardGameC_;

class Program
{
    static void Main(string[] args)
    {
        Cell blueCell = new Cell(Colors.Blue);
        blueCell.Display();
        blueCell.PrintCell();
        blueCell.ChangeOccupation();
        blueCell.PrintCell();
        blueCell.Display();
        Cell redCell = new Cell(Colors.Red);
        redCell.Display();
        Cell greenCell = new Cell(Colors.Green);
        greenCell.Display();
        Cell yellowCell = new Cell(Colors.Yellow);
        yellowCell.Display();
        yellowCell.PrintCell();
    }
}
