using UnityEngine;

[DisallowMultipleComponent]
public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _objectToFollow;
    [SerializeField] private Vector3 _offset;

    private void LateUpdate()
    {
        Vector3 target = new Vector3(transform.position.x, transform.position.y, _objectToFollow.position.z) + _offset;
        transform.position = target;
    }
}
