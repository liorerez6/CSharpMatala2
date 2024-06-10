﻿//using Ex02.ConsoleUtils;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Cryptography;
//using System.Text;

//class MemoryGameInputManager
//{

//    private const bool m_GotCorrectInputFromUser = true;
//    private const int m_MaxDimention = 6;
//    private const int m_MinDimention = 4;

//    private readonly ErrorHandling m_ErrorHandling = new ErrorHandling();
//    private MemoryGameLogic m_MemoryGameLogic = new MemoryGameLogic();
//    private bool m_UserInerfaceIsOn = true;

//    private Card m_FirstCard, m_SecondCard;
//    private int m_RowDimention, m_ColDimention;

//    public void PlayGame()
//    {
//        const bool v_PlayerWantsReGame = true;

//        PrintMessage("Welcome to memory game!");
//        SetupGame();

//        while (m_UserInerfaceIsOn)
//        {

//            PlayRounds();

//            if (askUserForReGame() == v_PlayerWantsReGame)
//            {
//                Screen.Clear();
//                m_MemoryGameLogic.reGameSetup();
//            }

//            else 
//            {
//                m_UserInerfaceIsOn = !v_PlayerWantsReGame;
//            }
//        }
//        Screen.Clear();
//    }


//    private void SetupGame()
//    {

//        GetGameMode();
//        Console.Clear();

//        GetPlayersNames();
//        Console.Clear();

//    }

//    private void PlayRounds()
//    {

//        GetBoardDimentions();

//        while (!m_MemoryGameLogic.GameIsOverStatus)
//        {
//            updateGameScreen();

//            if (m_MemoryGameLogic.GetPlayersTurn() == "Computer")
//            {
//                playComputerTurn();

//            }
//            else
//            {
//                playHumanTurn();
//            }

//            System.Threading.Thread.Sleep(2000);
//            Screen.Clear();
//        }




//    }

//    private void playComputerTurn()
//    {
//        bool cardsAreMatched;

//        //check if there is a liast with 2 cards --> true: reveald them
//        (Card card1, Card card2) = (m_MemoryGameLogic.CheckForMatchedCardsInMemoryList());

//        if(card1 == null)
//        {
//            card1 = m_MemoryGameLogic.GetRandomCardFromComputer();
//        }

//        m_MemoryGameLogic.FlipChosenCard(card1, true);

//        if (card2 == null)    // here are match
//        {
//            card2 = m_MemoryGameLogic.FindMatchingCard(card1);

//            if (card2 == null) // meaning here there is a match
//            {
//                card2 = m_MemoryGameLogic.GetRandomCardFromComputer();
//            }


//        }

//        m_MemoryGameLogic.FlipChosenCard(card2, true);
//        updateGameScreen();

//        cardsAreMatched = m_MemoryGameLogic.CheckForMatchAndUpdateAccordinly(card1, card2);

//        if (cardsAreMatched == false)
//        {
//            m_MemoryGameLogic.UpdateCardInComputerData(card1, card2);
//        }

//    }


//    private void playHumanTurn()
//    {
//        bool cardsAreMatched;
//        bool revealCard = true;

//        // first pick
//        (m_FirstCard) = GetVaildSlotFromUser(m_RowDimention, m_ColDimention);
//        m_MemoryGameLogic.FlipChosenCard(m_FirstCard, revealCard);
//        updateGameScreen();

//        // second pick
//        (m_SecondCard) = GetVaildSlotFromUser(m_RowDimention, m_ColDimention);
//        m_MemoryGameLogic.FlipChosenCard(m_SecondCard, revealCard);
//        updateGameScreen();

//         cardsAreMatched = m_MemoryGameLogic.CheckForMatchAndUpdateAccordinly(m_FirstCard, m_SecondCard);

//        if(m_MemoryGameLogic.IsComputerVsPlayerGame == true)
//        {
//            if(cardsAreMatched == false)
//            {
//                m_MemoryGameLogic.UpdateCardInComputerData(m_FirstCard, m_SecondCard);

//            }
//            else
//            {
//                m_MemoryGameLogic.DeleteCardsFromComputerData(m_FirstCard);
//            }
//        }

//    }

//    private void updateGameScreen()
//    {
//        string currentPlayerName;
//        int currentPlayerScore;

//        (currentPlayerName, currentPlayerScore) = m_MemoryGameLogic.GetCurrentPlayerInfo();

