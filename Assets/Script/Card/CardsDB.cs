using UnityEngine;
using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using Unity.VisualScripting;

namespace CardRH
{
    public class CardsDB : MonoBehaviour
    {
        [SerializeField] private List<CardView> _cardDeck;
        public List<CardView> CardChoose { get =>_cardDeck; }

        [Header("UI Elem")]
        [SerializeField] private GameObject _listSkill;
        [SerializeField] private GameObject _listSoftSkill;
        [SerializeField] private GameObject _listContext;
        [SerializeField] private TextMeshProUGUI _textMesh;

        private CardType _deckPhase = CardType.Skill;
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
            if (oldCard.cardData.Type != _deckPhase) return;
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
                oldCard.cardData.Type = CardType.Empty;
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
        
        public void ChangePhase()
        {
            switch (_deckPhase)
            {
                case CardType.Skill :
                    if (_cardDeck[0].cardData.Type == CardType.Empty) return;
                    _listSkill.SetActive(false);
                    _listSoftSkill.SetActive(true);
                    GameObject skillParent = _cardDeck[0].transform.parent.gameObject;
                    skillParent.SetActive(false);
                    GameObject softParent = _cardDeck[1].transform.parent.gameObject;
                    softParent.SetActive(true);
                    _textMesh.text = "Choisie le savoir être que tu recherhce";
                    _deckPhase = CardType.SoftSkill;
                    break;
            
                case CardType.SoftSkill :
                    if (_cardDeck[1].cardData.Type == CardType.Empty) return;
                    _listSoftSkill.SetActive(false);
                    _listContext.SetActive(true);
                    GameObject softParent1 = _cardDeck[1].transform.parent.gameObject;
                    softParent1.SetActive(false);
                    GameObject contextParent = _cardDeck[2].transform.parent.gameObject;
                    contextParent.SetActive(true);
                    GameObject contextParent1 = _cardDeck[3].transform.parent.gameObject;
                    contextParent1.SetActive(true);
                    GameObject contextParent2 = _cardDeck[4].transform.parent.gameObject;
                    contextParent2.SetActive(true);
                    _textMesh.text = "Choisie les contexte de job que tu recherhce";
                    _deckPhase = CardType.Context;
                    break;
            
                case CardType.Context :
                    if (_cardDeck[2].cardData.Type == CardType.Empty
                        || _cardDeck[3].cardData.Type == CardType.Empty
                        || _cardDeck[4].cardData.Type == CardType.Empty) return;
                    _listContext.SetActive(false);
                    break;
            
                default:
                    break;
            }
        }
    }
}
