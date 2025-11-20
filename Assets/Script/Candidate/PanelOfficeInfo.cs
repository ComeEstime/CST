using CardRH;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelOfficeInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _textCardCommun;
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _age;

    private CandidateSO _candidateInfo;
    
    public void DisplayInfo()
    {
        _name.text = _candidateInfo.Name;
        _textCardCommun.text = _candidateInfo.NumberCardInCommun.ToString();
        _image.sprite = _candidateInfo.HeadArt;
        _age.text = _candidateInfo.Age;
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