//        Screen.Clear();
//        Console.WriteLine($"{currentPlayerName}'s Turn");
//        Console.WriteLine($"Score: {currentPlayerScore}\n");
//        DisplayBoard(m_MemoryGameLogic.GetBoardState(), m_MemoryGameLogic.GetBoardReveals());
//    }

//    private Card GetVaildSlotFromUser(int i_RowDimention, int i_ColDimention)
//    {        
//        Card card = GetSlots(i_RowDimention, i_ColDimention); // UI checks that its withing dimentions of the board

//        while (!(m_MemoryGameLogic.IsValidEmptySlot(card))) // checks with the Logic if the slot is not taken already
//        {
//            m_ErrorHandling.InvalidTakenSlotError();
//            card = GetSlots(i_RowDimention, i_ColDimention);
//        }

//        return card;
//    }

//    private void DisplayBoard(char[,] boardState, bool[,] boardReveals)
//    {
//        StringBuilder sb = new StringBuilder();
//        sb.Append("   ");
//        char firstLetter = 'A';

//        for (int i = 0; i < boardState.GetLength(1); i++)
//        {
//            sb.Append(firstLetter);
//            sb.Append("   ");
//            firstLetter++;
//        }

//        sb.AppendLine();

//        for (int i = 0; i < boardState.GetLength(0); i++)
//        {
//            sb.Append(" ");
//            for (int k = 0; k <= boardState.GetLength(1) * 4; k++)
//            {
//                sb.Append("=");
//            }
//            sb.AppendLine();

//            sb.Append(i + 1);

//            for (int j = 0; j < boardState.GetLength(1); j++)
//            {
//                sb.Append("|");
//                if (boardReveals[i, j])
//                {
//                    sb.Append(" ");
//                    sb.Append(boardState[i, j]);
//                    sb.Append(" ");
//                }
//                else
//                {
//                    sb.Append("   ");
//                }
//            }
//            sb.Append("|");
//            sb.AppendLine();
//        }

//        sb.Append(" ");
//        for (int k = 0; k <= boardState.GetLength(1) * 4; k++)
//        {
//            sb.Append("=");
//        }

//        Console.WriteLine(sb.ToString());
//    }

//    private void GetGameMode() // true means TwoPlayerGame, false PlayerVsComputer
//    {

//        const string v_UserWantsPlayerVsPlayerGame = "1";
//        const string v_UserWantsPlayerVsComputerGame = "2";


//        DisplayGameModeOptions();

//        string userModeChoice = GetUserChoice(new string[] { v_UserWantsPlayerVsPlayerGame, v_UserWantsPlayerVsComputerGame });

//        bool gameMode = DetermineGameMode(userModeChoice);

//        m_MemoryGameLogic.GetGameModeFromUser(gameMode);
//    }

//    private void DisplayGameModeOptions()
//    {
//        PrintMessage("Enter 1 - Two players game");
//        PrintMessage("Enter 2 - Player vs Computer");
//    }

//    private bool DetermineGameMode(string userModeChoice)
//    {
//        const string m_TwoPlayersChoice = "1";

//        return userModeChoice.Equals(m_TwoPlayersChoice);
//    }

//    private bool askUserForReGame()
//    {
//        const string v_UserWantsReGame = "1";
//        const string v_UserDontWantReGame = "2";

//        PrintMessage("Do you want to play again?");
//        PrintMessage("Enter 1 - Yes");
//        PrintMessage("Enter 2 - No");

//        string userChoice = GetUserChoice(new string[] { v_UserWantsReGame, v_UserDontWantReGame });

//        return userChoice.Equals(v_UserWantsReGame);
//    }

//    private string GetUserChoice(string[] validChoices)
//    {
//        string userChoice;

//        do
//        {
//            userChoice = Console.ReadLine();
//            if (!IsValidChoice(userChoice, validChoices))
//            {
//                m_ErrorHandling.InvalidGameModeError();
//            }
//        } while (!IsValidChoice(userChoice, validChoices));

//        return userChoice;
//    }

//    private bool IsValidChoice(string choice, string[] validChoices)
//    {
//        return validChoices.Contains(choice);
//    }

//    private void GetPlayersNames()
//    {
//        List<Player> players = new List<Player>();

//        string firstPlayerName = null;
//        string secondPlayerName = null ;

//        firstPlayerName = GetPlayerName("Enter first player's name: ");

