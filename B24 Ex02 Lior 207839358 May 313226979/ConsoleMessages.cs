using Ex02.ConsoleUtils;
using System;
class ConsoleMessages
{
    //Display messages to user; options of choises/ requests input
    public void DisplayFirstGameMenuOptions()
    {
        Screen.Clear();
        Console.WriteLine("Welcome to Memory Game!");
        Console.WriteLine("Please choose the game mode:");
        Console.WriteLine("1. Two players - Enter 1");
        Console.WriteLine("2. Player vs Computer - Enter 2");
    }

    public void DisplayAnotherRoundMenuOptions()
    {
        Screen.Clear();
        Console.WriteLine("Do you want to play again?");
        Console.WriteLine("Enter 1 - Yes");
        Console.WriteLine("Enter 2 - No");
    }

    public void GetDimentionMessage(string i_DimentionRequested)
    {
        Console.WriteLine($"Enter the number of {i_DimentionRequested} between [4 - 6]: ");
    }

    public void ExitGameRequestMessage()
    {
        Console.WriteLine("Exit Game request confirmed. Goodbye.");
    }

    //Error messages for invalid input
    public void InvalidGameModeMessage()
    {
        Console.WriteLine("Error: Input should be only 1 or 2.");
    }

    public void InvalidNameMessage()
    {
        Console.WriteLine("Error: Name should contain only upper/lower-case letters (a-z, A-Z).");
    }

    public void InvalidCardForamtRequest()
    {
        Console.WriteLine("Error: Invalid card request.");
        Console.WriteLine("Should write in this specific order: one letter for column, and one number for row (e.g A1).");
    }

    //public void InvalidCardForamtDimention(int i_Row, char i_Col)
    //{
    //    Console.WriteLine("Error: Invalid card dimention request.");
    //    Console.WriteLine($"Should choose a row between 1-{i_Row}, and column between A-{i_Col}.");
    //}

    public void InvalidRowDimensionMessage()
    {
        Console.WriteLine("Error: Invalid number of rows.");
    }

    public void InvalidColsDimensionMessage()
    {
        Console.WriteLine("Error: Invalid number of cols.");
    }

    public void InvalidOddColsMessage()
    {
        Console.WriteLine("Error: Number of cols should be Even (4 or 6).");
    }

    public void InvalidCardOutOfDimentionMessage()
    {
        Console.WriteLine("Error: Invalid card choice, out of board's dimention.");
    }


    //DELETE maybe
    public void InvalidAlreadyRevealedCardMessage()
    {
        Console.WriteLine("Error: Card is already reveald, please choose another.");
    }

    public void InvalidCardFormatRequestMessage(int i_Dimension, string i_SlotType)
    {

        if (i_SlotType.Equals("row"))
        {
            Console.WriteLine($"Error: Invalid number, slot {i_SlotType} number should be (1-{i_Dimension}).");
        }
        else if (i_SlotType.Equals("col"))
        {
            char maxColLetter = (char)('A' + i_Dimension - 1);
            Console.WriteLine($"Error: Invalid letter, slot {i_SlotType} should be (A-{maxColLetter}).");
        }
    }

}