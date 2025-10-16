using System.Collections.Generic;
using CardRH;
using UnityEngine;

public class MainHUD : MonoBehaviour
{
    [SerializeField] private List<CardView> _cards;

    public void SetCard(List<CardSO> cardInfo)
    {
        for (int i = 0; i < _cards.Count; i++)
        {
            _cards[i].SetData(cardInfo[i]);
            _cards[i].UpdateCardUI();
        }
    }
}
