using Ex02.ConsoleUtils;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

class InputManager
{
    //private const bool v_TwoPlayersGame = true;
    //private const bool v_PlayerVsComputerGame = false;
    private const bool v_GotCorrectInputFromUser = true;
    private const int v_MaxDimention = 6;
    private const int v_MinDimention = 4;

    private const string m_TwoPlayersChoice = "1";
    private const string m_PlayerVsComputerChoice = "2";

    private readonly ErrorHandling errorHandling = new ErrorHandling();
    private MemoryGameLogic m_memoryGameLogic = new MemoryGameLogic();
    private bool m_UserInerfaceIsOn = true;

    private Card m_FirstCard, m_SecondCard;
    private int m_RowDimention, m_ColDimention;
    
    public void PlayGame()
    {

        PrintMessage("Welcome to memory game!");
        SetupGame();

        while (m_UserInerfaceIsOn)
        {
            
            PlayRounds();
            // ask user for reGame

        }

    }
    
    private void SetupGame()
    {
        
        GetGameMode();
        Console.Clear();

        GetPlayersNames();
        Console.Clear();

        //GetBoardDimentions();
       
    }
    
    private void PlayRounds()
    {
        GetBoardDimentions();

        while (!m_memoryGameLogic.GameIsOver)
        {
            updateGameScreen();

            if (m_memoryGameLogic.GetPlayersTurn() == "Computer")
            {
                playComputerTurn();

            }
            else
            {
                playHumanTurn();
            }
            
            System.Threading.Thread.Sleep(2000);
            Screen.Clear();
        }

        
    }

    //private void playComputerTurn()
    //{
    //    bool cardsAreMatched;
    //    //check if there is a liast with 2 cards --> true: reveald them
    //    (Card card1, Card card2) = (m_memoryGameLogic.CheckForMatchedCardsInMemoryList());

    //    //if both are not null values
    //    if(card1 != card2)    // here are match
    //    {
    //        m_memoryGameLogic.FlipChosenCard(card1);
    //        updateGameScreen();
    //        m_memoryGameLogic.FlipChosenCard(card2);
    //        updateGameScreen();
    //        m_memoryGameLogic.CheckForMatchAndUpdateAccordinly(card1, card2);
    //    }

    //    else
    //    {
    //        card1 = m_memoryGameLogic.GetRandomCardFromComputer();
    //        m_memoryGameLogic.FlipChosenCard(card1);
    //        updateGameScreen();

    //        card2 = m_memoryGameLogic.FindMattchingCard(card1);

    //        if(card2 == null) // meaning here there is a match
    //        {

    //            card2 = m_memoryGameLogic.GetRandomCardFromComputer();
    //            //m_memoryGameLogic.FlipChosenCard(card2);
    //            //updateGameScreen();

    //            // m_memoryGameLogic.CheckForMatchAndUpdateAccordinly(card1, card2);
    //        }
    //        //else
    //        //{
                
                
    //        //    //m_memoryGameLogic.FlipChosenCard(card2);
    //        //    //updateGameScreen();
    //        //}

    //        m_memoryGameLogic.FlipChosenCard(card2);
    //        updateGameScreen();
    //        cardsAreMatched = m_memoryGameLogic.CheckForMatchAndUpdateAccordinly(card1, card2);

    //        if(cardsAreMatched == false)
    //        {
    //            m_memoryGameLogic.UpdateCardInComputerData(card1, card2);
    //        }

    //    }

    //}

    private void playComputerTurn()
    {
        bool cardsAreMatched;

        //check if there is a liast with 2 cards --> true: reveald them
        (Card card1, Card card2) = (m_memoryGameLogic.CheckForMatchedCardsInMemoryList());
        
        if(card1 == null)
        {
            card1 = m_memoryGameLogic.GetRandomCardFromComputer();
        }

        m_memoryGameLogic.FlipChosenCard(card1, true);

        //if both are not null values
        if (card2 == null)    // here are match
        {
            card2 = m_memoryGameLogic.FindMattchingCard(card1);

            if (card2 == null) // meaning here there is a match
            {
                card2 = m_memoryGameLogic.GetRandomCardFromComputer();
            }

            //cardsAreMatched = m_memoryGameLogic.CheckForMatchAndUpdateAccordinly(card1, card2);

            //if (cardsAreMatched == false)
            //{
            //    m_memoryGameLogic.UpdateCardInComputerData(card1, card2);
            //}

        }

        m_memoryGameLogic.FlipChosenCard(card2, true);
        updateGameScreen();

        cardsAreMatched = m_memoryGameLogic.CheckForMatchAndUpdateAccordinly(card1, card2);

        if (cardsAreMatched == false)
        {
            m_memoryGameLogic.UpdateCardInComputerData(card1, card2);
        }

    }

    

