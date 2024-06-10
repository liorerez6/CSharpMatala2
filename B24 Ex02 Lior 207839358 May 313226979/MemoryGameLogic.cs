using System;
using System.Collections.Generic;

public class MemoryGameLogic
{
    private const int k_FirstPlayer = 0;
    private const int k_SecondPlayer = 1;

    private List<Player> m_Players;
    private Board m_Board;
    private bool m_IsComputerPlayerGame;
    private int m_NumberOfTurns;
    private bool m_GameIsOver;

    public MemoryGameLogic()
    {
        m_Players = new List<Player>();
        m_Players.Capacity = 2;
        m_NumberOfTurns = 0;
        m_GameIsOver = false;
    }

    public void NewGameRoundSetup()
    {
        m_GameIsOver = false;
        m_NumberOfTurns = 0;
        m_Players[k_FirstPlayer].InitilizeScore();
        m_Players[k_SecondPlayer].InitilizeScore();
    }

    public bool GameIsOverStatus
    {
        get { return m_GameIsOver; }
        set { m_GameIsOver = value; }
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
        bool v_HumanIsPlaying = true;

        addPlayerToGame(i_FirstName, v_HumanIsPlaying);

        v_HumanIsPlaying = (m_IsComputerPlayerGame == true);
        addPlayerToGame(i_SecondName, !v_HumanIsPlaying);
    }

    public bool GetBoardDimentionsFromUser(int i_Row, int i_Col)
    {
        bool validDimentionsForBoard = Board.CheckIfCanCreateBoardWithDimentions(i_Row, i_Col);

        if(validDimentionsForBoard)
        {
            m_Board = new Board(i_Row, i_Col);    
        }
  
        return validDimentionsForBoard;
    }

    private bool isGameOver() 
    {
        m_GameIsOver = m_Board.IsBoardFull();

        return m_GameIsOver;
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

    public (Card, Card) CheckForMatchedCardsInComputerData()
    {
        return m_Players[k_SecondPlayer].CheckForMatchedCardsInMemoryList();
    }

    public Card GetRandomCardForComputer()
    {
        return m_Board.FindAHiddenCardInBoard(m_Players[k_SecondPlayer]);
    }

    public Card FindMatchingCard(Card i_SearchForCard) 
    {
        char key = m_Board.GetCharFromIndexInBoard(i_SearchForCard);
        Card matchingCard = m_Players[k_SecondPlayer].SearchForAMatchingCard(key);

        return m_Players[k_SecondPlayer].SearchForAMatchingCard(key);
    }

    public void UpdateCardInComputerData(bool isDeleteCard, Card i_FirstCard, Card i_SecondCard) 
    {
        if (isDeleteCard)
        {
            m_Players[k_SecondPlayer].DeleteKeyFromData(m_Board.GetCharFromIndexInBoard(i_FirstCard));
            m_Players[k_SecondPlayer].DeleteKeyFromData(m_Board.GetCharFromIndexInBoard(i_SecondCard));
        }
        else
        {
            char charOfFirstCard = m_Board.GetCharFromIndexInBoard(i_FirstCard);
            char charOfSecondCard = m_Board.GetCharFromIndexInBoard(i_SecondCard);

            m_Players[k_SecondPlayer].UpdateCardsInDictionary(i_FirstCard, i_SecondCard, charOfFirstCard, charOfSecondCard);
        }
    }

    public bool CheckForMatchAndUpdateAccordinly(Card i_FirstCard, Card i_SecondCard)
    {
        bool cardsAreMatched = m_Board.CheckIfSameCards(i_FirstCard, i_SecondCard);

        if(cardsAreMatched)
        {
            m_Board.RevealedCard(i_FirstCard);
            m_Board.RevealedCard(i_SecondCard);
            m_Players[m_NumberOfTurns % 2].IncreaseScore();
            m_GameIsOver = isGameOver();
        }
        else
        {
            m_Board.HideCard(i_FirstCard);
            m_Board.HideCard(i_SecondCard);
            m_NumberOfTurns++;
        }

        if (m_Players[k_SecondPlayer].HumanIsPlaying == false)
        {
            UpdateCardInComputerData(cardsAreMatched, i_FirstCard, i_SecondCard);
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
}

