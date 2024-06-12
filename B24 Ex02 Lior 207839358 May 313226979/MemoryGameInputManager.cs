using Ex02.ConsoleUtils;
using System;
using System.Text;

class MemoryGameInputManager
{
    //Constants
    private const int k_MaxDimention = 6;
    private const int k_MinDimention = 4;
    private const int k_InvalidDimentionInt = -1;
    private const string k_ExitGameInput = "Q";

    //Other variables
    private bool m_IsComputerPlayerGameMode = false;
    private bool m_IsGameOver = false;
    private int m_MessagesDiffDisplay;              //for messages print
    private int m_RowDimention, m_ColDimention;

    //Objects
    private readonly ConsoleMessages m_MessagesForUser = new ConsoleMessages();     //maybe create a dll? instead of object
    private MemoryGameLogic m_MemoryGameLogic = new MemoryGameLogic();


    //###########################################################################################3
    
    //METHODS
    public void PlayGame()
    {
        getAndSetGameMode();

        while (!m_IsGameOver)
        {
            runGameRound();

            if (userRequestAnotherRound())
            {
                m_MemoryGameLogic.ResetGameSetup();
            }
        }
    }

    private void getAndSetGameMode()
    {
        const string v_TwoPlayerGameModeChoise = "1";
        const string v_CompuerPlayerGameModeChoise = "2";

        string userChoiseInput = null;
        bool isValidInput = false;

        while (isValidInput == false)
        {
            m_MessagesForUser.DisplayFirstGameMenuOptions();
            userChoiseInput = Console.ReadLine();

            switch (userChoiseInput)
            {
                case v_TwoPlayerGameModeChoise:
                    setGameModeForTwoHumanPlayers();
                    isValidInput = true;
                    break;
                case v_CompuerPlayerGameModeChoise:
                    setGameModeForHumanPlayerAndComputer();
                    isValidInput = true;
                    break;
                default:
                    m_MessagesForUser.InvalidGameModeMessage();
                    break;
            }
        }

        Console.Clear();
    }

    private void runGameRound()
    {
        //Board gameBoard = new Board();        maybe should be here, and each round create a new one

        getBoardDimentions();

        while (!m_IsGameOver)
        {
            updateGameScreen();

            Card fisrtCard = getCardInputFromPlayerAndDisplay();
            Card secondCard = getCardInputFromPlayerAndDisplay();
            System.Threading.Thread.Sleep(2000);

            (Card returnCard1, Card returnCard2) = m_MemoryGameLogic.PlayGameLogic(fisrtCard, secondCard);

            if (returnCard1 != null && returnCard2 != null)
            {
                updateGameScreen();
                displayRequestedSlot(returnCard1, Console.CursorLeft, Console.CursorTop);
                displayRequestedSlot(returnCard2, Console.CursorLeft, Console.CursorTop);
                System.Threading.Thread.Sleep(2000);
            }

            m_IsGameOver = m_MemoryGameLogic.GameStatus;
        }
    }

    private bool userRequestAnotherRound()
    {
        bool isValidInput = false;
        bool newRoundIsRequested = false;

        while (isValidInput == false)
        {
            m_MessagesForUser.DisplayAnotherRoundMenuOptions();
            string userChoice = Console.ReadLine();

            switch (userChoice)
            {
                case "1":
                    newRoundIsRequested = true;
                    break;
                case "2":
                    newRoundIsRequested = false;
                    m_IsGameOver = true;
                    break;
                default:
                    m_MessagesForUser.InvalidGameModeMessage();
                    break;
            }
        }

        return newRoundIsRequested;
    }

    private void clearConsoleLines(int i_Top, int i_Hight)
    {
        while(i_Hight > 0)
        {
            Console.SetCursorPosition(0, i_Top+1);
            Console.Write(new string(' ', Console.WindowWidth));
            i_Hight--;
        }
        Console.SetCursorPosition(0, i_Top);

    }

