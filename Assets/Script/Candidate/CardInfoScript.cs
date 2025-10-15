using TMPro;
using UnityEngine;

public class CardInfoScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _description;

    public void DisplayInfo(string _descText)
    {
        _description.text = _descText;
    }
}
