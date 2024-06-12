using Ex02.ConsoleUtils;
using System;
using System.Linq;
using System.Text;

class MemoryGameInputManager
{
    private const int m_MaxDimention = 6;
    private const int m_MinDimention = 4;
    private const string k_ExitGameKey = "Q";

    private readonly ErrorHandling m_ErrorHandling = new ErrorHandling();
    private MemoryGameLogic m_MemoryGameLogic = new MemoryGameLogic();

    private bool m_UserInerfaceIsOn = true;
    private bool m_IsExitGameKeyPressed = false;
    private int m_RowDimention, m_ColDimention;

    public void PlayGame()
    {
        setupGame();

        while (m_UserInerfaceIsOn)
        {
            playRounds();

            if (!m_IsExitGameKeyPressed)
            {
                m_UserInerfaceIsOn = askUserForReGame();
            }
        }

        Screen.Clear();
    }

    private void setupGame()
    {
        getGameMode();
        Console.Clear();

        getPlayersNames();
        Console.Clear();
    }

    private void playRounds()
    {
        Screen.Clear();
        getBoardDimentions();

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
        }
    }

    private void playComputerTurn()
    {

        bool cardsAreMatched;
        (Card card1, Card card2) = (m_MemoryGameLogic.CheckForMatchedCardsInComputerData());

        if (card1 == null)
        {
            card1 = m_MemoryGameLogic.GetRandomCardForComputer();
        }

        m_MemoryGameLogic.FlipChosenCard(card1, true);

        if (card2 == null) 
        {
            card2 = m_MemoryGameLogic.FindMatchingCard(card1);

            if (card2 == null)
            {
                card2 = m_MemoryGameLogic.GetRandomCardForComputer();
            }
        }

        m_MemoryGameLogic.FlipChosenCard(card2, true);
        updateGameScreen();
        cardsAreMatched = m_MemoryGameLogic.CheckForMatchAndUpdateAccordinly(card1, card2);

        if (cardsAreMatched == false)
        {
            m_MemoryGameLogic.UpdateCardInComputerData(cardsAreMatched, card1, card2);
        }
    }

    private void playHumanTurn()
    {
        Card firstCard = null;
        Card secondCard = null;

        firstCard = getVaildSlotFromUser();
        secondCard = getVaildSlotFromUser();

        if (!m_IsExitGameKeyPressed)
        {
            m_MemoryGameLogic.CheckForMatchAndUpdateAccordinly(firstCard, secondCard);
        }
    }

    private void updateGameScreen()
    {
        (string currentPlayerName, int currentPlayerScore) = m_MemoryGameLogic.GetCurrentPlayerInfo();

        Screen.Clear();
        Console.WriteLine($"{currentPlayerName}'s Turn");
        Console.WriteLine($"Score: {currentPlayerScore}\n");
        displayBoard(m_MemoryGameLogic.GetBoardState(), m_MemoryGameLogic.GetBoardReveals());
    }

    private Card getVaildSlotFromUser()
    {
        bool revealCard = true;
        Card card = getSlots(); 

        while (!m_IsExitGameKeyPressed && (!m_MemoryGameLogic.IsValidEmptySlot(card)))
        {
            m_ErrorHandling.InvalidTakenSlotError();
            card = getSlots();
        }

        if(card != null)
        {
            m_MemoryGameLogic.FlipChosenCard(card, revealCard);
            updateGameScreen();
        }

        return card;
    }

    private void displayBoard(char[,] boardState, bool[,] boardReveals)
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

    //TO DO
    private void getGameMode() // true means TwoPlayerGame, false PlayerVsComputer
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
        Console.WriteLine("Welcome to memory game!");
        Console.WriteLine("Enter 1 - Two players game");
        Console.WriteLine("Enter 2 - Player vs Computer");
    }

    private bool DetermineGameMode(string userModeChoice)
    {
        const string m_TwoPlayersChoice = "1";

        return userModeChoice.Equals(m_TwoPlayersChoice);
    }
    //###########################################

    //TO DO
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
            m_MemoryGameLogic.NewGameRoundSetup();
        }

        return reGameStatus;
    }

    private string GetUserChoice(string[] validChoices)
    {
        string userChoice;

        do
        {
            userChoice = Console.ReadLine();
            if (!isValidChoice(userChoice, validChoices))
            {
                m_ErrorHandling.InvalidGameModeError();
            }
        } while (!isValidChoice(userChoice, validChoices));

        return userChoice;
    }

    //###########################

    private bool isValidChoice(string choice, string[] validChoices)
    {
        return validChoices.Contains(choice);
    }

    private void getPlayersNames()
    {
        string firstPlayerName = null;
        string secondPlayerName = null;

        firstPlayerName = getPlayerName("Enter first player's name: ");

        if (!m_MemoryGameLogic.IsComputerVsPlayerGame) // meaing playing agaist another player 
        {
            secondPlayerName = getPlayerName("Enter second player's name: ");
        }
        else
        {
            secondPlayerName = "Computer";
        }

        m_MemoryGameLogic.AddPlayersToGame(firstPlayerName, secondPlayerName);
    }

    private string getPlayerName(string i_Message)
    {
        string playerName = null;
        bool isValidInput = false;

        while(isValidInput == false)
        {
            Console.WriteLine(i_Message);

            playerName = Console.ReadLine();
            isValidInput = isValidName(playerName);

            if (!isValidInput)
            {
                m_ErrorHandling.InvalidNameError();
            }
        }

        return playerName;
    }

    private bool isValidName(string i_Name)
    {
        bool isValidInput = !(string.IsNullOrEmpty(i_Name));

        if(isValidInput == true)
        {
            foreach (char c in i_Name)
            {
                if (!char.IsLetter(c))
                {
                    isValidInput = false;
                }
            }
        }

        return isValidInput;
    }

    private void getBoardDimentions()
    {
        bool isValidInput = false;

        while (isValidInput == false)
        {
            m_RowDimention = getBoardRows();
            bool rowNumberIsOdd = m_RowDimention % 2 != 0;
            m_ColDimention = getBoardCols(rowNumberIsOdd);

            isValidInput = m_MemoryGameLogic.GetBoardDimentionsFromUser(m_RowDimention, m_ColDimention);

            if (!isValidInput)
            {
                m_ErrorHandling.InvalidOddColsError();
            }
        }
    }

    private int getBoardRows()
    {
        bool isValidInput = false;
        int row = 0;

        while (isValidInput == false)
        {
            Console.Write("Enter the number of rows (4 - 6): ");
            string input = Console.ReadLine();

            isValidInput = int.TryParse(input, out row);

            if (isValidInput)  // now only checks if int
            {
                isValidInput = (row >= m_MinDimention && row <= m_MaxDimention);
            }
        }

        if (!isValidInput)
        {
            m_ErrorHandling.InvalidRowDimensionError();
        }

        return row;
    }

    private int getBoardCols(bool i_RowNumberIsOdd) // now only checks if int
    {
        bool isValidInput = false;
        int col = 0;

        while (isValidInput == false)
        {
            Console.Write(i_RowNumberIsOdd ? "Enter the number of cols (4 or 6): " : "Enter the number of cols (4 - 6): ");
            string input = Console.ReadLine();

            isValidInput = int.TryParse(input, out col);

            if (isValidInput)  // now only checks if int
            {
                isValidInput = (col >= m_MinDimention && col <= m_MaxDimention);
            }
        }

        if (!isValidInput)
        {
            m_ErrorHandling.InvalidColsDimensionError();
        }

        return col;
    }

    private Card getSlots()
    {
        Card card = null;

        while (card == null && !m_IsExitGameKeyPressed)
        {
            card = getCardInput();
        }

        return card;
    }

    private Card getCardInput()
    {
        Console.WriteLine("Please choose a card to reveal");

        string cardInput = Console.ReadLine();
        bool isValidInput = checkIfEndGameRequested(cardInput) ? false : isSlotInputValid(cardInput);
        Card card = null;

        if (!m_IsExitGameKeyPressed && isValidInput)
        {
            int row = int.Parse(cardInput[1].ToString());
            int col = cardInput[0] - 'A' + 1;

            if (isValidSlotBoundries(row, col))
            {
                card = new Card(row, col);
            }
        }

        return card;
    }

    private bool isValidSlotBoundries(int i_Row, int i_Col)
    {
        bool isValidInput = (i_Row <= m_RowDimention && i_Row >= 1) && (i_Col <= m_ColDimention && i_Col >= 1);

        if (!isValidInput)
        {
            m_ErrorHandling.InvalidSlotOutOfDimention();
        }

        return isValidInput;
    }

    private bool isSlotInputValid(string i_Input)
    {
        bool isValidInput = i_Input.Length == 2;

        if (isValidInput)
        {
            isValidInput = char.IsLetter(i_Input[0]) && char.IsDigit(i_Input[1]);
        }

        if (!isValidInput)
        {
            m_ErrorHandling.InvalidSlotError(m_RowDimention, m_ColDimention);
        }

        return isValidInput;
    }

    private bool checkIfEndGameRequested(string i_Input)
    {
        bool isExitGameKey = i_Input.Equals(k_ExitGameKey);

        if (isExitGameKey)
        {
            Console.WriteLine("Game ended by user.");
            m_UserInerfaceIsOn = false;
            m_IsExitGameKeyPressed = true;
            m_MemoryGameLogic.GameIsOverStatus = true;
        }

        return isExitGameKey;
    }

    //CHECK
    private void getAndSetGameMode()
    {
        const string v_TwoPlayerGameModeChoise = "1";
        const string v_CompuerPlayerGameModeChoise = "2";
        bool isValidInput = true;

        while (!isValidInput)
        {
            m_MessagesForUser.DisplayFirstGameMenuOptions();
            string userChoiseInput = Console.ReadLine();

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
                    m_MessagesForUser.InvalidGameModeError();
                    break;
            }
        }

        Console.Clear();
    }

    private void runGameRound()
    {
        //Board gameBoard = new Board();        maybe should be here, and each round create a new one

        getBoardDimentions();

        while (m_UserInerfaceIsOn)
        {
            updateGameScreen();

            Card fisrtCard = getCardInputFromPlayerAndDisplay();
            Card secondCard = getCardInputFromPlayerAndDisplay();
            System.Threading.Thread.Sleep(2000);

            (Card returnCard1, Card returnCard2) = m_MemoryGameLogic.PlayGameLogic(fisrtCard, secondCard);

            if (returnCard1 != null && returnCard2 != null)
            {
                updateGameScreen();
                displayRequestedCard(returnCard1, Console.CursorLeft, Console.CursorTop);
                displayRequestedCard(returnCard2, Console.CursorLeft, Console.CursorTop);
                System.Threading.Thread.Sleep(2000);
            }

            m_UserInerfaceIsOn = !m_MemoryGameLogic.IsGameIsOver;
        }
    }

    private bool userRequestAnotherRound()
    {
        string userChoice = null;
        bool newRoundIsRequested = false;

        while (userChoice == null)
        {
            m_MessagesForUser.DisplayAnotherRoundMenuOptions();
            userChoice = Console.ReadLine();

            switch (userChoice)
            {
                case "1":
                    newRoundIsRequested = true;
                    break;
                case "2":
                    newRoundIsRequested = false;
                    m_UserInerfaceIsOn = false;
                    break;
                default:
                    m_MessagesForUser.InvalidGameModeError();
                    userChoice = null;
                    break;
            }
        }

        return newRoundIsRequested;
    }

    private void setGameModeForHumanPlayerAndComputer()
    {
        string firstPlayerName = getPlayerName("first");

        m_MemoryGameLogic.InitializePlayers(firstPlayerName, "Computer", k_IsComputerVsPlayerGame);
    }

    private void setGameModeForTwoHumanPlayers()
    {
        string firstPlayerName = getPlayerName("first");
        string secondPlayerName = getPlayerName("second");

        m_MemoryGameLogic.InitializePlayers(firstPlayerName, secondPlayerName, !k_IsComputerVsPlayerGame);
    }

    private void displayRequestedCard(Card i_Card, int i_PreviousCursorLeft, int i_PreviousCursorTop)
    {
        const int v_ColSlotDiff = 4;
        const int v_RowSlotDiff = 2;

        char slotKey = m_MemoryGameLogic.GetCardKeyRequseted(i_Card);

        Console.SetCursorPosition((i_Card.Col * v_ColSlotDiff) - 1, i_Card.Row * v_RowSlotDiff);
        Console.Write(slotKey);
        Console.SetCursorPosition(i_PreviousCursorLeft, i_PreviousCursorTop);
}
