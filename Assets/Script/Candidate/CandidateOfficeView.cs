using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CandidateOfficeView : MonoBehaviour
{
    [SerializeField] private Image _imageCandidate; 
    [SerializeField] private TextMeshProUGUI _nameCandidate;
    [SerializeField] private List<Image> _stars;

    [Header("Prefab")] [SerializeField] private PanelOfficeInfo _cardInfoPrefab;

    private CandidateSO _candidate;

    public void DisplayCandidate(CandidateSO newCandidate)
    {
        _candidate = newCandidate;
        
        _imageCandidate.sprite = _candidate.HeadArt;
        _nameCandidate.text = _candidate.Name;

        DisplayStars();
    }

    private void DisplayStars()
    {
        if (_stars.Count < 6) return;
        
        switch (_candidate.NumberCardInCommun)
        {
            case 0 :
                break;
            
            case 1 :
                _stars[0].gameObject.SetActive(true);
                break;
            
            case 2 :
                _stars[1].gameObject.SetActive(true);
                goto case 1;
            
            case 3 :
                _stars[2].gameObject.SetActive(true);
                goto case 2;
            
            case 4 :
                _stars[3].gameObject.SetActive(true);
                goto case 3;
                    
            case 5 :
                _stars[4].gameObject.SetActive(true);
                goto case 4;
                        
            case 6 :
                _stars[5].gameObject.SetActive(true);
                goto case 5;
                            
            default:
                goto case 6;
        }
    }   

    public void DisplayPanel()
    {
        PanelOfficeInfo activeCard = Instantiate(_cardInfoPrefab, GameObject.Find("CanvasDeskop").transform);
        activeCard.SetInfo(_candidate);
    }
}
