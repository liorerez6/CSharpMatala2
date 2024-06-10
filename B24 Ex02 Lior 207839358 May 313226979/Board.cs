﻿using System;
using System.Collections.Generic;
using System.Text;

class Board
{
    private int m_Rows;
    private int m_Columns;
    private char[,] m_BoardState;
    private bool[,] m_BoardReveals;
    private List<char> m_Letters;

    public Board(int i_Rows, int i_Cols)
    {

        m_Rows = i_Rows;
        m_Columns = i_Cols;
        m_BoardState = new char[i_Rows, i_Cols];
        m_BoardReveals = new bool[i_Rows, i_Cols];

        initilizeBoard();

    }

    public int Row
    {
        get { return m_Rows; }
    }

    public int Col
    {
        get { return m_Columns; }
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
        m_BoardReveals[i_Card.Row-1, i_Card.Col-1] = true;
    }

    public void HideCard(Card i_Card)
    {
        m_BoardReveals[i_Card.Row - 1, i_Card.Col - 1] = false;
    }

    public static bool CheckIfCanCreateBoardWithDimentions(int i_Row, int i_Col)
    {

        bool validBoardDimention = true;

        bool rowNumberIsOdd = (i_Row % 2 == 1);

        if ((i_Col % 2 == 1) && (rowNumberIsOdd == true))
        {
            validBoardDimention = false;
        }

        return validBoardDimention; // needs to explain each error of input (maybe enum)
    }

    private void initilizeBoard()
    {
        m_Letters = generatePairsOfLetters();

        shuffleChars(m_Letters);

        fillBoardInChar(m_Letters);

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

    private List<char> generatePairsOfLetters()
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

        return listOfPairs;
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

    public void ReveldCard(Card i_Card)
    {
        Console.SetCursorPosition((i_Card.Col * 4) - 1, i_Card.Row * 2);
        Console.Write(m_BoardState[i_Card.Row - 1, i_Card.Col - 1]);
        Console.SetCursorPosition(0, 0);
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

    public void FlipCardStateOnBoard(Card i_Card)
    {
        if(m_BoardReveals[i_Card.Row - 1, i_Card.Col - 1] == true)
        {
            m_BoardReveals[i_Card.Row - 1, i_Card.Col - 1] = false;
        }
        else
        {
            m_BoardReveals[i_Card.Row - 1, i_Card.Col - 1] = true;
        }
    }

    public void ChangeCardsStateOnBoard(Card i_FirstCard, Card i_SecondCard)
    {
        m_BoardReveals[i_FirstCard.Row - 1, i_FirstCard.Col - 1] = true;
        m_BoardReveals[i_SecondCard.Row - 1, i_SecondCard.Col - 1] = true;
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