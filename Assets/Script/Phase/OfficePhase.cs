    using System.Collections.Generic;
    using CardRH;
    using UnityEngine;

public class OfficePhase : MonoBehaviour, IPhaseInterface
{
    [SerializeField] private CandidateOfficeView _candidatePrefab;
    [SerializeField] private Transform _transformCandidat;
    private List<CandidateSO> _candidateList = new List<CandidateSO>();
    
    public void EnterPhase()
    {
        _candidateList = GameManager.Instance.CandidateList;

        DisplayAllCandidate();
    }

    private void DisplayAllCandidate()
    {
        //Delete all child
        foreach (Transform child in _transformCandidat)
        {
            Destroy(child.gameObject);
        }
        
        
        foreach (var candidate in _candidateList)
        {
            if (candidate.HaveBeenSee)
            {
                Debug.Log(candidate.Name + " à " + candidate.NumberCardInCommun + " cartes");
                CandidateOfficeView instance = Instantiate(_candidatePrefab, _transformCandidat);
                instance.DisplayCandidate(candidate);
            }
        }
    }
    
    public void ExitPhase()
    {
        
    }

}
