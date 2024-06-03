using System;
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

    public void InvalidSlotError(int dimension, string slotType)
    {
        Console.WriteLine($"Error: Invalid number, slot {slotType} number should be (1-{dimension}).");
    }

    public void InvalidTakenSlotError()
    {
        Console.WriteLine("Error: Slot is already taken.");
    }
}