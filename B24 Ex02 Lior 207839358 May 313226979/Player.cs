

class Player
{
    private int m_Score;
    private string m_Name;
    public int Score
    {
        get { return m_Score; }
    }

    public string Name
    {
        get { return m_Name; }
        set
        {
            if (IsValidName(value))
            {
                m_Name = value;
            }
            else
            {
                //throw execption
            }
        }

    }
    private bool IsValidName(string i_Name)
    {
        // Check if the i_Name contains only letters (A-Z and a-z)
        // and is not empty or null
        if (string.IsNullOrEmpty(i_Name))
        {
            return false;
        }

        foreach (char c in i_Name)
        {
            if (!char.IsLetter(c))
            {
                return false;
            }
        }

        return true;
    }
    public int IncreaseScore()
    {
        return m_Score + 1;
    }

}
