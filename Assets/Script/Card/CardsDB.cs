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
        [SerializeField] private GameObject _skillGameObject;
        [SerializeField] private GameObject _softSkillGameObject;
        [SerializeField] private GameObject _contextGameObject;
        [SerializeField] private GameObject _warningAlert;
        
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
            else
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
                    _textMesh.text = "Choisis le savoir-faire que tu recherches";
                    _skillGameObject.SetActive(false);
                    _softSkillGameObject.SetActive(false);
                    _contextGameObject.SetActive(false);
                    break;
                
                case CardType.Skill :
                    if (_cardDeck[0].cardData.Type == CardType.Empty
                        || _cardDeck[1].cardData.Type == CardType.Empty)
                    {
                        InstanciateWarningPanel();
                        return;
                    }
                    _listSkill.SetActive(false);
                    _listSoftSkill.SetActive(true);
                    _cardDeck[0].transform.parent.gameObject.SetActive(false);
                    _cardDeck[1].transform.parent.gameObject.SetActive(false);
                    
                    _cardDeck[2].transform.parent.gameObject.SetActive(true);
                    _cardDeck[3].transform.parent.gameObject.SetActive(true);
                    _textMesh.text = "Choisis le savoir-être que tu recherches";
                    _deckPhase = CardType.SoftSkill;
                    break;
            
                case CardType.SoftSkill :
                    if (_cardDeck[2].cardData.Type == CardType.Empty 
                        || _cardDeck[3].cardData.Type == CardType.Empty)
                    {
                        InstanciateWarningPanel();
                        return;
                    }
                    _listSoftSkill.SetActive(false);
                    _listContext.SetActive(true);
                    _cardDeck[2].transform.parent.gameObject.SetActive(false);
                    _cardDeck[3].transform.parent.gameObject.SetActive(false);
                    
                    _cardDeck[4].transform.parent.gameObject.SetActive(true);
                    _cardDeck[5].transform.parent.gameObject.SetActive(true);
                    _textMesh.text = "Choisis les contextes de travail que tu recherches";
                    _deckPhase = CardType.Context;
                    break;
            
                case CardType.Context :
                    if (_cardDeck[4].cardData.Type == CardType.Empty
                        || _cardDeck[5].cardData.Type == CardType.Empty)
                    {
                        InstanciateWarningPanel();
                        return;
                    }
                    _textMesh.text = "Voici tes cartes de recherche, est-ce qu'elles te conviennent ?";
                    _listContext.SetActive(false);
                    DisplayFinalDeck();
                    _skillGameObject.SetActive(true);
                    _softSkillGameObject.SetActive(true);
                    _contextGameObject.SetActive(true);
                    _deckPhase = CardType.Empty;
                    break;
            
                default:
                    break;
            }
        }

        public void InstanciateWarningPanel()
        {
            Canvas parentCanvas = gameObject.GetComponentInParent<Canvas>();

            if (parentCanvas == null)
            {
                Debug.LogError("Aucun Canvas trouvé dans les parents !");
                return;
            }

            Instantiate(_warningAlert, parentCanvas.transform);
        }
    }
}