//        if (!m_MemoryGameLogic.IsComputerVsPlayerGame) // meaing playing agaist another player 
//        {
//            secondPlayerName = GetPlayerName("Enter second player's name: ");
//        }
//        else
//        {
//            secondPlayerName = "Computer";
//        }

//        m_MemoryGameLogic.AddPlayersToGame(firstPlayerName, secondPlayerName);

//    }

//    private string GetPlayerName(string i_Message)
//    {
//        string playerName;
//        do
//        {
//            Console.WriteLine(i_Message);
//            playerName = Console.ReadLine();
//            if (!IsValidName(playerName))
//            {
//                m_ErrorHandling.InvalidNameError();
//            }

//        } while (!IsValidName(playerName));

//        return playerName;
//    }

//    private bool IsValidName(string i_Name)
//    {
//        // Check if the i_Name contains only letters (A-Z and a-z)
//        // and is not empty or null
//        if (string.IsNullOrEmpty(i_Name))
//        {
//            return false;
//        }

//        foreach (char c in i_Name)
//        {
//            if (!char.IsLetter(c))
//            {
//                return false;
//            }
//        }

//        return true;
//    }

//    private void GetBoardDimentions()
//    {
//        int currentRowDimention;
//        int currentColDimention;
//        bool rowNumberIsOdd;


//        currentRowDimention = GetBoardRows();
//        rowNumberIsOdd = currentRowDimention % 2 != 0;
//        currentColDimention = GetBoardCols(rowNumberIsOdd);

//        while (!m_MemoryGameLogic.GetBoardDimentionsFromUser(currentRowDimention, currentColDimention))
//        {
//            m_ErrorHandling.InvalidOddColsError();

//            currentRowDimention = GetBoardRows();
//            rowNumberIsOdd = currentRowDimention % 2 != 0;
//            currentColDimention = GetBoardCols(rowNumberIsOdd);
//        }

//        m_RowDimention = currentRowDimention;
//        m_ColDimention = currentColDimention;
//    }

//    private int GetBoardRows()
//    {
//        while (m_GotCorrectInputFromUser)
//        {
//            Console.Write("Enter the number of rows (4 - 6): ");
//            string input = Console.ReadLine();

//            if (int.TryParse(input, out int rows))  // now only checks if int
//            {
//                if(rows >= m_MinDimention && rows <= m_MaxDimention)
//                {

//                    return rows;
//                }
//                else
//                {
//                    m_ErrorHandling.InvalidRowDimensionError();
//                }
//            }

//            else
//            {

//                m_ErrorHandling.InvalidRowDimensionError();
//            }
//        }

//    }

//    private int GetBoardCols(bool i_RowNumberIsOdd) // now only checks if int
//    {
//        while (m_GotCorrectInputFromUser)
//        {

//            Console.Write(i_RowNumberIsOdd ? "Enter the number of cols (4 or 6): " : "Enter the number of cols (4 - 6): ");

//            string input = Console.ReadLine();

//            if (int.TryParse(input, out int cols))
//            {
//                if (cols >= m_MinDimention && cols <= m_MaxDimention)
//                {

//                    return cols;
//                }
//                else
//                {
//                    m_ErrorHandling.InvalidColsDimensionError();
//                }
//            }

//            else
//            {

//                m_ErrorHandling.InvalidColsDimensionError();

//            }
//        }

//    }

//    private Card GetSlots(int i_CurrentRowDimention, int i_CurrentColDimention)
//    {
//        while(m_GotCorrectInputFromUser)
//        {
//            int rowSlot = GetSlotForRow(i_CurrentRowDimention);
//            int colSlot = GetSlotForCol(i_CurrentColDimention);
//            if(m_MemoryGameLogic.ChangeSlotWhithingDimentions(rowSlot, colSlot) == true)
//            {
//                return new Card(rowSlot, colSlot);
//            }
//            else
//            {
//                m_ErrorHandling.InvalidSlotOutOfDimention();
//            }
//        }
//    }  

//    private int GetSlotForRow(int i_CurrentRowDimention)
//    {
//        return GetSlot(i_CurrentRowDimention, "row");
//    }

//    private int GetSlotForCol(int i_CurrentColDimention)
//    {
//        return GetSlot(i_CurrentColDimention, "col");
//    }

//    private int GetSlot(int dimension, string slotType) 
//    {
//        while (m_GotCorrectInputFromUser)
//        {

