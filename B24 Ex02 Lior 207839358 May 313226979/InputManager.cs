using System;
using System.Collections.Generic;



/*
 * needs here a lot of changes.
inputManager should only check for vaild input but he doesnt responsible for knowing the dimentions of the board
for example:

inputManager should get input of (5,5) checks that it indead 2 integers.
then inputManger asks the Logic if this input a vaild input. (in our case its Board Class)
Board Class will response that its invaild input because its odd input(not even). then InputManger will print the right message.
 * 
 */

class InputManager
{
    private const bool v_TwoPlayersGame = true;
    private const bool v_PlayerVsComputerGame = false;
    private const bool v_GotCorrectInputFromUser = true;
    private readonly ErrorHandling errorHandling = new ErrorHandling();
    private const int v_MinDimention = 4;
    private const int v_MaxDimention = 6;
    private int m_currentRowDimention;
    private int m_currentColDimention;

    public bool GetGameMode()
    {

        Console.WriteLine("Enter 1 - Two players game");
        Console.WriteLine("Enter 2 - Player vs Computer");

        string userModeChoice;

        do
        {
            userModeChoice = Console.ReadLine();
            if (!userModeChoice.Equals("1") && !userModeChoice.Equals("2"))
            {
                Console.WriteLine("Input should be only 1 or 2");
            }

        } while (!userModeChoice.Equals("1") && !userModeChoice.Equals("2"));

        return userModeChoice == "1" ? v_TwoPlayersGame : v_PlayerVsComputerGame;

    }

    public List<Player> GetPlayersNames()
    {
        List<Player> players = new List<Player>();

        string firstPlayerName = GetPlayerName("Enter first player's name: ");
        players.Add(new Player(firstPlayerName));
        
        if (GetGameMode() == v_TwoPlayersGame) // meaing playing agaist another player 
        {
            string secondPlayerName = GetPlayerName("Enter second player's name: ");
            players.Add(new Player(secondPlayerName));
        }
        else
        {
            players.Add(new Player("Computer"));
        }

        return players;
    }

    private string GetPlayerName(string i_Message)
    {
        string playerName;
        do
        {
            Console.WriteLine(i_Message);
            playerName = Console.ReadLine();
            if (!IsValidName(playerName))
            {
                Console.WriteLine("Name should contain only a-Z");
            }

        } while (!IsValidName(playerName));

        return playerName;
    }

    private bool IsValidName(string i_Name)
    {
        // Check if the i_Name contains only letters (A-Z and a-z)
        // and is not empty or null
        if (string.IsNullOrEmpty(i_Name))
        {
            return false;
        }

        foreach (char c in i_Name)
        {
            if (!char.IsLetter(c))
            {
                return false;
            }
        }

        return true;
    }

    public Board GetBoardDimentions()
    {

        m_currentRowDimention = GetBoardRows();

        bool rowNumberIsOdd = m_currentRowDimention % 2 != 0;

        m_currentColDimention = GetBoardCols(rowNumberIsOdd);

        return new Board(m_currentRowDimention, m_currentColDimention);

    }

    private int GetBoardRows()
    {
        while (v_GotCorrectInputFromUser)
        {
            Console.Write("Enter the number of rows (4 - 6): ");
            string input = Console.ReadLine();

            if (int.TryParse(input, out int rows) && (rows >= v_MinDimention && rows <= v_MaxDimention))
            {
                return rows;
            }
            else
            {
                errorHandling.InvalidRowDimensionError();
            }
        }

    }

    private int GetBoardCols(bool i_RowNumberIsOdd)
    {
        while (v_GotCorrectInputFromUser)
        {

            Console.Write(i_RowNumberIsOdd ? "Enter the number of cols (4 or 6): " : "Enter the number of cols (4 - 6): ");

            string input = Console.ReadLine();

            if (int.TryParse(input, out int cols) && (cols >= v_MinDimention && cols <= v_MaxDimention))
            {
                if((cols % 2 == 1) && (i_RowNumberIsOdd == true))
                {
                    errorHandling.InvalidOddColsError();
                }
                else
                {
                    return cols;
                }
                
            }
            else
            {

                errorHandling.InvalidColsDimensionError();

            }
        }

    }

    public (int, int) GetSlots()
    {
        int rowSlot = GetSlotForRow();
        int colSlot = GetSlotForCol();
        return (rowSlot, colSlot);
    }

    private int GetSlotForRow()
    {
        return GetSlot(m_currentRowDimention, "row");
    }

    private int GetSlotForCol()
    {
        return GetSlot(m_currentColDimention, "col");
    }

    private int GetSlot(int dimension, string slotType) 
    {
        while (v_GotCorrectInputFromUser)
        {
            Console.WriteLine($"Enter slot position (1-{dimension}): ");
            string input = Console.ReadLine();

            if(input.Equals("Q"))
            {
                EndGameMessage(); // **exit the game // *edit* no need. just inform the big game loop that the game is finish. 
            }
            else
            {
                if (int.TryParse(input, out int slot) && (slot >= 1 && slot <= dimension))  // this related to the logic checks..
                {
                    return slot;
                }
                else
                {
                    errorHandling.InvalidSlotError(dimension, slotType);
                }
            }
            
        }
    }

    public void PrintMessage(string message)
    {
        Console.WriteLine(message);
    }

    private void EndGameMessage()
    {
        Console.WriteLine("Game ended by user.");
        Environment.Exit(0);
    }
}



