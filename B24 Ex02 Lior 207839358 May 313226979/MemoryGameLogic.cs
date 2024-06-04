using System;
using System.Collections.Generic;

public class MemoryGameLogic
{

    private List<Player> m_Players;
    private Board m_Board;
    bool m_IsComputerPlayerGame;
    bool m_IsPlayerVsPlayerGame;
    private int m_NumberOfTurns;
    bool m_GameIsOver;

    public MemoryGameLogic()
    {
        m_Players = new List<Player>();
        m_Players.Capacity = 2;  // in this game only 2 players are allowed
        m_NumberOfTurns = 0;
        m_GameIsOver = false;
    }

    public bool GameIsOver
    {
        get { return m_GameIsOver; }
    }

    public void GetGameModeFromUser(bool i_GameMode)
    {
            m_IsPlayerVsPlayerGame = i_GameMode;
            m_IsComputerPlayerGame = !i_GameMode;
    }

    public string GetPlayersTurn()
    {
        return m_Players[m_NumberOfTurns % 2].Name;
    }

    private void addPlayerToGame(string i_Name, bool i_HumanIsPlaying)
    {
        Player player = new Player(i_Name, i_HumanIsPlaying);
        m_Players.Add(player);
    }

    public void AddPlayersToGame(string i_FirstName, string i_SecondName)
    {
        bool i_HumanIsPlaying = true;

        addPlayerToGame(i_FirstName, i_HumanIsPlaying);

        if (m_IsComputerPlayerGame == true)
        {
            i_HumanIsPlaying = false;
        }
        
        addPlayerToGame(i_SecondName, i_HumanIsPlaying);

    }

    public bool GetBoardDimentionsFromUser(int i_Row, int i_Col)
    {

        bool validDimentionsForBoard = true;

        if(Board.CheckIfCanCreateBoardWithDimentions(i_Row,i_Col))
        {
            m_Board = new Board(i_Row, i_Col);
            
        }
        else
        {
            validDimentionsForBoard = false;
            // needs to return enum according to the problem with the input.
        }

        return validDimentionsForBoard;
    }

    public bool IsGameOver() // here need to check with Board if Game is over
    {
       
        return m_Board.IsBoardFull() ? true : false;
    }


    public bool IsValidEmptySlot(int i_SlotRow, int i_SlotCol)
    {

        return (m_Board.CheckIfEmptySlot(i_SlotRow, i_SlotCol)) ? true : false;
    }

    public void FlipChosenCard(int i_Row, int i_Col)
    {
        m_Board.FlipCardStateOnBoard(i_Row, i_Col);
    }

    public void CheckForMatchAndUpdateAccordinly(int i_FirstPickRowSlot, int i_FirstPickColSlot, int i_SecondPickRowSlot, int i_SecondPickColSlot)
    {
        if(m_Board.CheckIfSameCards(i_FirstPickRowSlot, i_FirstPickColSlot, i_SecondPickRowSlot, i_SecondPickColSlot))
        {
            m_Board.ChangeCardsStateOnBoard(i_FirstPickRowSlot, i_FirstPickColSlot, i_SecondPickRowSlot, i_SecondPickColSlot);
            m_Players[m_NumberOfTurns % 2].IncreaseScore();
            if(IsGameOver())
            {
                m_GameIsOver = true;
            }
        }
        else
        {
            FlipChosenCard(i_FirstPickRowSlot, i_FirstPickColSlot);
            FlipChosenCard(i_SecondPickRowSlot, i_SecondPickColSlot);
            m_NumberOfTurns++;
        }
        
        // computer here needs to remember the cards of the player

    }
  
    public char[,] GetBoardState()
    {
        return m_Board.GetBoardState();
    }

    public bool[,] GetBoardReveals()
    {
        return m_Board.GetBoardReveals();
    }

    public (string, int) GetCurrentPlayerInfo()
    {
        Player currentPlayer = m_Players[m_NumberOfTurns % 2];
        return (currentPlayer.Name, currentPlayer.Score);
    }

}

