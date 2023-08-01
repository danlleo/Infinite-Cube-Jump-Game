using UnityEngine;

public interface IPlatform
{
    void OnGrounded(GameObject cube);

    void OnLeftGround(GameObject cube);
}
