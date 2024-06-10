using System;

public class Card
{
    private int m_Row;
    private int m_Col;
    private char m_CardKey = '\0';
    private bool v_IsSlotReveald = false;

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

    public Card(int row, int col, char key)
    {
        m_Row = row;
        m_Col = col;
        m_CardKey = key;
        v_IsSlotReveald = false;
    }

    public void SetCardDetails(int i_Row, int i_Col, char i_Key, bool i_IsSlotReveald)
    {
        m_Row = i_Row;
        m_Col = i_Col;
        m_CardKey = i_Key;
        v_IsSlotReveald = i_IsSlotReveald;
    }

    public int Row
    {
        get { return m_Row; }
    }

    public int Col
    {
        get { return m_Col; }
    }

    public char CardKey
    {
        set { m_CardKey = value; }
        get { return (char)m_CardKey; }
    }

    public bool RevealingState
    {
        get { return v_IsSlotReveald; }
        set { v_IsSlotReveald = value; }
    }

    public bool IsEqualToSlot(Card i_CheckSlot)
    {
        return (i_CheckSlot.Col == m_Col && i_CheckSlot.Row == m_Row);
    }
}

