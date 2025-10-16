using CardRH;
using TMPro;
using UnityEngine;

public class CardInfoScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _description;

    private CandidateSO _candidateInfo;
    
    public void DisplayInfo(string _descText)
    {
        _description.text = _descText;
    }

    public void SetInfo(CandidateSO newCandidate)
    {
        _candidateInfo = newCandidate;
        DisplayInfo(_candidateInfo.Description);
    }

    public void ValidCandidate()
    {
        if(_candidateInfo != null) GameManager.Instance.SeeCandidate(_candidateInfo);
        Destroy(gameObject);
    }
    
    public void DestroyInfo()
    {
        Destroy(gameObject);
    }
}
