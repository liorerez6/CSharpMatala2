using System;
using System.Collections.Generic;
using System.Text;

class Board
{
    //CONSTANTS
    private const int m_ColSlotDiff = 4;
    private const int m_RowSlotDiff = 2;

    private int m_Rows;
    private int m_Columns;
    
    private Card[,] m_CardsBoard = null;

    private char[,] m_BoardState;
    private bool[,] m_BoardReveals;
    //private List<char> m_Letters;

    //CTOR 
    public Board(int i_Rows, int i_Cols)
    {
        m_Rows = i_Rows;
        m_Columns = i_Cols;

        //NEW: better option- one Card board, eahc includes it's state (revealed or not)
        m_CardsBoard = new Card[m_Rows + 1, m_Columns + 1];

        m_BoardState = new char[i_Rows, i_Cols];
        m_BoardReveals = new bool[i_Rows, i_Cols];

        initilizeBoard();
    }
    
    //METHODS

    public char[,] GetBoardState()
    {
        return (char[,])m_BoardState.Clone(); 
    }

    public bool[,] GetBoardReveals()
    {
        return (bool[,])m_BoardReveals.Clone();
    }

    public char GetCardKeyFromBoard(Card i_Card)
    {
        //return m_CardsBoard[i_Card.Row, i_Card.Col].CardKey;
        return (char)m_BoardState[i_Card.Row-1, i_Card.Col-1];
    }

    private void fillBoardInChar(List<char> i_ListOfChars)
    {
        int indexOfList = 0;

        for (int i = 0; i < m_Rows; i++)
        {
            for (int j = 0; j < m_Columns; j++)
            {
                Card card = new Card(i, j, i_ListOfChars[indexOfList]);
                m_CardsBoard[i, j] = card;

                m_BoardState[i, j] = i_ListOfChars[indexOfList];
                m_BoardReveals[i, j] = false;
                indexOfList++;
            }
        }
    }

    //NEW
    private void setCardsBoardWithNewCards(List<char> i_ListOfChars)
    {
        //NEW
        const bool cardIsReveald = true;
        int row = 0, col = 0;
        char letter = 'A';
        int indexOfList = 0;

        //First cell is empty, NON-REACHABLE

        m_CardsBoard[row, col] = new Card();
        m_CardsBoard[row, col].SetCardDetails(0, 0, ' ', cardIsReveald);

        //fill first column with numbers
        for (row = 1; row <= m_Rows; row++)
        {
            Card card = new Card();
            card.SetCardDetails(row, 1, row.ToString()[0], cardIsReveald);
            m_CardsBoard[row, 1] = card;
        }

        //fill first row to chars
        for (col = 1; col <= m_Columns; col++)
        {
            Card card = new Card(1, col, letter);
            card.SetCardDetails(1, col, letter, cardIsReveald);

            m_CardsBoard[1, col] = card;
            letter++;
        }

        for (row = 1; row <= m_Rows; row++)
        {
            for (col = 1; col < m_Columns; col++)
            {
                Card card = new Card(row, col, i_ListOfChars[indexOfList]);
                m_CardsBoard[row, col] = card;
                indexOfList++;
            }
        }
    }

    private void initilizeBoard()
    {
        //Creating a list with pairs of letters to be set as cards on board 
        List<char> listOfPairs = new List<char>();
        int sizeOfPairs = (m_Columns * m_Rows) / 2;
        char letter = 'A';
 
        for (int i = 0; i < sizeOfPairs; i++)
        {
            listOfPairs.Add(letter);
            listOfPairs.Add(letter);
            letter++;
        }

        //Shuffle the list randomlly
        shuffleChars(listOfPairs);

        //fill the board with the new organized list
        fillBoardInChar(listOfPairs);

        setCardsBoardWithNewCards(listOfPairs);
    }

    private void shuffleChars(List<char> i_ListOfChars)
    {
        Random rng = new Random();
        int sizeOfList = i_ListOfChars.Count;

        while (sizeOfList > 1)
        {
            sizeOfList--;
            int k = rng.Next(sizeOfList + 1);
            char value = i_ListOfChars[k];
            i_ListOfChars[k] = i_ListOfChars[sizeOfList];
            i_ListOfChars[sizeOfList] = value;
        }
    }

    public Card FindNeverRevealdCardInBoard(Player i_ComputerPlayer)
    {
        Card card = null;
        bool keepSearching = true;

        for(int i=0; i< m_Rows && keepSearching; i++)
        {
            for(int j=0; j< m_Columns && keepSearching; j++)
            {
                if (m_BoardReveals[i ,j] == false)
                {
                    card = new Card(i+1, j+1);
                    char charOfCard = m_BoardState[i, j];

                    if (!i_ComputerPlayer.IsCardInMemoryRevealedCards(card, charOfCard)) 
                    {
                        keepSearching = false;
                    }
                }
            }
        }

        return card;
    }

    public bool IsGameOver()
    {
        //check if all cards are reveald
        bool isBoardFull = true;

        foreach (bool state in m_BoardReveals)
        {
            if (state == false)
            {
                isBoardFull = false;
                break;
            }
        }

        return isBoardFull;
    }

    //SLOT CHANGES

    public void RevealCards(Card i_FirstCard, Card i_SecondCard)
    {
        m_BoardReveals[i_FirstCard.Row - 1, i_FirstCard.Col - 1] = true;
        m_BoardReveals[i_SecondCard.Row - 1, i_SecondCard.Col - 1] = true;
    }

    public bool CheckIfSameCardsKey(Card i_FirstCard, Card i_SecondCard)
    {
        return (m_BoardState[i_FirstCard.Row - 1, i_FirstCard.Col - 1] == m_BoardState[i_SecondCard.Row - 1, i_SecondCard.Col - 1]);
    }

    public bool IsCardAlreadyReveald(Card i_Card)
    {
        return (m_BoardReveals[i_Card.Row - 1, i_Card.Col - 1]);
    }
}
