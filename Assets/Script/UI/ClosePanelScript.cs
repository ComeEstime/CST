using UnityEngine;

public class ClosePanelScript : MonoBehaviour
{
    [SerializeField] private GameObject _gameObject;
    
    public void DestroyInfo()
    {
        Destroy(_gameObject);
    }
}
