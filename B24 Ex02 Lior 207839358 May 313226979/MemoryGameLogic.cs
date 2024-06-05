using System;
using System.Collections.Generic;

public class MemoryGameLogic
{

    private List<Player> m_Players;
    private Board m_Board;
    private bool m_IsComputerPlayerGame;
    private int m_NumberOfTurns;
    private bool m_GameIsOver;

    public MemoryGameLogic()
    {
        m_Players = new List<Player>();
        m_Players.Capacity = 2;  // in this game only 2 players are allowed
        m_NumberOfTurns = 0;
        m_GameIsOver = false;
    }

    public void reGameSetup()
    {
        const int v_FirstPlayer = 0;
        const int v_SecondPlayer = 1;

        m_GameIsOver = false;
        m_NumberOfTurns = 0;
        m_Players[v_FirstPlayer].InitilizeScore();
        m_Players[v_SecondPlayer].InitilizeScore();
    }

    public bool GameIsOverStatus
    {
        get { return m_GameIsOver; }
    }

    public void GetGameModeFromUser(bool i_GameMode)
    {
            m_IsComputerPlayerGame = !i_GameMode;
    }

    public bool IsComputerVsPlayerGame
    {
        get { return m_IsComputerPlayerGame; }
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
        }

        return validDimentionsForBoard;
    }

    private bool IsGameOver() 
    {  
        return m_Board.IsBoardFull();
    }

    public bool IsValidEmptySlot(Card i_Card)
    {
        return (m_Board.CheckIfEmptySlot(i_Card));
    }

    public void FlipChosenCard(Card i_Card, bool i_Reveal)
    {
        if (i_Reveal)
        {
            m_Board.RevealedCard(i_Card);
        }
        else
        {
            m_Board.HideCard(i_Card);
        }
    }

    public (Card, Card) CheckForMatchedCardsInMemoryList()
    {
        const int v_FirstPlayer = 1;
        return m_Players[v_FirstPlayer].CheckForMatchedCardsInMemoryList();
    }

    public Card GetRandomCardFromComputer()
    {
        return m_Board.FindAHiddenCardInBoard(m_Players[1]);

    }

    public Card FindMatchingCard(Card i_SearchForCard) 
    {
        const int v_FirstPlayer = 1;
        Card matchingCard = null;
        char key = m_Board.GetCharFromIndexInBoard(i_SearchForCard);


        matchingCard = m_Players[v_FirstPlayer].SearchForAMatchingCard(key);
        return m_Players[v_FirstPlayer].SearchForAMatchingCard(key);
 
    }

    
    public void UpdateCardInComputerData(Card i_FirstCard, Card i_SecondCard) 
    {
        const int v_FirstPlayer = 1;
        char charOfFirstCard = m_Board.GetCharFromIndexInBoard(i_FirstCard);
        char charOfSecondCard = m_Board.GetCharFromIndexInBoard(i_SecondCard);

        m_Players[v_FirstPlayer].UpdateCardsInDictionary(i_FirstCard, i_SecondCard, charOfFirstCard, charOfSecondCard);
    }

    public void DeleteCardsFromComputerData(Card i_Card)
    {
        m_Players[1].DeleteKeyFromData(m_Board.GetCharFromIndexInBoard(i_Card));
    }


    public bool CheckForMatchAndUpdateAccordinly(Card i_FirstCard, Card i_SecondCard)
    {
        bool cardsAreMatched = true;

        if(m_Board.CheckIfSameCards(i_FirstCard, i_SecondCard))
        {
            m_Board.RevealedCard(i_FirstCard);
            m_Board.RevealedCard(i_SecondCard);

            if (m_Players[1].HumanIsPlaying == false)
            {
                m_Players[1].DeleteKeyFromData(m_Board.GetCharFromIndexInBoard(i_FirstCard));
            }
            


            // m_Board.ChangeCardsStateOnBoard(i_FirstCard, i_SecondCard);
            m_Players[m_NumberOfTurns % 2].IncreaseScore();
            if(IsGameOver())
            {
                m_GameIsOver = true;
            }
        }

        else
        {
            m_Board.HideCard(i_FirstCard);
            m_Board.HideCard(i_SecondCard);
            m_NumberOfTurns++;
            cardsAreMatched = false;
        }

        return cardsAreMatched;
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

    public bool ChangeSlotWhithingDimentions(int i_Row, int i_Col)
    {
        return (i_Col <= m_Board.Col && i_Col >= 1 && i_Row <= m_Board.Row && i_Row >= 1);
    }

}

