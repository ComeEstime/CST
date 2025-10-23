using System.Collections;
using System.Collections.Generic;
using CardRH;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CandidateScript : MonoBehaviour
{
    [Header("SO")] 
    [SerializeField] private DeckCandidate _deckScript;

    [Header("Image")] 
    [SerializeField] private Image _imageCandidate;
    
    private CandidateSO _currentCandidate;
    

    public void ChangeCandidate(CandidateSO newCandidate)
    {
        ClearDeck();
        _currentCandidate = newCandidate;
        _imageCandidate.sprite = _currentCandidate.Art;
        _deckScript.InitDeck(_currentCandidate.CandidateDeck);
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
    }
    
    public CandidateSO SaveCandidate()
    {
        _currentCandidate.CandidateDeck.Clear();
        _currentCandidate.CandidateDeck = _deckScript.GetDeck();
        return _currentCandidate;
    }

    public void SetCandidateNull()
    {
        _currentCandidate = null;
    }
}
