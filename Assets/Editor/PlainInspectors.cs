#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(CandidateSO))]
public class CandidateSO_Inspector : Editor { public override void OnInspectorGUI() => DrawDefaultInspector(); }

[CustomEditor(typeof(CardRH.CardSO))]
public class CardSO_Inspector : Editor { public override void OnInspectorGUI() => DrawDefaultInspector(); }

[CustomEditor(typeof(CardRH.CardsDB))]
public class CardsDB_Inspector : Editor { public override void OnInspectorGUI() => DrawDefaultInspector(); }

[CustomEditor(typeof(CardRH.TraitsDB))]
public class TraitsDB_Inspector : Editor { public override void OnInspectorGUI() => DrawDefaultInspector(); }

[CustomEditor(typeof(CardRH.SpecialWords))]
public class SpecialWords_Inspector : Editor { public override void OnInspectorGUI() => DrawDefaultInspector(); }
#endif