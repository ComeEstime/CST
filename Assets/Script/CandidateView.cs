using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CandidateView : MonoBehaviour
{
    [SerializeField] private Image _imageCandidate; 
    [SerializeField] private List<Image> _stars;
    [SerializeField] private TextMeshProUGUI _nameCandidate;

    public void NumberStars(int numberStars)
    {
        switch (numberStars)
        {
            case 1 :
                _stars[0].gameObject.SetActive(true);
                _stars[1].gameObject.SetActive(false);
                _stars[2].gameObject.SetActive(false);
                break;
            
            case 2 :
                _stars[0].gameObject.SetActive(false);
                _stars[1].gameObject.SetActive(true);
                _stars[2].gameObject.SetActive(true);
                break;
            
            case 3 :
                _stars[0].gameObject.SetActive(true);
                _stars[1].gameObject.SetActive(true);
                _stars[2].gameObject.SetActive(true);
                break;
            
            default:
                _stars[0].gameObject.SetActive(false);
                _stars[1].gameObject.SetActive(false);
                _stars[2].gameObject.SetActive(false);
                break;
        }
    }
}
