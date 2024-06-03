using Ex02.ConsoleUtils;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

class InputManager
{
    private const bool v_TwoPlayersGame = true;
    private const bool v_PlayerVsComputerGame = false;
    private const bool v_GotCorrectInputFromUser = true;
    private readonly ErrorHandling errorHandling = new ErrorHandling();
    private MemoryGameLogic m_memoryGameLogic = new MemoryGameLogic();
    private bool m_UserInerfaceIsOn = true;

    private int m_FirstPickRowSlot, m_FirstPickColSlot, m_SecondPickRowSlot, m_SecondPickColSlot, m_RowDimention, m_ColDimention;

    public void PlayGame()
    {

        while (m_UserInerfaceIsOn)
        {
            //Console.WriteLine("Welcome to memory game!\n");
            PrintMessage("Welcome to memory game!");
            SetupGame();
            PlayRounds();
        }

    }

    private void SetupGame()
    {
        bool gameMode;
        string firstPlayerName, secondPlayerName;
        int rowDimention, colDimention;

        gameMode = GetGameMode();
        m_memoryGameLogic.GetGameModeFromUser(gameMode);
        Console.Clear();

        (firstPlayerName, secondPlayerName) = GetPlayersNames();
        m_memoryGameLogic.AddPlayersToGame(firstPlayerName, secondPlayerName);
        Console.Clear();

        (rowDimention, colDimention) = GetBoardDimentions();
        while (!m_memoryGameLogic.GetBoardDimentionsFromUser(rowDimention, colDimention))
        {
            (rowDimention, colDimention) = GetBoardDimentions();
        }
        m_RowDimention = rowDimention;
        m_ColDimention = colDimention;
    }

    private void PlayRounds()
    {
        while (!m_memoryGameLogic.GameIsOver)
        {
            updateGameScreen();

            (m_FirstPickRowSlot, m_FirstPickColSlot) = GetVaildSlotFromUser(m_RowDimention, m_ColDimention);
            m_memoryGameLogic.FlipChosenCard(m_FirstPickRowSlot, m_FirstPickColSlot);
            updateGameScreen();

            (m_SecondPickRowSlot, m_SecondPickColSlot) = GetVaildSlotFromUser(m_RowDimention, m_ColDimention);
            m_memoryGameLogic.FlipChosenCard(m_SecondPickRowSlot, m_SecondPickColSlot);
            updateGameScreen();

            m_memoryGameLogic.CheckForMatchAndUpdateAccordinly(m_FirstPickRowSlot, m_FirstPickColSlot, m_SecondPickRowSlot, m_SecondPickColSlot);

            System.Threading.Thread.Sleep(2000);
            Screen.Clear();
        }
    }
    private void updateGameScreen()
    {
        string currentPlayerName;
        int currentPlayerScore;

        (currentPlayerName, currentPlayerScore) = m_memoryGameLogic.GetCurrentPlayerInfo();

        Screen.Clear();
        Console.WriteLine($"{currentPlayerName}'s Turn");
        Console.WriteLine($"Score: {currentPlayerScore}\n");
        DisplayBoard(m_memoryGameLogic.GetBoardState(), m_memoryGameLogic.GetBoardReveals());
    }
    private (int,int) GetVaildSlotFromUser(int i_RowDimention, int i_ColDimention)
    {

        int row, col;
        
        //DisplayBoard(i_BoardCurrentState, i_BoardCurrentReveals);


        (row, col) = GetSlots(i_RowDimention, i_ColDimention); // already checks the input according to game's dimentions.
        while (!(m_memoryGameLogic.IsValidEmptySlot(row, col)))
        {
            (row, col) = GetSlots(i_RowDimention, i_ColDimention);
        }


        return (row, col);

    }
    private void DisplayBoard(char[,] boardState, bool[,] boardReveals)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("   ");
        char firstLetter = 'A';

