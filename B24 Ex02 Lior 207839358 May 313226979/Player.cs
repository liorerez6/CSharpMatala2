using System;
using System.Collections.Generic;
using System.Linq;


class Player
{
    //CONSTANTS
    private readonly string r_Name;
    private int m_Score;
    private Dictionary<char, List<Card>> m_ComputerDataForRevealedSlots = null;
    
    //CTOR
    public Player(string i_Name, bool i_IsPlayerVSComputerMode)
    {
        r_Name = i_Name;
        m_Score = 0;
        //m_IsHumanPlayer = i_IsPlayerVSComputerMode;

        if (i_IsPlayerVSComputerMode == true)
        {
            m_ComputerDataForRevealedSlots = new Dictionary<char, List<Card>>();
        }
    }

    //PROPERTIES
    public int Score
    {
        set { m_Score = value; }
        get { return m_Score; }
    }

    public string Name
    {
        get { return r_Name; } 
    }


    //METHODS

    //For both kinds of players
    public void IncreaseScore()
    {
        m_Score++;
    }

    //For Computer Player
    public Card SearchInComputerMemoryMatchingSlotKey(char i_Key)
    {
        Card card = null;

        if (m_ComputerDataForRevealedSlots.ContainsKey(i_Key) && m_ComputerDataForRevealedSlots[i_Key].Count > 0)
        {
            card = m_ComputerDataForRevealedSlots[i_Key][0];
        }

        return card;
    }

    public (Card, Card) SearchForMatchedCardsInComuterMemory()
    {
        (Card firstCard, Card secondCard) = (null, null);

        foreach (var card in m_ComputerDataForRevealedSlots)
        {
            if (card.Value.Count == 2)
            {
                var card1 = card.Value[0];
                var card2 = card.Value[1];

                (firstCard, secondCard) = (card1, card2);
            }
        }

        return (firstCard, secondCard);
    }

    //need to improve readability
    public bool IsCardInMemoryRevealedCards(Card i_SearchForCard, char i_Key)
    {
        bool cardInMemory = false;
        
        if (m_ComputerDataForRevealedSlots.ContainsKey(i_Key))
        {
            cardInMemory =  m_ComputerDataForRevealedSlots[i_Key].Any(card => card.Row == i_SearchForCard.Row && card.Col == i_SearchForCard.Col);
        }

        return cardInMemory;
    }

    public void DeleteKeyFromData(char i_Key)
    {
        m_ComputerDataForRevealedSlots.Remove(i_Key);
    }

    public void UpdateCardsInDictionary(Card i_Card1, Card i_Card2, char i_FirstCardKey, char i_SecondCardKey)
    {
        UpdateCardInDictionary(i_Card1, i_FirstCardKey);
        UpdateCardInDictionary(i_Card2, i_SecondCardKey);
    }

    public void UpdateCardInDictionary(Card i_Card, char i_CardKey)
    {
        bool addNewCard = false;

        if (m_ComputerDataForRevealedSlots.ContainsKey(i_CardKey))
        {
            addNewCard = (m_ComputerDataForRevealedSlots[i_CardKey])[0].IsEqualToSlot(i_Card);
        }
        else
        {
            m_ComputerDataForRevealedSlots[i_CardKey] = new List<Card>();
        }

        if (addNewCard == false)
        {
            m_ComputerDataForRevealedSlots[i_CardKey].Add(i_Card);
        }
    }
}