//            if (slotType.Equals("row"))
//            {
//                Console.WriteLine($"Enter slot position (1-{dimension}) for {slotType}:");
//            }

//            else if (slotType.Equals("col"))
//            {
//                char maxColLetter = (char)('A' + dimension - 1);
//                Console.WriteLine($"Enter slot position (A-{maxColLetter}) for {slotType}:");
//            }

//            string input = Console.ReadLine();

//            if(input.Equals("Q"))
//            {
//                checkIfEndGameRequested(); // **exit the game // *edit* no need. just inform the big game loop that the game is finish. 
//            }
//            else
//            {
//                if (slotType.Equals("row") && int.TryParse(input, out int slot))  // this related to the logic checks..
//                {
//                    return slot;
//                }

//                else if (slotType.Equals("col") && IsValidColumnInput(input, dimension, out int colSlot))
//                {
//                    return colSlot;
//                }

//                else
//                {
//                    m_ErrorHandling.InvalidSlotError(dimension, slotType);
//                }
//            }

//        }
//    }

//    private bool IsValidColumnInput(string input, int dimension, out int colSlot)
//    {
//        bool validInput = false;
//        colSlot = -1;

//        if (input.Length == 1 && char.IsLetter(input[0]) && char.IsUpper(input[0]))
//        {
//            char columnChar = input[0];
//            colSlot = columnChar - 'A' + 1; // Convert 'A' to 1, 'B' to 2, etc.
//            validInput = true;

//        }
//        return validInput;
//    }

//    private void PrintMessage(string message)
//    {
//        Console.WriteLine(message);
//    }

//    private void checkIfEndGameRequested()
//    {
//        Console.WriteLine("Game ended by user.");
//        Environment.Exit(0);
//    }
//}


using Ex02.ConsoleUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

class MemoryGameInputManager
{

    private const bool m_GotCorrectInputFromUser = true;
    private const int m_MaxDimention = 6;
    private const int m_MinDimention = 4;
    private const string k_ExitGameKey = "Q";

    private readonly ErrorHandling m_ErrorHandling = new ErrorHandling();
    private MemoryGameLogic m_MemoryGameLogic = new MemoryGameLogic();
    private bool m_UserInerfaceIsOn = true;

    private Card m_FirstCard, m_SecondCard;
    private int m_RowDimention, m_ColDimention;

    public void PlayGame()
    {
        SetupGame();

        while (m_UserInerfaceIsOn)
        {
            PlayRounds();
            m_UserInerfaceIsOn = askUserForReGame();
        }

        Screen.Clear();
    }


    private void SetupGame()
    {
        Console.WriteLine("Welcome to memory game!");

        GetGameMode();
        Console.Clear();

        GetPlayersNames();
        Console.Clear();

    }