    //NEW - GET SLOT!
    private Card getCardInputFromPlayerAndDisplay()
    {
        Card card = null;
        int top = Console.CursorTop;

        while (card == null && !m_IsGameOver)
        {
            card = getCardInput();
            clearConsoleLines(top, 6);
        }

        if (card != null)
        {
            displayRequestedSlot(card, Console.CursorLeft, Console.CursorTop);
        }

        return card;
    }

    private Card getCardInput()
    {
        Card card = null;

        Console.WriteLine("Please choose a card to reveal");
        string cardInput = Console.ReadLine();

        //if input is exit key "Q" -return false, else -check if input has 2 chars only
        bool isValidInput = isExitGameInput(cardInput) ? false : isSlotInputValid(cardInput);

        //          false && true
        if (!m_IsGameOver && isValidInput)
        {
            int row = int.Parse(cardInput[1].ToString());
            int col = cardInput[0] - 'A' + 1;

            if (isValidSlotBoundries(row, col))
            {
                card = new Card(row, col);

                if (m_MemoryGameLogic.isCardAlreadyReveald(card))
                {
                    m_MessagesForUser.InvalidCardOutOfDimentionMessage();
                }
            }
        }

        return card;
    }

    private bool isValidSlotBoundries(int i_Row, int i_Col)
    {
        bool isValidInput = (i_Row <= m_RowDimention && i_Row >= 1) && (i_Col <= m_ColDimention && i_Col >= 1);
        
        if (!isValidInput)
        {
            m_MessagesForUser.InvalidCardOutOfDimentionMessage();
        }

        return isValidInput;
    }

    private bool isSlotInputValid(string i_Input)
    {
        bool isValidInput = i_Input.Length == 2;

        if (isValidInput)
        {
            //check if first char is a letter, and second is a letter
            isValidInput = char.IsLetter(i_Input[0]) && char.IsDigit(i_Input[1]);
        }

        if(!isValidInput)
        {
            m_MessagesForUser.InvalidCardForamtRequest();
        }

        return isValidInput;
    }

    ////#############################################

    private bool isExitGameInput(string i_InputRequest)
    {
        bool isExitGameInput = i_InputRequest.Equals(k_ExitGameInput);

        if (isExitGameInput)
        {
            m_MessagesForUser.ExitGameRequestMessage();
            m_IsGameOver = true;
            //Environment.Exit(0);
        }

        return isExitGameInput;
    }

    private void setGameModeForHumanPlayerAndComputer()
    {
        Console.Clear();

        string firstPlayerName = getPlayerName("first");

        m_IsComputerPlayerGameMode = true;
        m_MemoryGameLogic.IsComputerVsPlayerGame = true;
        m_MemoryGameLogic.AddPlayerToGame(firstPlayerName,"Computer", m_IsComputerPlayerGameMode);
    }

    private void setGameModeForTwoHumanPlayers()
    {
        Console.Clear();

        string firstPlayerName = getPlayerName("first");
        string secondPlayerName = getPlayerName("second");
        
        m_IsComputerPlayerGameMode = false;
        m_MemoryGameLogic.IsComputerVsPlayerGame = false;
        m_MemoryGameLogic.AddPlayerToGame(firstPlayerName, secondPlayerName, m_IsComputerPlayerGameMode);
    }

