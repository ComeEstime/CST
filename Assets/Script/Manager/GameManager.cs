using System.Collections.Generic;
using CardRH;
using NUnit.Framework.Constraints;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace CardRH
{
    public enum GamePhase
    {
        DeckBuild = 0,
        ChoosePlace = 1,
        MeetCandidate = 2,
        FindCandidat = 3,
        ChooseCandidate = 4
    }
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        public GamePhase CurrentPhase = GamePhase.DeckBuild;
        
        [Header("Canvas")]
        [SerializeField] private Canvas _canvasDeckBuild;
        [SerializeField] private Canvas _canvasPlaceChoose;
        [SerializeField] private Canvas _canvasMeetCandidate;
        [SerializeField] private Canvas _canvasCandidate;
        [SerializeField] private Canvas _canvasDeskop;
        
        [Header("Deck Builder")]
        public CardsDB CardViewDeck;
        private List<CardSO> _cardDeck = new List<CardSO>();
        public List<CardSO> CardDeck { get => _cardDeck; }

        [Header("Candidate")] 
        [SerializeField] private List<CandidateSO> _candidateList;
        public List<CandidateSO> CandidateList { get => _candidateList; }
        [SerializeField] private MeetCandidateScript _meetCandidate;
        [SerializeField] private GameObject _meetDeck;
        [SerializeField] private CandidateScript _candidateScript;
        [SerializeField] private CandidateDisplayScript _candidateDisplayScript;
        private PlaceType _currentPlace = PlaceType.None;
        
        
        
        [Header("Time")]
        [SerializeField] private int _timeRessource = 25;
        [SerializeField] private TextMeshProUGUI  _textTime;
        
        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
        }

        public void ValidDeck() 
        {
            List<CardView> tempCard = CardViewDeck.CardChoose;
            for (int i = 0; i < CardViewDeck.CardChoose.Count; i++)
            {
                if (!CardViewDeck.CardChoose[i].gameObject.activeSelf) { Debug.Log("Non frr tu as pas toute les cartes"); return; }
                _cardDeck.Add(CardViewDeck.CardChoose[i].cardData);
            }
            Debug.Log("OK c'est bon on passe à la suite");

            ChangeCanvas(GamePhase.ChoosePlace);
            
            DisplayTime();
            ChangeCandidate();
        }

        public void EnterPlace(PlaceType newPlace)
        {
            if (newPlace == PlaceType.Office)
            {
                ChangeCanvas(GamePhase.ChooseCandidate);
                
                _candidateDisplayScript.DisplayCandidate();
                return;
            }
            
            ChangeCanvas(GamePhase.MeetCandidate);
            
            _currentPlace = newPlace;
            DisplayMeetCandidate();
            //ChangeCandidate();
        }

        public void LeavePlace()
        {
            //Finish and save the candidate
            CandidateSO oldCandidate = _candidateScript.FinishWithCandidate();
            if (oldCandidate != null) SaveCandidate(oldCandidate);
            _candidateScript.SetCandidateNull();
            
            //Change Canvas
            ChangeCanvas(GamePhase.ChoosePlace);
            
            _currentPlace = PlaceType.None;
        }
        
        public void ChangeCandidate()
        {
            //Save le candidate qui vient d'être jouer
            CandidateSO oldCandidate = _candidateScript.FinishWithCandidate();
            if (oldCandidate != null)
            {
                SaveCandidate(oldCandidate);
            }
            
            //Trouver un nouveau candidat
            CandidateSO tempCandidate = FindCandidate();
            if(tempCandidate != null) _candidateScript.ChangeCandidate(tempCandidate.CreateClone());
            else Debug.Log("Tu n'a plus de candidat à voir dans ce lieu");
        }

        public void DisplayMeetCandidate()
        {
            foreach (Transform child in _meetDeck.transform)
            {
                Destroy(child.gameObject);
            }

            foreach (var c in _candidateList)
            {
                foreach (var cp in c.CandidatePlace)
                {
                    if (cp == _currentPlace & !c.HaveBeenSee)
                    {
                        MeetCandidateScript candidateMeet = Instantiate(_meetCandidate, _meetDeck.transform);
                        candidateMeet.SetCandidate(c);
                        break;
                    }
                }
            }
        }
        
        public CandidateSO FindCandidate()
        {
            foreach (var c in _candidateList)
            {
                foreach (var cp in c.CandidatePlace)
                {
                    if (cp == _currentPlace & !c.HaveBeenSee)
                    {
                        return c;
                    }
                }
            }

            return null;
        }

        public void SaveCandidate(CandidateSO toSave)
        {
            for (int i = 0; i < _candidateList.Count; i++)
            {
                if (_candidateList[i].Name == toSave.Name)
                {
                    foreach (var card in toSave.CandidateDeck)
                    {
                        if (card.IsGolden && card.InDeck)
                        {
                            toSave.NumberCardInCommun++;
                        }
                    }
                    toSave.HaveBeenSee = true;
                    _candidateList[i] = toSave;
                    return;
                }
            }
        }

        public void ChangeCanvas(GamePhase newPhase)
        {
            //Remove last canva
            switch (CurrentPhase)
            {
                case GamePhase.DeckBuild :
                    _canvasDeckBuild.gameObject.SetActive(false);
                    break;
                
                case GamePhase.ChoosePlace :
                    _canvasPlaceChoose.gameObject.SetActive(false);
                    break;
                
                case GamePhase.MeetCandidate :
                    _canvasMeetCandidate.gameObject.SetActive(false);
                    break;
                
                case GamePhase.FindCandidat :
                    _canvasCandidate.gameObject.SetActive(false);
                    break;
                
                case GamePhase.ChooseCandidate :
                    _canvasDeskop.gameObject.SetActive(false);
                    break;
            }
            
            //Display new Canva
            switch (newPhase)
            {
                case GamePhase.DeckBuild :
                    _canvasDeckBuild.gameObject.SetActive(true);
                    break;
                
                case GamePhase.ChoosePlace :
                    _canvasPlaceChoose.gameObject.SetActive(true);
                    break;
                
                case GamePhase.MeetCandidate :
                    _canvasMeetCandidate.gameObject.SetActive(true);
                    break;
                
                case GamePhase.FindCandidat :
                    _canvasCandidate.gameObject.SetActive(true);
                    break;
                
                case GamePhase.ChooseCandidate :
                    _canvasDeskop.gameObject.SetActive(true);
                    break;
            }

            CurrentPhase = newPhase;
        }
        
        //Time gestion
        public void AddTime(int timeAdded)
        {
            _timeRessource += timeAdded;
            DisplayTime();
        }

        public void RemoveTime(int timeRemoved)
        {
            _timeRessource -= timeRemoved;
            DisplayTime();
        }

        public void DisplayTime()
        {
            _textTime.text = _timeRessource.ToString();
        }
    }
}
    

