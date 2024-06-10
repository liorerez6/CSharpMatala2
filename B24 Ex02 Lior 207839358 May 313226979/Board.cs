using System;
using System.Collections.Generic;
using System.Text;

class Board
{
    private const bool k_isRevealdCard = true;
    private int m_Rows;
    private int m_Columns;
    private char[,] m_BoardState;
    private bool[,] m_BoardReveals;
    
    public Board(int i_Rows, int i_Cols)
    {
        m_Rows = i_Rows;
        m_Columns = i_Cols;
        m_BoardState = new char[i_Rows, i_Cols];
        m_BoardReveals = new bool[i_Rows, i_Cols];

        initilizeBoard();
    }

    public char[,] GetBoardState()
    {
        return (char[,])m_BoardState.Clone();
    }

    public bool[,] GetBoardReveals()
    {
        return (bool[,])m_BoardReveals.Clone();
    }

    public char GetCharFromIndexInBoard(Card i_Card)
    {
        return (char)m_BoardState[i_Card.Row-1, i_Card.Col-1];
    }

    public void RevealedCard(Card i_Card)
    {
        if(i_Card != null)
        {
            m_BoardReveals[i_Card.Row - 1, i_Card.Col - 1] = k_isRevealdCard;
        }
    }

    public void HideCard(Card i_Card)
    {
        m_BoardReveals[i_Card.Row - 1, i_Card.Col - 1] = !k_isRevealdCard;
    }

    public static bool CheckIfCanCreateBoardWithDimentions(int i_Row, int i_Col)
    {
        bool isValidDimentionRequest = (i_Row % 2 == 1);

        if (isValidDimentionRequest)
        {
            isValidDimentionRequest = (i_Col % 2 == 1); // meaing Col is Odd and Row is Odd
        }

        return !isValidDimentionRequest;  // return if row is even or col is even
    }

    private void fillBoardInChar(List<char> i_ListOfChars)
    {
        int indexOfList = 0;

        for(int i = 0; i<m_Rows;i++)
        {
            for(int j = 0; j<m_Columns; j++)
            {
                m_BoardState[i, j] = i_ListOfChars[indexOfList];
                m_BoardReveals[i, j] = false;
                indexOfList++;
            }
        }
    }

    private void initilizeBoard()
    {
        List<char> listOfPairs = new List<char>();
        int sizeOfPairs = (m_Columns * m_Rows) / 2;
        char letter = 'A';
 
        for (int i = 0; i < sizeOfPairs; i++)
        {
            listOfPairs.Add(letter);
            listOfPairs.Add(letter);
            letter++;
        }

        shuffleChars(listOfPairs);
        fillBoardInChar(listOfPairs);
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

    public bool CheckIfSameCards(Card i_FirstCard, Card i_SecondCard)
    {
        return (m_BoardState[i_FirstCard.Row-1, i_FirstCard.Col -1] == m_BoardState[i_SecondCard.Row - 1, i_SecondCard.Col - 1]);
    }

    public bool CheckIfEmptySlot(Card i_Card)
    {
        return !(m_BoardReveals[i_Card.Row - 1, i_Card.Col - 1]);
    }

    public Card FindAHiddenCardInBoard(Player i_ComputerPlayer)
    {
        Card card = null;
        bool keepSearching = true;

        for(int i=0; i< m_Rows && keepSearching; i++)
        {
            for(int j=0; j< m_Columns && keepSearching; j++)
            {
                if (m_BoardReveals[i ,j] == false)
                {
                    char charOfCard = m_BoardState[i, j];
                    card = new Card(i+1, j+1);

                    if (!i_ComputerPlayer.IsCardInMemoryRevealedCards(card, charOfCard)) 
                    {
                        keepSearching = false;
                    }
                }
            }
        }

        return card;
    }

    public bool IsBoardFull()
    {
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
}