using UnityEngine;

namespace Interfaces
{
    public interface IPlatform
    {
        void OnGrounded(GameObject cube);

        void OnLeftGround(GameObject cube);
    }
}
