using System.Collections.Generic;
using UnityEngine;


namespace CardRH
{
    [CreateAssetMenu(fileName = "ImagesDB", menuName = "Create ImagesDB", order = 1)]
    public class ImagesDB : ScriptableObject
    {
        public int Count { get { return images.Count; } }
        public List<Sprite> images;
    }
}
