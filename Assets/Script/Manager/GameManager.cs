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
        FindCandidat = 1,
        ChoosePlace = 2,
    }
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        public GamePhase CurrentPhase = GamePhase.DeckBuild;
        
        [Header("Canvas")]
        [SerializeField] private Canvas _canvasDeckBuild;
        [SerializeField] private Canvas _canvasPlaceChoose;
        [SerializeField] private Canvas _canvasCandidate;
        
        [Header("Deck Builder")]
        public CardsDB CardViewDeck;
        private List<CardSO> _cardDeck = new List<CardSO>();
        public List<CardSO> CardDeck { get => _cardDeck; }

        [FormerlySerializedAs("_allCandidates")]
        [Header("Candidate")] 
        [SerializeField] private List<CandidateSO> _candidateList;
        [FormerlySerializedAs("_candidate")] [SerializeField] private CandidateScript _candidateScript;
        private int _candidateCount = 0;
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
            
            _canvasDeckBuild.gameObject.SetActive(false);
            _canvasPlaceChoose.gameObject.SetActive(true);
            DisplayTime();
            CurrentPhase = GamePhase.ChoosePlace;
            ChangeCandidate();
        }

        public void EnterPlace(PlaceType newPlace)
        {
            _canvasPlaceChoose.gameObject.SetActive(false);
            _canvasCandidate.gameObject.SetActive(true);
            CurrentPhase = GamePhase.FindCandidat;
            
            _currentPlace = newPlace;
            ChangeCandidate();
        }

        public void LeavePlace()
        {
            //Finish and save the candidate
            CandidateSO oldCandidate = _candidateScript.FinishWithCandidate();
            if (oldCandidate != null)
            {
                SaveCandidate(oldCandidate);
            }
            _candidateScript.SetCandidateNull();
            
            //Change Canvas
            _canvasCandidate.gameObject.SetActive(false);
            _canvasPlaceChoose.gameObject.SetActive(true);
            CurrentPhase = GamePhase.ChoosePlace;
            
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
                    _candidateList[i] = toSave;
                    return;
                }
            }
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
    

