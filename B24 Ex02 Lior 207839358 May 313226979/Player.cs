

class Player
{
    private int m_Score;
    private string m_Name; // should be readonly because it doesn't change during the game.


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
    
        get { return m_Name; } // doesn't need set because gets the Name after checking from the input Manager
        
    }
   
    public void IncreaseScore()
    {
        m_Score++;
    }

}
