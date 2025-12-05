using System.Collections;
using System.Collections.Generic;
using CardRH;
using NUnit.Framework.Constraints;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace CardRH
{
    public enum GamePhase
    {
        MainMenu = 100,
        Intro = -1,
        DeckBuild = 0,
        ChoosePlace = 1,
        MeetCandidate = 2,
        DiscoverCandidate = 3,
        ChooseCandidate = 4
    }
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        public GamePhase CurrentPhase = GamePhase.DeckBuild;
        
        [Header("Canvas")]
        [SerializeField] private Canvas _canvasMainMenu;
        [SerializeField] private Canvas _canvasIntro;
        [SerializeField] private Canvas _canvasDeckBuild;
        [SerializeField] private Canvas _canvasPlaceChoose;
        [SerializeField] private Canvas _canvasMeetCandidate;
        [SerializeField] private Canvas _canvasCandidate;
        [SerializeField] private Canvas _canvasDeskop;
        [SerializeField] private MainHUD _HUDGame;
        
        [Header("Sprite")]
        [SerializeField] private Sprite _imageSchool;
        [SerializeField] private Sprite _imageCooptation;
        [SerializeField] private Sprite _imageHall;
        [SerializeField] private List<Image> _background;
        
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
        [SerializeField] private OfficePhase _officeManager;
        private PlaceType _currentPlace = PlaceType.None;

        [Header("UI")] [SerializeField] private TextMeshProUGUI _textPlace;
        [SerializeField] private GameObject _cityGrid;
        [SerializeField] private GameObject _emptyCandidate;
        [SerializeField] private Animator _transition;
        private float _transitionTime = 1f;
        
        [Header("Time")]
        [SerializeField] private int _timeRessource = 25;
        [SerializeField] private TextMeshProUGUI  _textTime;
        
        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
        }

        public void StartGame()
        {
            StartCoroutine(LoadCanvas(GamePhase.Intro));
        }

        public void StartDeckChoice()
        {
            StartCoroutine(LoadCanvas(GamePhase.DeckBuild));
        }
        
        public void ValidDeck() 
        {
            List<CardView> tempCard = CardViewDeck.CardChoose;
            for (int i = 0; i < CardViewDeck.CardChoose.Count; i++)
            {
                if (!CardViewDeck.CardChoose[i].gameObject.activeSelf) return;
                _cardDeck.Add(CardViewDeck.CardChoose[i].cardData);
            }
            StartCoroutine(LoadCanvas(GamePhase.ChoosePlace, after: () =>
            { 
                _HUDGame.gameObject.SetActive(true);
                _HUDGame.SetCard(_cardDeck);
                DisplayTime();
            }));
        }

        public void EnterPlace(PlaceType newPlace)
        {
            if (newPlace == PlaceType.Office)
            {
                StartCoroutine(LoadCanvas(GamePhase.ChooseCandidate, after: () =>
                { 
                    _officeManager.EnterPhase();
                }));
                
                return;
            }

            _currentPlace = newPlace;

            DisplayPlaceName();
            StartCoroutine(LoadCanvas(GamePhase.MeetCandidate, after: DisplayMeetCandidate));
            
        }

        public void DisplayPlaceName()
        {
            switch (_currentPlace)
            {
                case PlaceType.School :
                    _textPlace.text = "École";
                    break;
                case PlaceType.Cooptation :
                    _textPlace.text = "Cooptation";
                    break;
                
                case PlaceType.Hall :
                    _textPlace.text = "Hall/Forum";
                    break;
                
                case PlaceType.SocialNetwork :
                    _textPlace.text = "Jobboard / Réseau sociaux";
                    break;
                
                case PlaceType.Alternative : 
                    _textPlace.text = "Sources alternatives";
                    break;
                
                default:
                    _textPlace.text = "Néant";
                    break;
            }
        }
        
        public void StayPlace()
        {
            //Finish and save the candidate
            CandidateSO oldCandidate = _candidateScript.FinishWithCandidate();
            if (oldCandidate != null) SaveCandidate(oldCandidate);
            _candidateScript.SetCandidateNull();
            
            EnterPlace(_currentPlace);
        }

        public void LeavePlace()
        {
            //Finish and save the candidate
            CandidateSO oldCandidate = _candidateScript.FinishWithCandidate();
            if (oldCandidate != null) SaveCandidate(oldCandidate);
            _candidateScript.SetCandidateNull();
            
            //Change Canvas
            StartCoroutine(LoadCanvas(GamePhase.ChoosePlace, after: () =>
            { 
                _currentPlace = PlaceType.None;
            }));
            
            _currentPlace = PlaceType.None;
        }
        
        public void DisplayMeetCandidate()
        {
            foreach (Transform child in _meetDeck.transform)
            {
                Destroy(child.gameObject);
            }
            _emptyCandidate.gameObject.SetActive(false);

            int countCandidate = 0;
            foreach (var c in _candidateList)
            {
                foreach (var cp in c.CandidatePlace)
                {
                    if (cp == _currentPlace & !c.HaveBeenSee)
                    {
                        MeetCandidateScript candidateMeet = Instantiate(_meetCandidate, _meetDeck.transform);
                        candidateMeet.SetCandidate(c);
                        countCandidate++;
                        break;
                    }
                }
            }

            if (countCandidate == 0) _emptyCandidate.gameObject.SetActive(true);
        }

        public void SeeCandidate(CandidateSO tempCandidate)
        {
            StartCoroutine(LoadCanvas(GamePhase.DiscoverCandidate, after: () =>
            { 
                _candidateScript.ChangeCandidate(tempCandidate.CreateClone());
            }));
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

        
        IEnumerator LoadCanvas(GamePhase newPhase, System.Action after = null)
        {
            _transition.SetTrigger("Start");

            yield return new WaitForSeconds(_transitionTime);
        
            ChangeCanvas(newPhase);
            
            after?.Invoke();
        }
        
        public void ChangeCanvas(GamePhase newPhase)
        {
            //Remove last canva
            switch (CurrentPhase)
            {
                case GamePhase.MainMenu :
                    _canvasMainMenu.gameObject.SetActive(false);
                    break;
                
                case GamePhase.Intro :
                    _canvasIntro.gameObject.SetActive(false);
                    break;
                
                case GamePhase.DeckBuild :
                    _canvasDeckBuild.gameObject.SetActive(false);
                    break;
                
                case GamePhase.ChoosePlace :
                    _canvasPlaceChoose.gameObject.SetActive(false);
                    _cityGrid.SetActive(false);
                    break;
                
                case GamePhase.MeetCandidate :
                    _canvasMeetCandidate.gameObject.SetActive(false);
                    break;
                
                case GamePhase.DiscoverCandidate :
                    _canvasCandidate.gameObject.SetActive(false);
                    break;
                
                case GamePhase.ChooseCandidate :
                    _canvasDeskop.gameObject.SetActive(false);
                    break;
            }
            
            //Display new Canva
            switch (newPhase)
            {
                case GamePhase.MainMenu :
                    _canvasMainMenu.gameObject.SetActive(true);
                    break;
                
                case GamePhase.Intro :
                    _canvasIntro.gameObject.SetActive(true);
                    break;
                
                case GamePhase.DeckBuild :
                    _canvasDeckBuild.gameObject.SetActive(true);
                    break;
                
                case GamePhase.ChoosePlace :
                    _canvasPlaceChoose.gameObject.SetActive(true);
                    _cityGrid.SetActive(true);
                    break;
                
                case GamePhase.MeetCandidate :
                    _canvasMeetCandidate.gameObject.SetActive(true);
                    ChangeBackground();
                    break;
                
                case GamePhase.DiscoverCandidate :
                    _canvasCandidate.gameObject.SetActive(true);
                    break;
                
                case GamePhase.ChooseCandidate :
                    _canvasDeskop.gameObject.SetActive(true);
                    break;
            }

            CurrentPhase = newPhase;
        }

        public void ChangeBackground()
        {
            switch (_currentPlace)
            {
                case PlaceType.School :
                    foreach (Image bg in _background)
                    {
                        bg.sprite = _imageSchool;
                    }
                    break;
                
                case PlaceType.Cooptation :
                    foreach (Image bg in _background)
                    {
                        bg.sprite = _imageCooptation;
                    }
                    break;
                
                case PlaceType.Hall :
                    foreach (Image bg in _background)
                    {
                        bg.sprite = _imageHall;
                    }
                    break;
                    
            }
        }
        
        //Time gestion
        public bool CanDisplayCard()
        {
            return _timeRessource > 0;
        }
        
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
    

