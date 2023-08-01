using UnityEngine;

public class Cube : MonoBehaviour
{
    [SerializeField] private Transform _raycastPosition;

    [SerializeField] private float _raycastDistance;

    private IPlatform _platform;

    private void OnEnable()
    {
        CubeController.OnCubePerformedJump += CubeController_OnCubePerformedJump;
        CubeController.OnCubeStartedJump += CubeController_OnCubeStartedJump;
    }

    private void OnDisable()
    {
        CubeController.OnCubePerformedJump -= CubeController_OnCubePerformedJump;
        CubeController.OnCubeStartedJump -= CubeController_OnCubeStartedJump;
    }

    private void CubeController_OnCubeStartedJump(object sender, System.EventArgs e)
    {
        _platform?.OnLeftGround(gameObject);
    }

    private void CubeController_OnCubePerformedJump(object sender, System.EventArgs e)
    {
        if (Physics.Raycast(_raycastPosition.position, -transform.up, out RaycastHit hit, _raycastDistance))
        {
            if (hit.collider.TryGetComponent(out IPlatform platform))
            {
                _platform = platform;
                _platform.OnGrounded(gameObject);
            }
        }
        else
        {
            gameObject.AddComponent<Rigidbody>();
        }
    }
}
