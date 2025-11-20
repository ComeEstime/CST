using UnityEngine;
using System.Collections.Generic;

namespace CardRH
{
    [CreateAssetMenu(fileName = "TraitsDB", menuName = "Create TraitDB", order = 4)]
    public class TraitsDB : ScriptableObject
    {
        static TraitsDB instance;

        public static TraitsDB Instance
        {
            get { 
                if (instance == null)
                {
                    instance = Resources.Load<TraitsDB>("TraitsDB");
                }
                return instance; 
            }
        }

        public List<CardTrait> Traits;
        
#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
        static void __InitEditor_TraitsDB()
        {
            if (instance == null)
                instance = Resources.Load<TraitsDB>("TraitsDB");
        }
#endif

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void __InitRuntime_TraitsDB()
        {
            if (instance == null)
                instance = Resources.Load<TraitsDB>("TraitsDB");
        }

    }
}
