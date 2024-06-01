using System;
using System.Text;


public class RunGame
{
    private Player m_Player1 = null;
    private Player m_Player2 = null;
    private Board m_Board = null;

    private bool m_IsGameOver = false;

    public RunGame()
    {
        DisplayMenu();

        if(m_IsGameOver == false)
        {
            InitilizeGameBoard();
            StartGame();
        }

        //TO DO
        DisplaysEndGameMessage();
        //clear screen
        //exit
    }

    private void DisplaysEndGameMessage()
    {
        //TO DO
        //winner annoncement
    }

    //Get Board preference
    private void InitilizeGameBoard()
    {
        int row = 0;
        int col = 0;
        bool isValidInput = false;

        while(isValidInput == false)
        {
            System.Console.WriteLine("Please choose the board size");
            System.Console.WriteLine("Enter size of coloumns: ");
            col = System.Console.Read();
            //TO DO
            //check input
            //isValidInput = method - should also send an error message
            isValidInput = true;    //temporary

        }

        isValidInput = false;

        while (isValidInput == false)
        {
            System.Console.WriteLine("Enter size of rows: ");
            row = System.Console.Read();
            //TO DO
            //check input
            //isValidInput = method - should also send an error message
            isValidInput = true;    //temporary
        }

        m_Board = new Board(row, col);
    }

    private void PrintMenu()
    {
        //Maybe a text file instead of method
        //clear screen

        System.Console.WriteLine("Welcome to Memory Game");
        System.Console.WriteLine("Please press the number of your choise to start a game:");
        System.Console.WriteLine("1. 2 Players");
        System.Console.WriteLine("2. 1 Player vs Computer");
        System.Console.WriteLine("3. Exit");
    }

    private void DisplayMenu()
    {
        bool isValidInput = false;

        PrintMenu();

        while (isValidInput == false)
        {
            int press = System.Console.Read();

            //TO DO
            //check input isValidInput = method 
            //temporary set->
            isValidInput = true;

            switch (press)
            {
                case 1:
                    m_Player1 = new Player();
                    m_Player2 = new Player();
                    break;

                case 2:
                    m_Player1 = new Player();
                    m_Player2 = new Player("Computer");
                    break;

                case 3:
                    m_IsGameOver = true;
                    break;
            }
        }

        //TO DO
        //clear screen
    }

    private void StartGame()
    {
        while(m_IsGameOver ==  false)
        {
            bool sameCardsRevealed = false;

            //TO DO
            //clear screen
            m_Board.DisplayBoard();
            
            //Player 1 cards choise:
            sameCardsRevealed = GetCardeFromPlayerAndCheckIfSimilar();
            if (sameCardsRevealed == true)
            {
                m_Player1.Score += 1;
            }

            //Player 2 cards choise:
            sameCardsRevealed = GetCardeFromPlayerAndCheckIfSimilar();
            if (sameCardsRevealed == true)
            {
                m_Player1.Score += 1;
            }

            //TO DO
            //need to add delay time for each reveal 
            CheckIfGameIsOver();
        }

        //TO DO
        //clear screen
        //Display winner
    }

    //Maybe move to Card class
    private bool GetCardeFromPlayerAndCheckIfSimilar()
    {
        Card firstCard = null;
        Card secondCard = null;
        bool sameCardsAreRevealed = false;

        //TO DO
        //clear screen
        m_Board.DisplayBoard();

        //Player's cards choises:
        firstCard = GetOneCardFromPlayer();
        secondCard = GetOneCardFromPlayer();

        sameCardsAreRevealed = m_Board.CheckIfSameCards(firstCard, secondCard);
        
        return sameCardsAreRevealed;
    }

    //Maybe move to Card class
    private Card GetOneCardFromPlayer()
    {
        Card card = new Card();
        bool isValidInput = false;
        bool isCardChoiseValid = false;

        while (isValidInput == false && isCardChoiseValid == false)
        {
            System.Console.WriteLine("Please choose a card by writing the numbers of it's row and column: ");

            card.Row = System.Console.Read();
            card.Col = System.Console.Read();
            //TO DO
            //check and convert input from player
            //should be a letter and number

            //isValidInput = 
            isCardChoiseValid = m_Board.CheckACardOnBoard(card.Row, card.Col);

            //check input, check if 'Q' for endGame, char??
            if (card.Row == 'Q' || card.Col == 'Q')
            {
                m_IsGameOver = true;
                break;
            }

            //TO DO
            //if false: clear -> error message -> clear -> display board again
            //else if {}

        }

        //TO DO
        //isValidInput == true
        m_Board.RevealCard(card.Row, card.Col);

        return card;
    }

    private void CheckIfGameIsOver()
    {
        if (m_Board.IsBoardFull() == true)
        {
            m_IsGameOver = true;
        }
    }
}

