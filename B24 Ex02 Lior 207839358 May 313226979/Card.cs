using System;

public class Card
{
    private int m_Row;
    private int m_Col;

    public Card()
    {
        m_Row = 0;
        m_Col = 0;
    }

    public Card(int row, int col)
    {
        m_Row = row;
        m_Col = col;
    }

    public int Row
    {
        get { return m_Row; }
    }

    public int Col
    {
        get { return m_Col; }
    }

    public bool AreCardEquals(Card i_checkcard)
    {
        return (i_checkcard.Col == m_Col && i_checkcard.Row == m_Row);
    }
}

