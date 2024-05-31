using System;

public class Computer
{
    private char[,] m_RevealedCardsMemoryBoard;

    public Computer()
    {
        m_RevealedCardsMemoryBoard = null;
    }

    private void ChooseCards()
    {
        //choose visely:
        if (m_RevealedCardsMemoryBoard != null)
        {
            //choose one from it, then search the other one
            //if the other one isn't in memory- choose another not from memory
            //SearchForCardInMemoryBoard()
        }

        else
        {
            //choose 2 cards randomlly

        }

        //remember coised
    }

    private void UpdateRevealedCardsMemoryBoard(int col, int row, char card)
    {
        m_RevealedCardsMemoryBoard[row, col] = card;
    }

    private bool SearchForCardInMemoryBoard(char i_Search_card)
    {
        bool isCardFounded = false;

        foreach (char card in m_RevealedCardsMemoryBoard)
        {
            if (card == i_Search_card)
            {
                isCardFounded = true;
                break;
            }
        }

        return isCardFounded;
    }

}
