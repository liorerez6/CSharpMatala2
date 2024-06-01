

using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

class Player
{
    private int m_Score = 0;
    private string m_Name;

    public Player()
    {
        m_Name = GetPlayerName();
    }

    public Player(string i_Coumputer)
    {
        m_Name = i_Coumputer;
    }

    private string GetPlayerName()
    {
        bool validInput = false;
        string getPlayerName = null;

        while (validInput == false)
        {
            //TO DO
            //clear screen
            System.Console.WriteLine("Please type you name, using letters only: ");
            getPlayerName = System.Console.ReadLine();
            validInput = IsValidName(getPlayerName);
        }

        return getPlayerName;
    }

    public int Score
    {
        get { return m_Score; }
        set { m_Score = value; }
    }

    public string Name
    {
        get { return m_Name; }

        //maybe name should'nt be available to re-set after object is created
        //set { m_Name = value; }       
    }

    private bool IsValidName(string i_Name)
    {
        bool isValidName = true;

        // Check if the i_Name contains only letters (A-Z and a-z)
        // and is not empty or null
        if (string.IsNullOrEmpty(i_Name))
        {
            isValidName = false;
        }

        foreach (char c in i_Name)
        {
            if (!char.IsLetter(c))
            {
                isValidName = false;
                break;
            }
        }

        if(isValidName == false)
        {
            System.Console.WriteLine("Invalid input! \nPlease try again, usuing A-Z or a-z letters only.");
        }

        return isValidName;
    }

    public void IncreaseScore()
    {
        m_Score++;
    }

    //public bool GetACardFromPlayer(ref int i_row, ref int i_col)
    //{
    //    bool endGameInput = false;
    //    bool isValidInput = false;

    //    while (isValidInput == false)
    //    {
    //        System.Console.WriteLine("Please type (row, column) of the card you want to reveal: ");
    //        i_row = System.Console.Read();
    //        i_col = System.Console.Read();

    //        //TO DO
    //        //isValidInput = 
    //        //check input, check if 'Q' for endGame, char??
    //        if( i_row == 'Q' || i_col == 'Q')
    //        {
    //            endGameInput = true;
    //            break;
    //        }

    //    }

    //    return endGameInput;
    //}

}
