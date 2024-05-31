using System;
using System.Text;


public class RunGame
{
    private Player m_player1 = null;
    private Player m_player2 = null;
    private Board m_board = null;

    private bool m_isGameOver = false;

    public RunGame()
    {
        DisplayMenu();

        if(m_isGameOver == false)
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
            //isValidInput = 
        }

        while (isValidInput == false)
        {
            System.Console.WriteLine("Enter size of rows: ");
            row = System.Console.Read();
            //TO DO
            //check input
            //isValidInput = 
        }

        m_board = new Board(row, col);
    }

    private void PrintMenu()
    {
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
            //check input isValidInput = 

            switch (press)
            {
                case 1:
                    m_player1 = new Player();
                    m_player2 = new Player();
                    break;

                case 2:
                    m_player1 = new Player();
                    m_player2 = new Player("Computer");
                    break;

                case 3:
                    m_isGameOver = true;
                    break;
            }
        }

        //TO DO
        //clear screen
    }

    private void StartGame()
    {
        while(m_isGameOver ==  false)
        {
            bool sameCardsRevealed = false;

            //clear screen
            m_board.DisplayBoard();
            
            //Player 1 cards choise:
            sameCardsRevealed = GetCardeFromPlayerAndCheckIfSimilar();
            if (sameCardsRevealed == true)
            {
                m_player1.Score += 1;
            }

            //Player 2 cards choise:
            sameCardsRevealed = GetCardeFromPlayerAndCheckIfSimilar();
            if (sameCardsRevealed == true)
            {
                m_player1.Score += 1;
            }

            //TO DO
            //need to add delay time for each reveal 
            CheckIfGameIsOver();
        }

        //TO DO
        //clear screen
        //Display winner
    }

    private bool GetCardeFromPlayerAndCheckIfSimilar()
    {
        Card firstCard = null;
        Card secondCard = null;
        bool sameCardsRevealed = false;

        //clear screen
        m_board.DisplayBoard();

        //Player's cards choises:
        firstCard = GetOneCardFromPlayer();
        secondCard = GetOneCardFromPlayer();

        sameCardsRevealed = m_board.CheckIfSameCards(firstCard, secondCard);
        
        return sameCardsRevealed;
    }

    private Card GetOneCardFromPlayer()
    {
        Card card = new Card();
        bool isValidInput = false;

        while (isValidInput == false)
        {
            System.Console.WriteLine("Please choose a card by writing the numbers of it's row and column: ");

            card.Row = System.Console.Read();
            card.Col = System.Console.Read();

            //isValidInput = 
            isValidInput = m_board.CheckACardOnBoard(card.Row, card.Col);

            //check input, check if 'Q' for endGame, char??
            if (card.Row == 'Q' || card.Col == 'Q')
            {
                m_isGameOver = true;
                break;
            }

            //TO DO
            //if false: clear -> error message -> clear -> display board again
            //else if {}

        }

        //TO DO
        //isValidInput == true
        m_board.RevealCard(card.Row, card.Col);

        return card;
    }

    private void CheckIfGameIsOver()
    {
        if (m_board.IsBoardFull() == true)
        {
            m_isGameOver = true;
        }
    }
}

