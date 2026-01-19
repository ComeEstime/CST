using CardRH;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PanelOfficeInfo : MonoBehaviour
{
    [SerializeField] private CardView _prefab;
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _textCardCommun;
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _age;
    [SerializeField] private GameObject _box;

    private CandidateSO _candidateInfo;
    
    public void DisplayInfo()
    {
        _name.text = _candidateInfo.Name;
        _textCardCommun.text = _candidateInfo.NumberCardInCommun.ToString();
        _image.sprite = _candidateInfo.HeadArt;
        _age.text = _candidateInfo.Age;

        foreach (var card in _candidateInfo.CandidateDeck)
        {
            if (card.IsGolden && card.InDeck)
            {
                CardView newCard = Instantiate(_prefab, _box.transform);
                newCard.cardData = card.CreateClone();
                newCard.transform.localScale = new Vector3(0.7f,0.7f,0.7f);
                newCard.cardData.IsGolden = false;
                newCard.cardData.Interactable = false;

                newCard.UpdateCardUI();
            }
        }
    }

    public void SetInfo(CandidateSO newCandidate)
    {
        _candidateInfo = newCandidate;
        DisplayInfo();
    }

    public void ValidCandidate()
    {
        if(_candidateInfo != null) GameManager.Instance.FindEnd(_candidateInfo);
        Destroy(gameObject);
    }
    
    public void DestroyInfo()
    {
        Destroy(gameObject);
    }
}
