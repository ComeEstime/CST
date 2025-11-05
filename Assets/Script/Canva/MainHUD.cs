using System.Collections.Generic;
using CardRH;
using UnityEngine;

public class MainHUD : MonoBehaviour
{
    [SerializeField] private List<CardView> _cards;

    public void SetCard(List<CardSO> cardInfo)
    {
        if (cardInfo.Count < 6) return;
        
        //1
        _cards[0].SetData(cardInfo[0]);
        _cards[0].UpdateCardUI();
        
        //2
        _cards[1].SetData(cardInfo[2]);
        _cards[1].UpdateCardUI();
        
        //3
        _cards[2].SetData(cardInfo[4]);
        _cards[2].UpdateCardUI();
        
        //4
        _cards[3].SetData(cardInfo[1]);
        _cards[3].UpdateCardUI();
        
        //5
        _cards[4].SetData(cardInfo[3]);
        _cards[4].UpdateCardUI();
        
        //6
        _cards[5].SetData(cardInfo[5]);
        _cards[5].UpdateCardUI();
    }
}
