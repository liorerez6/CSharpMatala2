﻿using System;
class ErrorHandling
{
    public void InvalidGameModeError()
    {
        Console.WriteLine("Error: Input should be only 1 or 2.");
    }

    public void InvalidNameError()
    {
        Console.WriteLine("Error: Name should contain only a-Z.");
    }


    public void InvalidRowDimensionError()
    {
        Console.WriteLine("Error: Invalid number of rows.");
    }

    public void InvalidColsDimensionError()
    {
        Console.WriteLine("Error: Invalid number of cols.");
    }

    public void InvalidOddColsError()
    {
        Console.WriteLine("Error: Number of cols should be Even (4 or 6).");
    }

    public void InvalidSlotError(int i_Dimension, string i_SlotType)
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

    public void InvalidSlotOutOfDimention()
    {
        Console.WriteLine($"Error: Invalid slot choice, out of board's dimention.");

    }

    public void InvalidTakenSlotError()
    {
        Console.WriteLine("Error: Slot is already taken.");
    }
}