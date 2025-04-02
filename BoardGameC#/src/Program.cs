using BoardGameC_.Models;

namespace BoardGameC_;

class Program
{
    static void Main(string[] args)
    {
    
        Board MainBoard = new Board(8,10);
        MainBoard.AddYellowCells();
        MainBoard.AddBlueCells();
        MainBoard.AddRedCells();
        MainBoard.AddGreenCells();
        MainBoard.DisplayBoard();
    }
}
