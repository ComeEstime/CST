using System.Collections.Generic;
using CardRH;
using UnityEngine;

public class DeckCandidate : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private CardView _prefabCard;
    [SerializeField] private List<CardSO> _hidden;

    [Header("UI")] [SerializeField] private Transform _secondDeckTransform;

    private List<CardSO> _deckCard = new List<CardSO>();

    public void InitDeck(List<CardSO> newDeck)
    {
        _deckCard.Clear();
        if (newDeck != null & newDeck.Count > 0)
        {
            int cardCount = 0;
            foreach (CardSO c in newDeck)
            {
                CardView instance = null;
                if(cardCount >= 6) instance = Instantiate(_prefabCard, _secondDeckTransform);
                else instance = Instantiate(_prefabCard, gameObject.transform);
                
                cardCount++;
                instance.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

                switch (c.Type)
                {
                    case CardType.Skill :
                        instance.SetData(_hidden[0].CreateClone());
                        break;
                    
                    case CardType.SoftSkill :
                        instance.SetData(_hidden[1].CreateClone());
                        break;
                    
                    case CardType.Context :
                        instance.SetData(_hidden[2].CreateClone());
                        break;
                }

                instance.cardData = c.CreateClone();
                            
                foreach (var gameModeCard in GameManager.Instance.CardDeck)
                {
                    if (gameModeCard.Description == c.Description)
                    {
                        instance.cardData.IsGolden = true;
                    }
                }
                _deckCard.Add(instance.cardData);
            }
        }
    }

    public List<CardSO> GetDeck()
    {
        return _deckCard;
    }
}
