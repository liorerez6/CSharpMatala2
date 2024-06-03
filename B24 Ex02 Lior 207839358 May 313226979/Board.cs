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

        InitilizeBoard();

    }

    public int Row
    {
        get { return m_Rows; }
    }

    public int Col
    {
        get { return m_Columns; }
    }

    // this way, outside methods cannot change the board state
    public char[,] GetBoardState()
    {
        return (char[,])m_BoardState.Clone();
    }

    public bool[,] GetBoardReveals()
    {
        return (bool[,])m_BoardReveals.Clone();
    }


    public static bool CheckIfCanCreateBoardWithDimentions(int i_Rows, int i_Cols)
    {

        bool validBoardDimention = true;

        bool rowNumberIsOdd = (i_Rows % 2 == 1);

        if ((i_Cols % 2 == 1) && (rowNumberIsOdd == true))
        {
            validBoardDimention = false;
        }

        return validBoardDimention; // needs to explain each error of input (maybe enum)
    }

    private void InitilizeBoard()
    {


        m_Letters = generatePairsOfLetters();

        ShuffleChars(m_Letters);

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

    private void ShuffleChars(List<char> i_ListOfChars)
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

    public void ReveldCard(int row, int col)
    {
        // needs to check if its already occupied

        Console.SetCursorPosition((col * 4) -1, row * 2 );
        Console.Write(m_BoardState[row-1, col-1]);
        Console.SetCursorPosition(0,0);
    }

    public bool CheckIfSameCards(int row1, int col1, int row2, int col2)
    {
        return (m_BoardState[row1-1, col1 - 1] == m_BoardState[row2-1, col2 - 1]);
    }

    public bool CheckIfEmptySlot(int i_SlotRow, int i_SlotCol)
    {

        return (m_BoardReveals[i_SlotRow-1, i_SlotCol-1] == false) ? true : false;
    }

    public void FlipCardStateOnBoard(int i_Row, int i_Col)
    {
        if(m_BoardReveals[i_Row-1, i_Col-1] == true)
        {
            m_BoardReveals[i_Row-1, i_Col-1] = false;
        }
        else
        {
            m_BoardReveals[i_Row-1, i_Col-1] = true;
        }
    }

    public void ChangeCardsStateOnBoard(int row1, int col1, int row2, int col2)
    {
        m_BoardReveals[row1-1, col1-1] = true;
        m_BoardReveals[row2-1, col2-1] = true;
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