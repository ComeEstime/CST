using CardRH;
using UnityEngine;

public class PlaceScript : MonoBehaviour
{
    [SerializeField] private PlaceType _place;

    public void EnterPlace()
    {
        if(_place != PlaceType.None) GameManager.Instance.EnterPlace(_place);
    }

}