    private void playHumanTurn()
    {
        bool cardsAreMatched;
        bool revealCard = true;

        // first pick
        (m_FirstCard) = GetVaildSlotFromUser(m_RowDimention, m_ColDimention);
        m_memoryGameLogic.FlipChosenCard(m_FirstCard, revealCard);
        updateGameScreen();

        // second pick
        (m_SecondCard) = GetVaildSlotFromUser(m_RowDimention, m_ColDimention);
        m_memoryGameLogic.FlipChosenCard(m_SecondCard, revealCard);
        updateGameScreen();

         cardsAreMatched = m_memoryGameLogic.CheckForMatchAndUpdateAccordinly(m_FirstCard, m_SecondCard);

        if(m_memoryGameLogic.IsComputerVsPlayerGame == true)
        {
            if(cardsAreMatched == false)
            {
                m_memoryGameLogic.UpdateCardInComputerData(m_FirstCard, m_SecondCard);

            }
            else
            {
                m_memoryGameLogic.DeleteCardsFromComputerData(m_FirstCard);
            }
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
  
    private Card GetVaildSlotFromUser(int i_RowDimention, int i_ColDimention)
    {        
        Card card = GetSlots(i_RowDimention, i_ColDimention); // UI checks that its withing dimentions of the board
        
        while (!(m_memoryGameLogic.IsValidEmptySlot(card))) // checks with the Logic if the slot is not taken already
        {
            errorHandling.InvalidTakenSlotError();
            card = GetSlots(i_RowDimention, i_ColDimention);
        }

        return card;
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

    //private void GetGameMode() // true means TwoPlayerGame, false PlayerVsComputer
    //{

    //    PrintMessage("Enter 1 - Two players game");
    //    PrintMessage("Enter 2 - Player vs Computer");

    //    bool gameMode;
    //    string userModeChoice;

    //    do
    //    {
    //        userModeChoice = Console.ReadLine();
    //        if (!userModeChoice.Equals("1") && !userModeChoice.Equals("2"))
    //        {
    //            errorHandling.InvalidGameModeError();
    //        }

    //    } while (!userModeChoice.Equals("1") && !userModeChoice.Equals("2"));

    //    if(userModeChoice.Equals("1"))
    //    {
    //        gameMode = v_TwoPlayersGame;
    //    }
    //    else
    //    {
    //        gameMode = v_PlayerVsComputerGame;
    //    }

    //    m_memoryGameLogic.GetGameModeFromUser(gameMode);      
    //}


    private void GetGameMode() // true means TwoPlayerGame, false PlayerVsComputer
    {
        DisplayGameModeOptions();

        string userModeChoice = GetUserGameModeChoice();

        bool gameMode = DetermineGameMode(userModeChoice);

        m_memoryGameLogic.GetGameModeFromUser(gameMode);
    }

    private void DisplayGameModeOptions()
    {
        PrintMessage("Enter 1 - Two players game");
        PrintMessage("Enter 2 - Player vs Computer");
    }

    private string GetUserGameModeChoice()
    {
        string userModeChoice;

        do
        {
            userModeChoice = Console.ReadLine();
            if (!IsValidGameModeChoice(userModeChoice))
            {
                errorHandling.InvalidGameModeError();
            }
        } while (!IsValidGameModeChoice(userModeChoice));

        return userModeChoice;
    }

    private bool IsValidGameModeChoice(string choice)
    {
        return choice.Equals(m_TwoPlayersChoice) || choice.Equals(m_PlayerVsComputerChoice);
    }

    private bool DetermineGameMode(string userModeChoice)
    {
        return userModeChoice.Equals(m_TwoPlayersChoice);
    }


    private void GetPlayersNames()
    {
        List<Player> players = new List<Player>();

        string firstPlayerName = null;
        string secondPlayerName = null ;

        firstPlayerName = GetPlayerName("Enter first player's name: ");

        if (!m_memoryGameLogic.IsComputerVsPlayerGame) // meaing playing agaist another player 
        {
            secondPlayerName = GetPlayerName("Enter second player's name: ");
        }
        else
        {
            secondPlayerName = "Computer";
        }

        m_memoryGameLogic.AddPlayersToGame(firstPlayerName, secondPlayerName);

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
                errorHandling.InvalidNameError();
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

    private void GetBoardDimentions()
    {
        int currentRowDimention;
        int currentColDimention;
        bool rowNumberIsOdd;


        currentRowDimention = GetBoardRows();
        rowNumberIsOdd = currentRowDimention % 2 != 0;
        currentColDimention = GetBoardCols(rowNumberIsOdd);

        while (!m_memoryGameLogic.GetBoardDimentionsFromUser(currentRowDimention, currentColDimention))
        {
            errorHandling.InvalidOddColsError();

            currentRowDimention = GetBoardRows();
            rowNumberIsOdd = currentRowDimention % 2 != 0;
            currentColDimention = GetBoardCols(rowNumberIsOdd);
        }

        m_RowDimention = currentRowDimention;
        m_ColDimention = currentColDimention;
    }

    private int GetBoardRows()
    {
        while (v_GotCorrectInputFromUser)
        {
            Console.Write("Enter the number of rows (4 - 6): ");
            string input = Console.ReadLine();

            if (int.TryParse(input, out int rows))  // now only checks if int
            {
                if(rows >= v_MinDimention && rows <= v_MaxDimention)
                {
                    
                    return rows;
                }
                else
                {
                    errorHandling.InvalidRowDimensionError();
                }
            }

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
                if (cols >= v_MinDimention && cols <= v_MaxDimention)
                {
                    
                    return cols;
                }
                else
                {
                    errorHandling.InvalidColsDimensionError();
                }
            }

            else
            {

                errorHandling.InvalidColsDimensionError();

            }
        }

    }

    private Card GetSlots(int i_CurrentRowDimention, int i_CurrentColDimention)
    {
        int rowSlot = GetSlotForRow(i_CurrentRowDimention);
        int colSlot = GetSlotForCol(i_CurrentColDimention);

        return new Card(rowSlot, colSlot);
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

    private void PrintMessage(string message)
    {
        Console.WriteLine(message);
    }

    private void EndGameMessage()
    {
        Console.WriteLine("Game ended by user.");
        Environment.Exit(0);
    }
}



