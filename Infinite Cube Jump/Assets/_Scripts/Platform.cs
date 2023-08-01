using UnityEngine;

public class Platform : MonoBehaviour, IPlatform
{
    [SerializeField] private int _scoreToAdd;

    public void OnGrounded(GameObject cube)
    {
        cube.transform.SetParent(transform);       
    }

    public void OnLeftGround(GameObject cube)
    {
        cube.transform.SetParent(null);
    }
}
