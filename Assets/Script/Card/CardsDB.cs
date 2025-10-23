using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

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
        [SerializeField] private GameObject  _finalDeck;
        [SerializeField] private TextMeshProUGUI _textMesh;
        
        [Header("Button")]
        [SerializeField] private Button _buttonStart;
        [SerializeField] private Button _buttonRefoundDeck;
        [SerializeField] private Button _nextChoice;

        private CardType _deckPhase = CardType.Skill;
        public void AddCard(CardView newCard)
        {
            foreach (CardView card in _cardDeck) { if (newCard.cardData.Description == card.cardData.Description) return; }
            int temp1 = 0;
            int temp2 = 0;
            switch (newCard.cardData.Type)
            {
                case CardType.Skill:
                    temp1 = 0;
                    temp2 = 1;
                    break;
                
                case CardType.SoftSkill:
                    temp1 = 2;
                    temp2 = 3;
                    break;
                
                case CardType.Context:
                    temp1 = 4;
                    temp2 = 5;
                    /*if (!_cardDeck[2].gameObject.activeSelf)
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
                    }*/
                    break;
                
                default:
                    break;
            }


            if (!_cardDeck[temp1].gameObject.activeSelf)
            {
                _cardDeck[temp1].cardData = newCard.cardData.CreateClone();
                _cardDeck[temp1].cardData.InDeck = true;
                _cardDeck[temp1].gameObject.SetActive(true);
            }
            else /*if(!_cardDeck[temp2].gameObject.activeSelf)*/
            {
                _cardDeck[temp2].cardData = newCard.cardData.CreateClone();
                _cardDeck[temp2].cardData.InDeck = true;
                _cardDeck[temp2].gameObject.SetActive(true);
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

        public void DisplayFinalDeck()
        {
            foreach (var card in _cardDeck)
            {
                card.transform.parent.SetParent(_finalDeck.transform);
                card.transform.parent.gameObject.SetActive(true);
            }
            _finalDeck.SetActive(true);
            
            _buttonStart.gameObject.SetActive(true);
            _buttonRefoundDeck.gameObject.SetActive(true);
            _nextChoice.gameObject.SetActive(false);
        }

        public void RefoundDeck()
        {
            foreach (var card in _cardDeck)
            {
                card.transform.parent.SetParent(transform);
                card.transform.parent.gameObject.SetActive(false);
            }
            
            _buttonStart.gameObject.SetActive(false);
            _buttonRefoundDeck.gameObject.SetActive(false);
            _nextChoice.gameObject.SetActive(true);
            
            ChangePhase();
        }
        
        public void ChangePhase()
        {
            switch (_deckPhase)
            {
                case CardType.Empty :
                    _listSkill.SetActive(true);
                    _cardDeck[0].transform.parent.gameObject.SetActive(true);
                    _cardDeck[1].transform.parent.gameObject.SetActive(true);
                    _deckPhase = CardType.Skill;
                    _textMesh.text = "Choisie le savoir faire que tu recherhce";
                    break;
                
                case CardType.Skill :
                    if (_cardDeck[0].cardData.Type == CardType.Empty 
                        || _cardDeck[1].cardData.Type == CardType.Empty) return;
                    _listSkill.SetActive(false);
                    _listSoftSkill.SetActive(true);
                    _cardDeck[0].transform.parent.gameObject.SetActive(false);
                    _cardDeck[1].transform.parent.gameObject.SetActive(false);
                    
                    _cardDeck[2].transform.parent.gameObject.SetActive(true);
                    _cardDeck[3].transform.parent.gameObject.SetActive(true);
                    _textMesh.text = "Choisie le savoir être que tu recherhce";
                    _deckPhase = CardType.SoftSkill;
                    break;
            
                case CardType.SoftSkill :
                    if (_cardDeck[2].cardData.Type == CardType.Empty 
                        || _cardDeck[3].cardData.Type == CardType.Empty) return;
                    _listSoftSkill.SetActive(false);
                    _listContext.SetActive(true);
                    _cardDeck[2].transform.parent.gameObject.SetActive(false);
                    _cardDeck[3].transform.parent.gameObject.SetActive(false);
                    
                    _cardDeck[4].transform.parent.gameObject.SetActive(true);
                    _cardDeck[5].transform.parent.gameObject.SetActive(true);
                    _textMesh.text = "Choisie les contexte de job que tu recherhce";
                    _deckPhase = CardType.Context;
                    break;
            
                case CardType.Context :
                    if (_cardDeck[4].cardData.Type == CardType.Empty
                        || _cardDeck[5].cardData.Type == CardType.Empty) return;
                    _textMesh.text = "Voici ton deck, est ce que il te convient?";
                    _listContext.SetActive(false);
                    DisplayFinalDeck();
                    _deckPhase = CardType.Empty;
                    break;
            
                default:
                    break;
            }
        }
    }
}
