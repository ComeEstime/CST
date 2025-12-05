using UnityEngine;

public class WarningAnimationEnd : MonoBehaviour
{
    public void AnimationEnd()
    {
        //gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