    private void displayBoard()
    {
        Screen.Clear();

        char[,] boardState = m_MemoryGameLogic.GetBoardState();
        bool[,] boardReveals = m_MemoryGameLogic.GetBoardReveals();
        char firstLetter = 'A';
        StringBuilder sb = new StringBuilder();

        sb.Append("   ");

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

    private void updateGameScreen()
    {
        (string currentPlayerName, int currentPlayerScore) = m_MemoryGameLogic.CurrentPlayerInfo;

        Screen.Clear();
        displayBoard();
        Console.WriteLine($"{currentPlayerName}'s Turn");
        Console.WriteLine($"Score: {currentPlayerScore}");
        Console.WriteLine("################################");
    }

    private void displayRequestedSlot(Card i_Card, int i_PreviousCursorLeft, int i_PreviousCursorTop)
    {
        const int v_ColSlotDiff = 4;
        const int v_RowSlotDiff = 2;

        char slotKey = m_MemoryGameLogic.GetCardKeyRequseted(i_Card);

        Console.SetCursorPosition((i_Card.Col * v_ColSlotDiff) - 1, i_Card.Row * v_RowSlotDiff);
        Console.Write(slotKey);
        Console.SetCursorPosition(i_PreviousCursorLeft, i_PreviousCursorTop);
    }

    private string getPlayerName(string i_Message)
    {
        string playerName = null;
        bool isValidPlayerName = false;

        while (isValidPlayerName == false)
        {
            Console.WriteLine($"Enter {i_Message} player's name: ");
            playerName = Console.ReadLine();
            isValidPlayerName = isNameInputValid(playerName);
        }

        return playerName;
    }

    private bool isNameInputValid(string i_Name)
    {
        // Check if the i_Name contains only letters (A-Z and a-z)
        // and is not empty or null
        bool isValidNameInput = false;

        if (string.IsNullOrEmpty(i_Name) == false)
        {
            //Temporary change before checking each char in i_Name
            isValidNameInput = true;

            foreach (char c in i_Name)
            {
                if (!char.IsLetter(c))
                {
                    isValidNameInput = false;
                    break;
                }
            }
        }

        if (isValidNameInput == false)
        {
            m_MessagesForUser.InvalidNameMessage();
        }

        return isValidNameInput;
    }

    //########################################################
    //GET BOARD
    private void getBoardDimentions()
    {
        bool isValidInputRequested = false;

        while (isValidInputRequested == false)
        {
            m_RowDimention = getBoardRows();
            m_ColDimention = getBoardColumns();

            isValidInputRequested = (m_RowDimention * m_ColDimention) % 2 == 0;

            if (!isValidInputRequested)
            {
                m_MessagesForUser.InvalidOddColsMessage();
            }
        }

        m_MemoryGameLogic.SetBoardDimentionsFromUser(m_RowDimention, m_ColDimention);
        m_MessagesDiffDisplay = m_ColDimention + 10;
    }

    private int getBoardRows()
    {
        int rows = k_InvalidDimentionInt;
        bool isValidInputRequested = false;

        while (isValidInputRequested == false)
        {
            rows = getDimentionInputNumber("rows");
            isValidInputRequested = isValidDimentionInputNumber(rows);

            if (isValidInputRequested == false)
            {
                m_MessagesForUser.InvalidRowDimensionMessage();
            }
            Screen.Clear();
        }

        return rows;
    }

    private int getBoardColumns()
    {
        int cols = k_InvalidDimentionInt;
        bool isValidInputRequested = false;

        while (isValidInputRequested == false)
        {
            cols = getDimentionInputNumber("rows");
            isValidInputRequested = isValidDimentionInputNumber(cols);

            if (isValidInputRequested == false)
            {
                m_MessagesForUser.InvalidColsDimensionMessage();
            }
            Screen.Clear();
        }

        return cols;
    }

    private bool isValidDimentionInputNumber(int i_DimentionNumber)
    {
        bool isValidIntInput = (i_DimentionNumber >= k_MinDimention && i_DimentionNumber <= k_MaxDimention);

        return isValidIntInput;
    }

    private int getDimentionInputNumber(string i_DimentionRequested)
    {
        Console.WriteLine($"Enter the number of {i_DimentionRequested} between [4 - 6]: ");

        string input = Console.ReadLine();
        int dimentionNumber = (int.TryParse(input, out dimentionNumber) == true) ? dimentionNumber : k_InvalidDimentionInt;

        return dimentionNumber;
    }
}
