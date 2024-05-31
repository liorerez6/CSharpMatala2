

class Player
{
    private int m_Score;
    private string m_Name;


    public Player(string i_Name)
    {
        m_Name = i_Name;
        m_Score = 0;
    }

    public int Score
    {
        get { return m_Score; }
    }

    public string Name
    {
        get { return m_Name; }
        
    }
   
    public void IncreaseScore()
    {
        m_Score++;
    }

}
