using System;
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

    public void DisplayBoard()
    {


        StringBuilder sb = new StringBuilder();
        sb.Append("   ");
        char firstLetter = 'A';

        for (int i = 0; i < m_Columns; i++)
        {
            sb.Append(firstLetter);
            sb.Append("   ");
            firstLetter++;
        }

        sb.AppendLine();

        for (int i = 0; i < m_Rows; i++)
        {
            sb.Append(" ");
            for (int k = 0; k <= m_Columns * 4; k++)
            {
                sb.Append("=");
            }
            sb.AppendLine();

            sb.Append(i + 1);

            for (int j = 0; j < m_Columns; j++)
            {
                sb.Append("|");
                if (m_BoardReveals[i, j])
                {
                    sb.Append(" ");
                    sb.Append(m_BoardState[i, j]);
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
        for (int k = 0; k <= m_Columns * 4; k++)
        {
            sb.Append("=");
        }

        Console.WriteLine(sb.ToString());




        //    System.Console.Write("   ");
        //    char firstLetter = 'A';

        //    for (int i = 0; i<=m_Rows; i++)
        //    {
        //        System.Console.Write(firstLetter);
        //        System.Console.Write("   ");
        //        firstLetter++;
        //    }

        //    System.Console.Write("   \n");

        //    for (int i = 0; i < m_Rows; i++)
        //    {

        //        System.Console.Write(" ");
        //        for (int k = 0; k <= m_Rows * 5; k++)
        //        {
        //            System.Console.Write("=");
        //        }
        //        System.Console.Write("   \n");

        //        System.Console.Write(i+1);

        //        for (int j = 0; j<m_Columns; j++)
        //        {
        //            System.Console.Write("|");
        //            if (m_BoardReveals[i,j] == true)
        //            {
        //                System.Console.Write(" ");
        //                System.Console.Write(m_BoardState[i, j]);
        //                System.Console.Write(" ");
        //            }
        //            else
        //            {
        //                System.Console.Write("   ");
        //            }


        //        }
        //        System.Console.Write("|");
        //        System.Console.Write("\n");

        //    }

        //    System.Console.Write(" ");
        //    for (int k = 0; k <= m_Rows * 5; k++)
        //    {
        //        System.Console.Write("=");
        //    }

        //}



    }

    public void ReveldCard(int row, int col)
    {
        Console.SetCursorPosition((col * 4) -1, row * 2 );
        Console.Write(m_BoardState[row-1, col-1]);
        Console.SetCursorPosition(0,0);
    }

    public bool CheckIfSameCards(int row1, int col1, int row2, int col2)
    {
        return (m_BoardState[row1, col1] == m_BoardState[row2, col2]);
    }

    public void ChangeCardsStateOnBoard(int row1, int col1, int row2, int col2)
    {
        m_BoardReveals[row1, col1] = true;
        m_BoardReveals[row2, col2] = true;
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