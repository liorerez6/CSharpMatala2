

class Player
{
    private int m_Score;
    private string m_Name; // should be readonly because it doesn't change during the game.
    private bool m_IsHumanPlayer = true;

    public Player(string i_Name, bool humanIsPlaying)
    {
        m_Name = i_Name;
        m_Score = 0;
        m_IsHumanPlayer = humanIsPlaying;
    }

    public bool HumanIsPlaying
    {
        get { return m_IsHumanPlayer; }
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
