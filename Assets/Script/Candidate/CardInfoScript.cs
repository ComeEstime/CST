using CardRH;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardInfoScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _description;
    [SerializeField] private Image _image;

    private CandidateSO _candidateInfo;
    
    public void DisplayInfo()
    {
        _name.text = _candidateInfo.Name;
        _description.text = _candidateInfo.Description;
        _image.sprite = _candidateInfo.HeadArt;
    }

    public void SetInfo(CandidateSO newCandidate)
    {
        _candidateInfo = newCandidate;
        DisplayInfo();
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
