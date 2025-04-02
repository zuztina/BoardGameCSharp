using BoardGameC_.Models;

namespace BoardGameC_;

class Program
{
    static void Main(string[] args)
    {
        // emptyCell test
        EmptyCell testCell = new EmptyCell (0,0);
        testCell.PrintCell();
        testCell.Display();

        // colored cell manipulation
        Cell blueCell = new Cell(Colors.Blue);
        blueCell.Display();
        Console.WriteLine();
        blueCell.PrintCell();
        blueCell.ChangeOccupation();
        blueCell.PrintCell();
        blueCell.Display();
        Console.WriteLine();

        //edge cell
        EdgeCell Top = new EdgeCell(0,0);
        Top.PrintCell();
        EdgeCell Side = new EdgeCell(0,0);
        Side.PrintCell();
        Side.SetToSideEdge();
        Side.PrintCell();

        // cell row
        Cell redCell = new Cell(Colors.Red);
        Side.Display();
        redCell.Display();
        testCell.Display();
        Cell greenCell = new Cell(Colors.Green);
        greenCell.Display();
        testCell.Display();
        Cell yellowCell = new Cell(Colors.Yellow);
        yellowCell.Display();
        Top.Display();
        Top.Display();
    }
}
