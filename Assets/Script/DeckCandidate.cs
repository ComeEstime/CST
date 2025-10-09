using System.Collections.Generic;
using CardRH;
using UnityEngine;

public class DeckCandidate : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private CardView _prefabCard;
    [SerializeField] private CardSO  _empty;

    private List<CardSO> _deckCard = new List<CardSO>();
    

    public void InitDeck(List<CardSO> newDeck)
    {
        _deckCard.Clear();
        if (newDeck != null & newDeck.Count > 0)
        {
            foreach (CardSO c in newDeck)
            {
                CardView instance = Instantiate(_prefabCard, gameObject.transform);
                instance.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

                instance.SetData(_empty.CreateClone());

                instance.cardData = c.CreateClone();
                _deckCard.Add(instance.cardData);
            }
        }
    }

    public List<CardSO> GetDeck()
    {
        return _deckCard;
    }
}
