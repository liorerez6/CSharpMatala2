
class Position
{
    private int m_RowIndex;
    private int m_ColumnIndex;

    public int Row
    {
        get { return m_RowIndex; }
        set
        {
            if(value < 4 || value > 7)
            {
                // throw exeption 
            }
            else
            {
                m_RowIndex = value;
            }
        }
    }

    public int Collumn
    {
        get { return m_ColumnIndex; }
        set
        {
            if (value < 4 || value > 7)
            {
                // throw exeption 
            }
            else
            {
                m_ColumnIndex = value;
            }
        }
    }

}