    private void PlayRounds()
    {
        Screen.Clear();
        GetBoardDimentions();

        while (!m_MemoryGameLogic.GameIsOverStatus)
        {
            updateGameScreen();

            if (m_MemoryGameLogic.GetPlayersTurn() == "Computer")
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


        m_UserInerfaceIsOn = false;


    }

    private void playComputerTurn()
    {
        bool cardsAreMatched;

        //check if there is a liast with 2 cards --> true: reveald them
        (Card card1, Card card2) = (m_MemoryGameLogic.CheckForMatchedCardsInMemoryList());

        if (card1 == null)
        {
            card1 = m_MemoryGameLogic.GetRandomCardFromComputer();
        }

        m_MemoryGameLogic.FlipChosenCard(card1, true);

        if (card2 == null)    // here are match
        {
            card2 = m_MemoryGameLogic.FindMatchingCard(card1);

            if (card2 == null) // meaning here there is a match
            {
                card2 = m_MemoryGameLogic.GetRandomCardFromComputer();
            }


        }

        m_MemoryGameLogic.FlipChosenCard(card2, true);
        updateGameScreen();

        cardsAreMatched = m_MemoryGameLogic.CheckForMatchAndUpdateAccordinly(card1, card2);

        if (cardsAreMatched == false)
        {
            m_MemoryGameLogic.UpdateCardInComputerData(card1, card2);
        }

    }


    private void playHumanTurn()
    {
        bool cardsAreMatched;
        bool revealCard = true;

        // first pick
        (m_FirstCard) = GetVaildSlotFromUser(m_RowDimention, m_ColDimention);
        m_MemoryGameLogic.FlipChosenCard(m_FirstCard, revealCard);
        updateGameScreen();

        // second pick
        (m_SecondCard) = GetVaildSlotFromUser(m_RowDimention, m_ColDimention);
        m_MemoryGameLogic.FlipChosenCard(m_SecondCard, revealCard);
        updateGameScreen();

        cardsAreMatched = m_MemoryGameLogic.CheckForMatchAndUpdateAccordinly(m_FirstCard, m_SecondCard);

        if (m_MemoryGameLogic.IsComputerVsPlayerGame == true)
        {
            if (cardsAreMatched == false)
            {
                m_MemoryGameLogic.UpdateCardInComputerData(m_FirstCard, m_SecondCard);

            }
            else
            {
                m_MemoryGameLogic.DeleteCardsFromComputerData(m_FirstCard);
            }
        }

    }

    private void updateGameScreen()
    {
        string currentPlayerName;
        int currentPlayerScore;

        (currentPlayerName, currentPlayerScore) = m_MemoryGameLogic.GetCurrentPlayerInfo();

        Screen.Clear();
        Console.WriteLine($"{currentPlayerName}'s Turn");
        Console.WriteLine($"Score: {currentPlayerScore}\n");
        DisplayBoard(m_MemoryGameLogic.GetBoardState(), m_MemoryGameLogic.GetBoardReveals());
    }

    private Card GetVaildSlotFromUser(int i_RowDimention, int i_ColDimention)
    {
        Card card = GetSlots(i_RowDimention, i_ColDimention); // UI checks that its withing dimentions of the board

        while (!(m_MemoryGameLogic.IsValidEmptySlot(card))) // checks with the Logic if the slot is not taken already
        {
            m_ErrorHandling.InvalidTakenSlotError();
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

    private void GetGameMode() // true means TwoPlayerGame, false PlayerVsComputer
    {

        const string v_UserWantsPlayerVsPlayerGame = "1";
        const string v_UserWantsPlayerVsComputerGame = "2";


        DisplayGameModeOptions();

        string userModeChoice = GetUserChoice(new string[] { v_UserWantsPlayerVsPlayerGame, v_UserWantsPlayerVsComputerGame });

        bool gameMode = DetermineGameMode(userModeChoice);

        m_MemoryGameLogic.GetGameModeFromUser(gameMode);
    }

    private void DisplayGameModeOptions()
    {

        Console.WriteLine("Enter 1 - Two players game");
        Console.WriteLine("Enter 2 - Player vs Computer");
    }

    private bool DetermineGameMode(string userModeChoice)
    {
        const string m_TwoPlayersChoice = "1";

        return userModeChoice.Equals(m_TwoPlayersChoice);
    }

    private bool askUserForReGame()
    {
        const string v_UserWantsReGame = "1";
        const string v_UserDontWantReGame = "2";
        bool reGameStatus;
        Console.WriteLine("Do you want to play again?");
        Console.WriteLine("Enter 1 - Yes");
        Console.WriteLine("Enter 2 - No");

        string userChoice = GetUserChoice(new string[] { v_UserWantsReGame, v_UserDontWantReGame });

        reGameStatus = userChoice.Equals(v_UserWantsReGame);

        if(reGameStatus)
        {
            m_MemoryGameLogic.reGameSetup();
        }

        return reGameStatus;
    }

    private string GetUserChoice(string[] validChoices)
    {
        string userChoice;

        do
        {
            userChoice = Console.ReadLine();
            if (!IsValidChoice(userChoice, validChoices))
            {
                m_ErrorHandling.InvalidGameModeError();
            }
        } while (!IsValidChoice(userChoice, validChoices));

        return userChoice;
    }

    private bool IsValidChoice(string choice, string[] validChoices)
    {
        return validChoices.Contains(choice);
    }

    private void GetPlayersNames()
    {
        //List<Player> players = new List<Player>();

        string firstPlayerName = null;
        string secondPlayerName = null;

        firstPlayerName = GetPlayerName("Enter first player's name: ");

        if (!m_MemoryGameLogic.IsComputerVsPlayerGame) // meaing playing agaist another player 
        {
            secondPlayerName = GetPlayerName("Enter second player's name: ");
        }
        else
        {
            secondPlayerName = "Computer";
        }

        m_MemoryGameLogic.AddPlayersToGame(firstPlayerName, secondPlayerName);

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
                m_ErrorHandling.InvalidNameError();
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

        // to do kfilot code
        currentRowDimention = GetBoardRows();
        rowNumberIsOdd = currentRowDimention % 2 != 0;
        currentColDimention = GetBoardCols(rowNumberIsOdd);

        while (!m_MemoryGameLogic.GetBoardDimentionsFromUser(currentRowDimention, currentColDimention))
        {
            m_ErrorHandling.InvalidOddColsError();

            currentRowDimention = GetBoardRows();
            rowNumberIsOdd = currentRowDimention % 2 != 0;
            currentColDimention = GetBoardCols(rowNumberIsOdd);
        }

        m_RowDimention = currentRowDimention;
        m_ColDimention = currentColDimention;
    }

    private int GetBoardRows()
    {
        while (m_GotCorrectInputFromUser)
        {
            Console.Write("Enter the number of rows (4 - 6): ");
            string input = Console.ReadLine();

            if (int.TryParse(input, out int rows))  // now only checks if int
            {
                if (rows >= m_MinDimention && rows <= m_MaxDimention)
                {

                    return rows;
                }
                else
                {
                    m_ErrorHandling.InvalidRowDimensionError();
                }
            }

            else
            {

                m_ErrorHandling.InvalidRowDimensionError();
            }
        }

    }

    private int GetBoardCols(bool i_RowNumberIsOdd) // now only checks if int
    {
        while (m_GotCorrectInputFromUser)
        {

            Console.Write(i_RowNumberIsOdd ? "Enter the number of cols (4 or 6): " : "Enter the number of cols (4 - 6): ");

            string input = Console.ReadLine();

            if (int.TryParse(input, out int cols))
            {
                if (cols >= m_MinDimention && cols <= m_MaxDimention)
                {

                    return cols;
                }
                else
                {
                    m_ErrorHandling.InvalidColsDimensionError();
                }
            }

            else
            {

                m_ErrorHandling.InvalidColsDimensionError();

            }
        }

    }

    private Card GetSlots(int i_CurrentRowDimention, int i_CurrentColDimention)
    {
        while (m_GotCorrectInputFromUser)
        {
            int rowSlot = GetSlotForRow(i_CurrentRowDimention);
            int colSlot = GetSlotForCol(i_CurrentColDimention);
            if (m_MemoryGameLogic.ChangeSlotWhithingDimentions(rowSlot, colSlot) == true)
            {
                return new Card(rowSlot, colSlot);
            }
            else
            {
                m_ErrorHandling.InvalidSlotOutOfDimention();
            }
        }
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
        //bool isExitGameKey = false;

        while (m_GotCorrectInputFromUser)
        {

            if (slotType.Equals("row"))
            {
                Console.WriteLine($"Enter slot position (1-{dimension}) for {slotType}:");
            }

            else if (slotType.Equals("col"))
            {
                char maxColLetter = (char)('A' + dimension - 1);
                Console.WriteLine($"Enter slot position (A-{maxColLetter}) for {slotType}:");
            }

            string input = Console.ReadLine();
            bool isExitGameKey = checkIfEndGameRequested(input);

            if (!isExitGameKey)
            {
                if (slotType.Equals("row") && int.TryParse(input, out int slot))  // this related to the logic checks..
                {
                    return slot;
                }

                else if (slotType.Equals("col") && IsValidColumnInput(input, dimension, out int colSlot))
                {
                    return colSlot;
                }

                else
                {
                    m_ErrorHandling.InvalidSlotError(dimension, slotType);
                }
            }
        }
    }

    private bool IsValidColumnInput(string input, int dimension, out int colSlot)
    {
        bool validInput = false;
        colSlot = -1;

        if (input.Length == 1 && char.IsLetter(input[0]) && char.IsUpper(input[0]))
        {
            char columnChar = input[0];
            colSlot = columnChar - 'A' + 1; // Convert 'A' to 1, 'B' to 2, etc.
            validInput = true;

        }
        return validInput;
    }

    private bool checkIfEndGameRequested(string i_Input)
    {
        bool isExitGameKey = i_Input.Equals(k_ExitGameKey);

        if (isExitGameKey)
        {
            Console.WriteLine("Game ended by user.");
            m_UserInerfaceIsOn = false;
            m_MemoryGameLogic.GameIsOverStatus = true;
        }

        return isExitGameKey;
        //Environment.Exit(0);
    }
}
