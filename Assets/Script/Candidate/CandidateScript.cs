using System;
using System.Collections;
using System.Collections.Generic;
using CardRH;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CandidateScript : MonoBehaviour
{
    [Header("SO")] 
    [SerializeField] private DeckCandidate _deckScript;

    [Header("Image")] 
    [SerializeField] private Image _imageCandidate;
    [SerializeField] private TextMeshProUGUI _nameCandidate;
    [SerializeField] private TextMeshProUGUI _ageCandidate;
    [SerializeField] private TextMeshProUGUI _carteCandidate;
    
    private CandidateSO _currentCandidate;
    private int _countNumberCard = 0;

    private void OnEnable()
    {
        _deckScript.NewGoldenDetected += BindCard;
    }

    private void OnDisable()
    {
        _deckScript.NewGoldenDetected -= BindCard;
    }

    public void ChangeCandidate(CandidateSO newCandidate)
    {
        _countNumberCard = 0;
        ClearDeck();
        _currentCandidate = newCandidate.CreateClone();
        DisplayInfo();
        _deckScript.InitDeck(_currentCandidate.CandidateDeck);
    }

    private void BindCard()
    {
        _countNumberCard++;
        DisplayInfo();
    }
    
    public void DisplayInfo()
    {
        _imageCandidate.sprite = _currentCandidate.Art;
        _nameCandidate.text = _currentCandidate.Name;
        _ageCandidate.text = _currentCandidate.Age;
        _carteCandidate.text = _countNumberCard.ToString();
    }
    
    public CandidateSO FinishWithCandidate()
    {
        if (_currentCandidate != null)
        {
            return SaveCandidate();
        }
        return null;
    }
    
    public void ClearDeck()
    {
        foreach (Transform child in _deckScript.gameObject.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in _deckScript._secondDeckTransform)
        {
            Destroy(child.gameObject);
        }
    }
    
    public CandidateSO SaveCandidate()
    {
        _currentCandidate.CandidateDeck.Clear();
        _currentCandidate.CandidateDeck = _deckScript.GetDeck();
        _currentCandidate.NumberCardInCommun = _countNumberCard;
        return _currentCandidate;
    }

    public void SetCandidateNull()
    {
        _currentCandidate = null;
    }
}
