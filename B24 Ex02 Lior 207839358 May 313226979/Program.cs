using System;
using Ex02.ConsoleUtils;

class Program
{

    public static void Main()
    {

        InputManager inputManger = new InputManager();
        
        Board board = inputManger.GetBoardDimentions();
        Screen.Clear();
        board.DisplayBoard();
        (int row, int col) = inputManger.GetSlots();
        board.ReveldCard(row, col);



        Screen.Clear();
        





    }

}



