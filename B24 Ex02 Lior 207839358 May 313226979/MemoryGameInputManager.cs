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

    //########################################################
    ////move to logic!!!!!!!
    //private void playHumanTurn()
    //{
    //    bool cardsAreMatched;

    //    // first pick
    //    //(m_FirstCard) = GetVaildSlotFromUser(m_RowDimention, m_ColDimention);
    //    m_MemoryGameLogic.FlipChosenSlot(m_FirstCard, k_RevealCard);
    //    updateGameScreen();

    //    // secondCard pick
    //    //(m_SecondCard) = GetVaildSlotFromUser(m_RowDimention, m_ColDimention);
    //    m_MemoryGameLogic.FlipChosenSlot(m_SecondCard, k_RevealCard);
    //    updateGameScreen();

    //    cardsAreMatched = m_MemoryGameLogic.checkAndUpdateIfSameCardKey(m_FirstCard, m_SecondCard);

    //    if (m_MemoryGameLogic.IsComputerVsPlayerGame == true)
    //    {
    //        if (cardsAreMatched == false)
    //        {
    //            m_MemoryGameLogic.saveCardsInComputerData(m_FirstCard, m_SecondCard);
    //        }
    //        else
    //        {
    //            m_MemoryGameLogic.deleteCardsFromComputerData(m_FirstCard);
    //        }
    //    }

    //}

    ////move to logic!
    //private void playComputerTurn()
    //{
    //    const bool v_RevealCard = true;
    //    bool cardsAreMatched;

    //    //check if there is a liast with 2 cards --> true: reveald them
    //    (Card card1, Card card2) = (m_MemoryGameLogic.checkForMatchedCardsInMemoryList());

    //    if (card1 == null)
    //    {
    //        card1 = m_MemoryGameLogic.getRandomCardForomputer();
    //    }

    //    m_MemoryGameLogic.FlipChosenSlot(card1, true);

    //    if (card2 == null)    // here are match
    //    {
    //        card2 = m_MemoryGameLogic.searchForSameKeyCardInComputerData(card1);

    //        if (card2 == null) // meaning here there is a match
    //        {
    //            card2 = m_MemoryGameLogic.getRandomCardForomputer();
    //        }


    //    }

    //    m_MemoryGameLogic.FlipChosenSlot(card2, v_RevealCard);
    //    updateGameScreen();

    //    cardsAreMatched = m_MemoryGameLogic.checkAndUpdateIfSameCardKey(card1, card2);

    //    if (cardsAreMatched == false)
    //    {
    //        m_MemoryGameLogic.saveCardsInComputerData(card1, card2);
    //    }

    //}


    ////private int GetSlot(int i_Dimention, string slotType)
    ////{
    ////    bool isValidInputRequested = false;
    ////    int rows = 0;

    ////    while (isValidInputRequested == false)
    ////    {
    ////        string i_Input = Console.ReadLine();

    ////        if (i_Input == k_ExitGameInput)
    ////        {
    ////            isExitGameInput(i_Input);
    ////            isValidInputRequested = true;
    ////            break;
    ////        }

    ////        isValidInputRequested = int.TryParse(i_Input, out rows);

    ////        if (isValidInputRequested == false)
    ////        {
    ////            m_MessagesForUser.InvalidCardFormatRequestMessage(dimension, slotType);

    ////        }

    ////        //if (slotType.Equals("row"))
    ////        //{
    ////        //    Console.WriteLine($"Enter card position (1-{i_Dimention}) for {slotType}:");
    ////        //}

    ////        //else if (slotType.Equals("colChar"))
    ////        //{
    ////        //    char maxColLetter = (char)('A' + dimension - 1);
    ////        //    Console.WriteLine($"Enter card position (A-{maxColLetter}) for {slotType}:");
    ////        //}

    ////        m_OLDisGameOver = isExitGameInput(i_Input); // **exit the game // *edit* no need. just inform the big game loop that the game is finish. 

    ////        if (!m_OLDisGameOver)
    ////        {
    ////            if (slotType.Equals("row") && int.TryParse(i_Input, out int card))  // this related to the logic checks..
    ////            {
    ////                return card;
    ////            }

    ////            else if (slotType.Equals("colChar") && isValidColumnInput(i_Input, out int io_ColSlot))
    ////            {
    ////                return io_ColSlot;
    ////            }

    ////            else
    ////            {
    ////                m_MessagesForUser.InvalidCardFormatRequestMessage(dimension, slotType);
    ////            }
    ////        }

    ////    }
    ////}


    ////CHECK INPUT VALIDATION
    //private bool isValidColumnInput(string i_Input, out int io_ColSlot)
    //{
    //    bool validInput = false;
    //    io_ColSlot = -1;

    //    if (i_Input.Length == 1 && char.IsLetter(i_Input[0]) && char.IsUpper(i_Input[0]))
    //    {
    //        char columnChar = i_Input[0];
    //        io_ColSlot = columnChar - 'A' + 1; // Convert 'A' to 1, 'B' to 2, etc.
    //        validInput = true;
    //    }
    //    else
    //    {
    //        m_MessagesForUser.InvalidCardFormatRequestMessage(m_ColDimention, i_Input);
    //    }

    //    return validInput;
    //}



