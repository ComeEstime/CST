using UnityEngine;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;

namespace CardRH
{
    public class CardsDB : MonoBehaviour
    {
        [SerializeField] private List<CardView> _cardDeck;
        public List<CardView> CardChoose { get =>_cardDeck; }

        public void AddCard(CardView newCard)
        {
            switch (newCard.cardData.Type)
            {
                case CardType.Skill:
                    _cardDeck[0].cardData = newCard.cardData.CreateClone();
                    _cardDeck[0].cardData.InDeck = true;
                    _cardDeck[0].gameObject.SetActive(true);
                    break;
                
                case CardType.SoftSkill:
                    _cardDeck[1].cardData = newCard.cardData.CreateClone();
                    _cardDeck[1].cardData.InDeck = true;
                    _cardDeck[1].gameObject.SetActive(true);
                    break;
                
                case CardType.Context:
                    foreach (CardView card in _cardDeck) { if (newCard.cardData.Description == card.cardData.Description) return; }
                    if (!_cardDeck[2].gameObject.activeSelf)
                    {
                        _cardDeck[2].cardData = newCard.cardData.CreateClone();
                        _cardDeck[2].cardData.InDeck = true;
                        _cardDeck[2].gameObject.SetActive(true);
                    }
                    else if (!_cardDeck[3].gameObject.activeSelf)
                    {
                        _cardDeck[3].cardData = newCard.cardData.CreateClone();
                        _cardDeck[3].cardData.InDeck = true;
                        _cardDeck[3].gameObject.SetActive(true);
                    }
                    else if (!_cardDeck[4].gameObject.activeSelf)
                    {
                        _cardDeck[4].cardData = newCard.cardData.CreateClone();
                        _cardDeck[4].cardData.InDeck = true;
                        _cardDeck[4].gameObject.SetActive(true);
                    }
                    else
                    {
                        _cardDeck[4].cardData = newCard.cardData.CreateClone();
                        _cardDeck[4].cardData.InDeck = true;
                        _cardDeck[4].gameObject.SetActive(true);
                    }
                    break;
                
                default:
                    break;
            }

            DisplayDeck();
        }

        public void RemoveCard(CardView oldCard)
        {
            int temp = -1 ;
            for(int i = 0; i < _cardDeck.Count; i++)
            {
                if (_cardDeck[i] == oldCard)
                {
                    temp = i;
                    break;
                }
            }

            if (temp >= 0)
            {
                oldCard.cardData.Description = null;
                _cardDeck[temp].gameObject.SetActive(false);
            }
        }

        public void DisplayDeck()
        {
            foreach (CardView view in _cardDeck)
            {
                view.UpdateCardUI();
            }
        }
    }
}
