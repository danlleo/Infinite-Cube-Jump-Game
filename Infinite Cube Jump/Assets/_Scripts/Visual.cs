using UnityEngine;

public class Visual : MonoBehaviour
{
    private void OnBecameInvisible()
    {
        if (transform.parent.TryGetComponent(out IVisible component)) 
        {
            component.OnInvisible();
        }
    }
}
