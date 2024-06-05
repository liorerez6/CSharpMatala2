using System;
using System.Collections.Generic;
using System.Linq;


class Player
{
    private int m_Score;
    private readonly string m_Name; // should be readonly because it doesn't change during the game.
    private bool m_IsHumanPlayer = true;
    private Dictionary<char, List<Card>> m_RevealedCards;

    public Player(string i_Name, bool humanIsPlaying)
    {
        m_Name = i_Name;
        m_Score = 0;
        m_IsHumanPlayer = humanIsPlaying;

        if (!m_IsHumanPlayer)
        {
            m_RevealedCards = new Dictionary<char, List<Card>>();
        }
    }

    public Card SearchForAMatchingCard(char i_Key)
    {

        if (m_RevealedCards.ContainsKey(i_Key) && m_RevealedCards[i_Key].Count > 0)
        {
            return m_RevealedCards[i_Key][0];
        }
        return null;
    }

    public bool IsCardInMemoryRevealedCards(Card i_SearchForCard, char i_Key)
    {
        
        if (m_RevealedCards.ContainsKey(i_Key))
        {
            return m_RevealedCards[i_Key].Any(card => card.Row == i_SearchForCard.Row && card.Col == i_SearchForCard.Col);
        }
        return false;
    }

    public (Card, Card) CheckForMatchedCardsInMemoryList()
    {
        foreach (var card in m_RevealedCards)
        {
            if (card.Value.Count == 2)
            {
                var card1 = card.Value[0];
                var card2 = card.Value[1];
                
                
                return (card1, card2);
            }
        }
        return (null, null);
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

    public void DeleteKeyFromData(char i_Key)
    {
        m_RevealedCards.Remove(i_Key);
    }

    public void UpdateCardInDictionary1(Card i_Card1, char i_CardKey)
    {
        bool addNewCard = false;

        if (m_RevealedCards.ContainsKey(i_CardKey))
        {
            addNewCard = (m_RevealedCards[i_CardKey])[0].AreCardEquals(i_Card1);
        }
        else
        {
            m_RevealedCards[i_CardKey] = new List<Card>();
        }

        if (addNewCard == false)
        {
            m_RevealedCards[i_CardKey].Add(i_Card1);
        }

    }

    public void UpdateCardsInDictionary(Card i_Card1, Card i_Card2, char i_FirstCard, char i_SecondCard)
    {
        UpdateCardInDictionary1(i_Card1, i_FirstCard);
        UpdateCardInDictionary1(i_Card2, i_SecondCard);
    }

    public void UpdateCardInDictionary(Card i_Card1, Card i_Card2, char i_FirstCard, char i_SecondCard)
    {
        if (!m_RevealedCards.ContainsKey(i_FirstCard))
        {
            m_RevealedCards[i_FirstCard] = new List<Card>();
        }

        if (!m_RevealedCards.ContainsKey(i_SecondCard))
        {
            m_RevealedCards[i_SecondCard] = new List<Card>();
        }

        if (!m_RevealedCards[i_FirstCard].Contains(i_Card1))
        {
            m_RevealedCards[i_FirstCard].Add(i_Card1);
        }

        if (!m_RevealedCards[i_SecondCard].Contains(i_Card2))
        {
            m_RevealedCards[i_SecondCard].Add(i_Card2);
        }
    }

    public void InitilizeScore()
    {
        m_Score = 0;
    }
}