//private Slot GetVaildSlotFromUser(int i_RowDimention, int i_ColDimention)
//{
//    Slot card = GetSlots(i_RowDimention, i_ColDimention); // UI checks that its withing dimentions of the board

//    //Sopposed to be in LOGIC!!!!!!!!!!!!!
//    while (!(m_MemoryGameLogic.isCardAlreadyReveald(card))) // checks with the Logic if the card is not taken already
//    {
//        m_MessagesForUser.InvalidAlreadyRevealedCardMessage();
//        card = GetSlots(i_RowDimention, i_ColDimention);
//    }

//    return card;
//}

//OK

//NOPE
//private bool checkChoiseValidation(string choice, string[] validChoices)
//{
//    bool isValidChoise = validChoices.Contains(choice);

//    if (isValidChoise == false)
//    {
//        m_MessagesForUser.InvalidGameModeMessage();
//    }

//    return isValidChoise;
//}


//Updated- needed only 1 return!!

//GET NAMES


///OLD
///
////TO FIX AND DELETE!!!!!!!!!!!!!!!!!
//private void GetGameMode() // true means TwoPlayerGame, false PlayerVsComputer
//{
//    const string v_UserWantsPlayerVsPlayerGame = "1";
//    const string v_UserWantsPlayerVsComputerGame = "2";

//    //Console.WriteLine("Welcome to memory game!");
//    //Console.WriteLine("Enter 1 - Two players game");
//    //Console.WriteLine("Enter 2 - Player vs Computer");

//    displayFirstGameMenuMessages();

//    string userModeChoice = GetUserChoice(new string[] { v_UserWantsPlayerVsPlayerGame, v_UserWantsPlayerVsComputerGame });

//    bool gameMode = DetermineGameMode(userModeChoice);

//    m_MemoryGameLogic.SetGameModeChoise(gameMode);
//}

//private string GetUserChoice(string[] validChoices)
//{
//    string userChoice = null;
//    bool isValidChoise = false;

//    while (isValidChoise == false)
//    {
//        userChoice = Console.ReadLine();
//        isValidChoise = checkChoiseValidation(userChoice, validChoices);
//    }

//    return userChoice;
//}

//private bool DetermineGameMode(string userModeChoice)
//{
//    const string m_TwoPlayersChoice = "1";

//    return userModeChoice.Equals(m_TwoPlayersChoice);
//}


//public MemoryGameInputManager()
//{
//    m_OLDisGameOver = true;

//}

//NEW
//public void NewPlayGame()
//{
//    getAndSetGameMode();
//    //initialize players - get names and set new objects

//}


