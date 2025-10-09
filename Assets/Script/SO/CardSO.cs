using UnityEngine;
using System.Collections.Generic;

namespace CardRH
{
    [CreateAssetMenu(fileName = "Card", menuName = "Create Card", order = 0)]
    public class CardSO : ScriptableObject
    {
        public string Title = "New Card";
        public string Description = "Describe the card abilities";
        public int Cost=2;
        public CardType Type = CardType.Skill;
        public Sprite Border;
        public Sprite Art;
        public bool InDeck;
        public List<CardTrait> Traits = new List<CardTrait>();
        public List<PlaceType> Place = new List<PlaceType>();

        public CardSO CreateClone()
        {
            CardSO clone = ScriptableObject.CreateInstance<CardSO>();
            clone.Title = Title;
            clone.Description = Description;
            clone.Cost = Cost;
            clone.Type = Type;
            clone.Border = Border;
            clone.Art = Art;
            clone.InDeck = InDeck;
            clone.Traits = new List<CardTrait>(Traits);
            return clone;

        }
    }
}
