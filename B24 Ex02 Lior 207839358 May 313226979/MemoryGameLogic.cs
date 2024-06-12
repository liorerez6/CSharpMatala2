using Ex02.ConsoleUtils;
using System;
using System.Collections.Generic;

public class MemoryGameLogic
{
    //Constants
    private const int k_FirstPlayer = 0;
    private const int k_SecondPlayer = 1;

    //Variables
    private bool m_IsGameOver;
    private int m_CurrentPlayer;                
    private bool m_IsComputerPlayerGameMode;
    private Board m_Board;
    private List<Player> m_Players;

    //CTOR
    public MemoryGameLogic()
    {
        m_Players = new List<Player>();
        ResetGameSetup();
    }

    public void ResetGameSetup()
    {
        m_IsGameOver = false;
        m_CurrentPlayer = k_FirstPlayer;
    }

    //PROPERTIES

    public bool IsComputerVsPlayerGame
    {
        set { m_IsComputerPlayerGameMode = value; }
        get { return m_IsComputerPlayerGameMode; }
    }

    public bool GameStatus
    {
        get { return m_IsGameOver; }
    }

    //METHOODS
    public (Card, Card) PlayGameLogic(Card i_FisrtSlot, Card i_SecondSlot)
    {
        //m_CurrentPlayer = (m_CurrentPlayer == k_FirstPlayer) ? k_SecondPlayer : k_FirstPlayer;

        bool playerEarnedAnotherRound = false;
        (Card card1, Card card2) = (null, null);

        playerEarnedAnotherRound = playHumanPlayerTurn(i_FisrtSlot, i_SecondSlot);

        if (m_IsComputerPlayerGameMode)
        {
            //If human player earned another round-> delete (true) cards from memory, else-> save (false) them
            updateComputerDataBoard(playerEarnedAnotherRound, i_FisrtSlot, i_SecondSlot);

            //if player didn't earned another turn-> play computer turn, and return cards pick
            if (!playerEarnedAnotherRound)
            {
                (card1, card2) = playComputerPlayerTurn();
            }
        }

        return (card1, card2);
    }
  
    private bool playHumanPlayerTurn(Card i_FisrtSlot, Card i_SecondSlot)
    {
        bool cardsAreMatched = checkAndUpdateIfSameCardKey(i_FisrtSlot, i_SecondSlot);
        
        if(cardsAreMatched == false)
        {
            m_CurrentPlayer = (m_CurrentPlayer == k_FirstPlayer) ? k_SecondPlayer : k_FirstPlayer;      //change current player
        }

        return cardsAreMatched;
    }

    private (Card, Card) playComputerPlayerTurn()
    {
        (Card card1, Card card2) = (null, null);
        (card1, card2) = getTwoCardsForComputerPlayer();

        bool earnedAnotherRound = checkAndUpdateIfSameCardKey(card1, card2);

        while (earnedAnotherRound)
        {
            (card1, card2) = getTwoCardsForComputerPlayer();
            earnedAnotherRound = checkAndUpdateIfSameCardKey(card1, card2);
        }

        m_CurrentPlayer = k_FirstPlayer; //change current player

        //If cards are matched-> (true) delete from memory, else-> (false) save in memory
        updateComputerDataBoard(earnedAnotherRound, card1, card2);

        return (card1, card2);
    }

    private (Card, Card) getTwoCardsForComputerPlayer()
    {
        //check if there matched cards in memory, if so- get them
        (Card card1, Card card2) = m_Players[k_SecondPlayer].SearchForMatchedCardsInComuterMemory();

        //if didn't find mathced cards-> both are null-> find a random card1 that wasn't revealed yet
        if (card1 == null)
        {
            card1 = m_Board.FindNeverRevealdCardInBoard(m_Players[k_SecondPlayer]);
            card2 = searchForSameKeyCardInComputerData(card1);

            //if didn't find a match for card1, get another random card
            if (card2 == null)
            {
                card2 = getRandomCardForomputer();
            }
        }

        return (card1, card2);
    }

    private void updateComputerDataBoard(bool i_IfDeleteCards, Card i_Card1, Card i_Card2)
    {
        if (i_IfDeleteCards)
        {
            m_Players[k_SecondPlayer].DeleteKeyFromData(m_Board.GetCardKeyFromBoard(i_Card1));
            m_Players[k_SecondPlayer].DeleteKeyFromData(m_Board.GetCardKeyFromBoard(i_Card2));
            m_IsGameOver = m_Board.IsGameOver();
        }
        else
        {
            saveCardsInComputerData(i_Card1, i_Card2);
        }
    }

    public char GetCardKeyRequseted(Card i_Card)
    {
        return m_Board.GetCardKeyFromBoard(i_Card);
    }


    public void SetBoardDimentionsFromUser(int i_Row, int i_Col)
    {
        m_Board = new Board(i_Row, i_Col);
    }

    //For human and computer
    private bool checkAndUpdateIfSameCardKey(Card i_FirstCard, Card i_SecondCard)
    {
        bool cardsAreMatched = m_Board.CheckIfSameCardsKey(i_FirstCard, i_SecondCard);

        if(cardsAreMatched == true)
        {
            m_Board.RevealCards(i_FirstCard, i_SecondCard);
            m_Players[m_CurrentPlayer].IncreaseScore();
            m_IsGameOver = m_Board.IsGameOver();
        }

        return cardsAreMatched;
    }

    
    public (string, int) CurrentPlayerInfo
    {
        get { return (m_Players[m_CurrentPlayer].Name, m_Players[m_CurrentPlayer].Score); }
    }

    //should try another logic, maybe send only one board of Card with all infrmaition
    public char[,] GetBoardState()
    {
        return m_Board.GetBoardState();
    }

    public bool[,] GetBoardReveals()
    {
        return m_Board.GetBoardReveals();
    }
    
    private Card getRandomCardForomputer()
    {
        return m_Board.FindNeverRevealdCardInBoard(m_Players[k_SecondPlayer]);
    }

    private Card searchForSameKeyCardInComputerData(Card i_SearchForCard)
    {
        char key = m_Board.GetCardKeyFromBoard(i_SearchForCard);
        Card matchingCard = m_Players[k_SecondPlayer].SearchInComputerMemoryMatchingSlotKey(key);
        
        return matchingCard;
    }

    public bool isCardAlreadyReveald(Card i_Card)
    {
        bool isCardAlreadyReveald = (i_Card != null) ? m_Board.IsCardAlreadyReveald(i_Card) : false;

        return isCardAlreadyReveald;
    }

    private void saveCardsInComputerData(Card i_FirstCard, Card i_SecondCard)
    {
        //const int v_FirstPlayer = 1;
        char charOfFirstCard = m_Board.GetCardKeyFromBoard(i_FirstCard);
        char charOfSecondCard = m_Board.GetCardKeyFromBoard(i_SecondCard);

        m_Players[k_SecondPlayer].UpdateCardsInDictionary(i_FirstCard, i_SecondCard, charOfFirstCard, charOfSecondCard);
    }

    public void AddPlayerToGame(string i_FirstName, string i_SecondName, bool i_IsComputerPlayerGameMode)
    {
        Player player1 = new Player(i_FirstName, i_IsComputerPlayerGameMode);
        Player player2 = new Player(i_SecondName, i_IsComputerPlayerGameMode);

        m_Players.Add(player1);
        m_Players.Add(player2);
    }
}
