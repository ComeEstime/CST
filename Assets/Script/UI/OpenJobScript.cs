using UnityEngine;

public class OpenJobScript : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;

    public void OpenJob()
    {
        GameObject temp = Instantiate(_prefab, GameObject.Find("CanvasChooseDeck").transform);
    }
}
