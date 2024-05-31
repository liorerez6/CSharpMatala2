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
        Console.WriteLine("Error: Invalid number of rows. Please enter 4 - 6.");
    }

    public void InvalidColsDimensionError()
    {
        Console.WriteLine("Error: Invalid number of cols. Please enter 4 - 6.");
    }

    public void InvalidOddColsError()
    {
        Console.WriteLine("Error: Number of cols should be Even (4 or 6).");
    }

    //public void InvaildSlotForRow(int i_CurrentRowDimention)
    //{
    //    Console.WriteLine("Error: Invaild number, slot row number should be (0-" + i_CurrentRowDimention + ").");
    //}

    //public void InvaildSlotForCol(int i_CurrentColDimention)
    //{
    //    Console.WriteLine("Error: Invaild number, slot col number should be (0-" + i_CurrentColDimention + ").");
    //}

    public void InvalidSlotError(int dimension, string slotType)
    {
        Console.WriteLine($"Error: Invalid number, slot {slotType} number should be (1-{dimension}).");
    }
}