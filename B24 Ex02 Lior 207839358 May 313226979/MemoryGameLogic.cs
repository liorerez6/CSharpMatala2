using System;
using System.Collections.Generic;

public class MemoryGameLogic
{

    private List<Player> m_Players;
    private Board m_Board;
    bool m_IsComputerPlayerGame;
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

    public (Card, Card) CheckForMatchedCardsInMemoryList()
    {
        return m_Players[1].CheckForMatchedCardsInMemoryList();
    }

    public Card GetRandomCardFromComputer()
    {
        return m_Board.FindAHiddenCardInBoard(m_Players[1]);

    }

    public Card FindMattchingCard(Card i_SearchForCard) 
    {        
        char key = m_Board.GetCharFromIndexInBoard(i_SearchForCard.Row, i_SearchForCard.Col);

        return m_Players[1].SearchForAMattchingCard(key);
 
    }

    
    public void UpdateCardInComputerData(Card i_FirstCard, Card i_SecondCard) {
        char firstCard = m_Board.GetCharFromIndexInBoard(i_FirstCard);
        char secondCard = m_Board.GetCharFromIndexInBoard(i_SecondCard);

        m_Players[1].UpdateCardInDictonary(i_FirstCard, i_SecondCard);
    }


    //public void UpdateCardInComputerData(int i_FirstRowCard, int i_FirstColCard, int i_SecondRowCard, int i_SecondColCard)
    //{
    //    char firstCard = m_Board.GetCharFromIndexInBoard(i_FirstRowCard, i_FirstColCard);
    //    char secondCard = m_Board.GetCharFromIndexInBoard(i_SecondRowCard, i_SecondColCard);

    //    m_Players[1].UpdateCardInDictonary(i_FirstRowCard, i_FirstColCard, i_SecondRowCard, i_SecondColCard, firstCard, secondCard);
    //}


    public bool CheckForMatchAndUpdateAccordinly(int i_FirstPickRowSlot, int i_FirstPickColSlot, int i_SecondPickRowSlot, int i_SecondPickColSlot)
    {
        bool cardsAreMatched = true;


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

}