        for (int i = 0; i < boardState.GetLength(1); i++)
        {
            sb.Append(firstLetter);
            sb.Append("   ");
            firstLetter++;
        }

        sb.AppendLine();

        for (int i = 0; i < boardState.GetLength(0); i++)
        {
            sb.Append(" ");
            for (int k = 0; k <= boardState.GetLength(1) * 4; k++)
            {
                sb.Append("=");
            }
            sb.AppendLine();

            sb.Append(i + 1);

            for (int j = 0; j < boardState.GetLength(1); j++)
            {
                sb.Append("|");
                if (boardReveals[i, j])
                {
                    sb.Append(" ");
                    sb.Append(boardState[i, j]);
                    sb.Append(" ");
                }
                else
                {
                    sb.Append("   ");
                }
            }
            sb.Append("|");
            sb.AppendLine();
        }

        sb.Append(" ");
        for (int k = 0; k <= boardState.GetLength(1) * 4; k++)
        {
            sb.Append("=");
        }

        Console.WriteLine(sb.ToString());
    }

    public bool GetGameMode() // true means TwoPlayerGame, false PlayerVsComputer
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

    public (string, string) GetPlayersNames()
    {
        List<Player> players = new List<Player>();

        string firstPlayerName;
        string secondPlayerName;

        firstPlayerName = GetPlayerName("Enter first player's name: ");

        //players.Add(new Player(firstPlayerName));

        if (v_TwoPlayersGame) // meaing playing agaist another player 
        {
            secondPlayerName = GetPlayerName("Enter second player's name: ");
            //players.Add(new Player(secondPlayerName));
        }
        else
        {
            secondPlayerName = "Computer";
        }

        return (firstPlayerName, secondPlayerName);
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

    public (int,int) GetBoardDimentions()
    {
        int currentRowDimention;
        int currentColDimention;
        bool rowNumberIsOdd;


        currentRowDimention = GetBoardRows();

        rowNumberIsOdd = currentRowDimention % 2 != 0;

        currentColDimention = GetBoardCols(rowNumberIsOdd);

        return (currentRowDimention, currentColDimention);

    }

    private int GetBoardRows()
    {
        while (v_GotCorrectInputFromUser)
        {
            Console.Write("Enter the number of rows (4 - 6): ");
            string input = Console.ReadLine();

            if (int.TryParse(input, out int rows))  // now only checks if int
            {
                return rows;
            }

            //if (int.TryParse(input, out int rows) && (rows >= v_MinDimention && rows <= v_MaxDimention))
            //{
            //    return rows;
            //}

            else
            {
                errorHandling.InvalidRowDimensionError();
            }
        }

    }

    private int GetBoardCols(bool i_RowNumberIsOdd) // now only checks if int
    {
        while (v_GotCorrectInputFromUser)
        {

            Console.Write(i_RowNumberIsOdd ? "Enter the number of cols (4 or 6): " : "Enter the number of cols (4 - 6): ");

            string input = Console.ReadLine();

            if (int.TryParse(input, out int cols))
            {
                return cols;
            }

            else
            {

                errorHandling.InvalidColsDimensionError();

            }
        }

    }

    public (int, int) GetSlots(int i_CurrentRowDimention, int i_CurrentColDimention)
    {
        int rowSlot = GetSlotForRow(i_CurrentRowDimention);
        int colSlot = GetSlotForCol(i_CurrentColDimention);
        return (rowSlot, colSlot);
    }

    private int GetSlotForRow(int i_CurrentRowDimention)
    {
        return GetSlot(i_CurrentRowDimention, "row");
    }

    private int GetSlotForCol(int i_CurrentColDimention)
    {
        return GetSlot(i_CurrentColDimention, "col");
    }

    private int GetSlot(int dimension, string slotType) 
    {
        while (v_GotCorrectInputFromUser)
        {
            Console.WriteLine($"Enter slot position (1-{dimension}) for {slotType}:");
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



