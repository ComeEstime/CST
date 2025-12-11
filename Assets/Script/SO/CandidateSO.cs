using System.Collections.Generic;
using CardRH;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "CandidateSO", menuName = "Scriptable Objects/CandidateSO")]
public class CandidateSO : ScriptableObject
{
    public string Name = "CharacterName";
    public string Age = "CharacterAge";
    public string Description = "None";
    public Sprite Art;
    public Sprite HeadArt;
    public List<CardSO> CandidateDeck = new List<CardSO>();
    public List<PlaceType> CandidatePlace = new List<PlaceType>();
    public bool HaveBeenSee = false;
    [FormerlySerializedAs("NumberCardInDeck")] public int NumberCardInCommun = 0;
    
    public CandidateSO CreateClone()
    {
        CandidateSO clone = ScriptableObject.CreateInstance<CandidateSO>();
        clone.Name = Name;
        clone.Age = Age;
        clone.Description = Description;
        clone.Art = Art;
        clone.HeadArt = HeadArt;
        clone.CandidateDeck = new List<CardSO>();
        
        foreach (var card in CandidateDeck)
        {
            clone.CandidateDeck.Add(card.CreateClone());
        }
        clone.CandidatePlace = new List<PlaceType>(CandidatePlace);
        clone.HaveBeenSee = HaveBeenSee;
        clone.NumberCardInCommun = NumberCardInCommun;
        return clone;
    }
}
