using System.Collections.Generic;
using CardRH;
using UnityEngine;

public class CandidateDisplayScript : MonoBehaviour
{
    [SerializeField] private CandidateView _candidatePrefab;

    public void DisplayCandidate()
    {
        foreach (var candidate in GameManager.Instance.CandidateList) //Peut être optimiser en demandant un attribut plutôt qu'une instance
        {
            if (candidate.HaveBeenSee)
            {
                CandidateView instance = Instantiate(_candidatePrefab, gameObject.transform);
                instance.NumberStars(candidate.NumberCardInCommun);
            }
        }
    }
}