//private void getPlayersNames()
//{
//    string firstPlayerName = getNameInputFromUser();
//    string secondPlayerName = ((m_TwoHumanPlayersMode == true) ? getNameInputFromUser() : "computer");

//    //m_MemoryGameLogic
//}

//private string getNameInputFromUser()
//{
//    bool v_IsValidName = false;
//    string name = null;

//    while (v_IsValidName == false)
//    {
//        name = Console.ReadLine();
//        v_IsValidName = isNameInputValid(name);

//        if (v_IsValidName == false)
//        {
//            m_MessagesForUser.InvalidNameMessage();
//        }
//    }

//    return name;
//}

//private void oneGameRound()
//{

//}

////NEW








////try new
//private Slot GetSlots(int i_CurrentRowDimention, int i_CurrentColDimention)
//{
//    Slot card = null;
//    bool isValidInputRequested = false;

//    while (isValidInputRequested)
//    {
//        int rowSlot = GetSlotForRow(i_CurrentRowDimention);
//        int colSlot = GetSlotForCol(i_CurrentColDimention);

//        if (m_MemoryGameLogic.ChangeSlotWhithingDimentions(rowSlot, colSlot) == true)
//        {
//            card = new Slot(rowSlot, colSlot);
//            isValidInputRequested = true;
//        }
//        else
//        {
//            m_MessagesForUser.InvalidCardOutOfDimentionMessage();
//        }
//    }

//    return card;
//}

//private int GetSlotForRow(int i_CurrentRowDimention)
//{
//    bool isValidInputRequested = false;
//    int rows = 0;

//    while (isValidInputRequested == false)
//    {
//        Console.WriteLine($"Enter card position (1-{0}) for {1}:", m_RowDimention, "row");

//        string getInput = Console.ReadLine();
//        isExitGameInput(getInput);
//        isValidInputRequested = int.TryParse(getInput, out rows);
//    }

//    return getRow;
//}

//private int GetSlotForCol(int i_CurrentColDimention)
//{
//    bool isValidInputRequested = false;
//    char maxColLetter = (char)('A' + m_ColDimention - 1);
//    int colChar = 0;

//    while (isValidInputRequested == false)
//    {
//        Console.WriteLine($"Enter card position (A-{0}) for {1}:", maxColLetter, "column");

//        string getInput = Console.ReadLine();
//        isExitGameInput(getInput);
//        isValidInputRequested = isValidColumnInput(getInput, out colChar);
//    }

//    return colChar;
//}


//private int GetBoardCols(bool i_RowNumberIsOdd) // now only checks if int
//{
//    while (k_IsValidInputFromUser)
//    {

//        Console.Write(i_RowNumberIsOdd ? "Enter the number of rows (4 or 6): " : "Enter the number of rows (4 - 6): ");

//        string input = Console.ReadLine();

//        if (int.TryParse(input, out int cols))
//        {
//            if (cols >= k_MinDimention && cols <= k_MaxDimention)
//            {

//                return cols;
//            }
//            else
//            {
//                m_MessagesForUser.InvalidColsDimensionMessage();
//            }
//        }

//        else
//        {

//            m_MessagesForUser.InvalidColsDimensionMessage();

//        }
//    }

//}


//private void getPlayersNames()
//{
//    List<Player> players = new List<Player>();

//    string firstPlayerName = null;
//    string secondPlayerName = null;

//    firstPlayerName = getPlayerName("Enter first player's name: ");

//    if (!m_MemoryGameLogic.IsComputerVsPlayerGame) // meaing playing agaist another player 
//    {
//        secondPlayerName = getPlayerName("Enter secondCard player's name: ");
//    }
//    else
//    {
//        secondPlayerName = "Computer";
//    }

//    m_MemoryGameLogic.AddPlayersToGame(firstPlayerName, secondPlayerName);

//}


//private void SetupGame()
//{
//    GetGameMode();
//    Console.Clear();

//    getPlayersNames();
//    Console.Clear();

//}