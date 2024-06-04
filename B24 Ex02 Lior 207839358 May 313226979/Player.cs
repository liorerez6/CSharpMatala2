

using System;
using System.Collections.Generic;
using System.Linq;

class Player
{
    private int m_Score;
    private string m_Name; // should be readonly because it doesn't change during the game.
    private bool m_IsHumanPlayer = true;
    private Dictionary<char, List<(int, int)>> m_RevealedCards;


    public Player(string i_Name, bool humanIsPlaying)
    {
        m_Name = i_Name;
        m_Score = 0;
        m_IsHumanPlayer = humanIsPlaying;

        if(!m_IsHumanPlayer)
        {
            m_RevealedCards = new Dictionary<char, List<(int, int)>>();
            
        }

    }

    public Card SearchForAMattchingCard(char i_Key)
    {
        Card card = new Card((m_RevealedCards[i_Key])[0].Item1, (m_RevealedCards[i_Key])[0].Item2);
        
        return card;
    }

    public bool IsCardInMemoryRevealedCards(Card i_SearchForCard, char i_Key)
    {
        int row = i_SearchForCard.Row;
        int col = i_SearchForCard.Col;

        //need to change list to Card type
        return ((m_RevealedCards[i_Key])[0].Item1.Equals((row, col)));
    }
    

    public (Card, Card) CheckForMatchedCardsInMemoryList()
    {
        Card card1 = null;
        Card card2 = null;

        foreach (var card in m_RevealedCards)
        {
            if( card.Value.Count() == 2)
            {
                card1 = new Card(card.Value[0].Item1, card.Value[0].Item2);
                card2 = new Card(card.Value[1].Item1, card.Value[1].Item2);
                m_RevealedCards.Remove(card.Key);
                break;
            }
        }

        return (card1, card2);
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

    public void UpdateCardInDictonary(Card i_FirstCard, Card i_SecondCard, char i_KeyOfFirstChar, char i_KeyOfSecondChar)
    {
      
        if (!m_RevealedCards.ContainsKey(i_KeyOfFirstChar))
        {
            m_RevealedCards[i_KeyOfFirstChar] = new List<(int, int)>();
        }

        if (!m_RevealedCards.ContainsKey(i_KeyOfSecondChar))
        {
            m_RevealedCards[i_KeyOfSecondChar] = new List<(int, int)>();
        }


        if (!(m_RevealedCards[i_KeyOfFirstChar].Contains((i_FirstCard.Row, i_FirstCard.col))))
        {
            m_RevealedCards[i_KeyOfFirstChar].Add((i_FirstRowCard, i_FirstColCard));
        }

        if (!(m_RevealedCards[i_KeyOfSecondChar].Contains((i_SecondRowCard, i_SecondColCard))))
        {
            m_RevealedCards[i_KeyOfSecondChar].Add((i_SecondRowCard, i_SecondColCard));
        }


    }


    public void UpdateCardInDictonary(int i_FirstRowCard, int i_FirstColCard, int i_SecondRowCard, int i_SecondColCard, char i_FirstCard, char i_SecondCard)
    {


        if (!m_RevealedCards.ContainsKey(i_KeyOfFirstChar))
        {
            m_RevealedCards[i_KeyOfFirstChar] = new List<(int, int)>();
        }

        if (!m_RevealedCards.ContainsKey(i_KeyOfSecondChar))
        {
            m_RevealedCards[i_KeyOfSecondChar] = new List<(int, int)>();
        }


        if (!(m_RevealedCards[i_KeyOfFirstChar].Contains((i_FirstRowCard, i_FirstColCard))))
        {
            m_RevealedCards[i_KeyOfFirstChar].Add((i_FirstRowCard, i_FirstColCard));
        }

        if (!(m_RevealedCards[i_KeyOfSecondChar].Contains((i_SecondRowCard, i_SecondColCard))))
        {
            m_RevealedCards[i_KeyOfSecondChar].Add((i_SecondRowCard, i_SecondColCard));
        }


    }

}
