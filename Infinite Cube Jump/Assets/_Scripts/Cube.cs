using System;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public static event EventHandler OnCubeFell;

    [SerializeField] private GameObject _scoredAnimationPrefab;
    [SerializeField] private Transform _scoredAnimationSpawnPosition;
    [SerializeField] private Transform _raycastPosition;

    [SerializeField] private float _raycastDistance;

    private IPlatform _platform;

    private void OnEnable()
    {
        CubeController.OnCubePerformedJump += CubeController_OnCubePerformedJump;
        CubeController.OnCubeStartedJump += CubeController_OnCubeStartedJump;
        Platform.OnCubeGrounded += Platform_OnCubeGrounded;
    }

    private void OnDisable()
    {
        CubeController.OnCubePerformedJump -= CubeController_OnCubePerformedJump;
        CubeController.OnCubeStartedJump -= CubeController_OnCubeStartedJump;
        Platform.OnCubeGrounded -= Platform_OnCubeGrounded;
    }

    public void TriggerCubeFellEvent()
        => OnCubeFell?.Invoke(this, EventArgs.Empty);

    private void Platform_OnCubeGrounded(object sender, OnCubeGroundedArgs e)
    {
        GameObject spawnedScoredAnimation = Instantiate(_scoredAnimationPrefab, _scoredAnimationSpawnPosition);
        spawnedScoredAnimation.transform.SetParent(null);

        if (spawnedScoredAnimation.TryGetComponent(out ScoredAnimation scoredAnimation))
            scoredAnimation.Initialize(e.ScoreToAdd.ToString());

        Destroy(spawnedScoredAnimation, 1f);
    }

    private void CubeController_OnCubeStartedJump(object sender, EventArgs e)
    {
        ResetPosition();
        _platform?.OnLeftGround(gameObject);
    }

    private void ResetPosition()
        => transform.position = new Vector3(transform.position.x, 1f, transform.position.z);

    private void CubeController_OnCubePerformedJump(object sender, EventArgs e)
    {
        if (Physics.Raycast(_raycastPosition.position, -transform.up, out RaycastHit hit, _raycastDistance))
        {
            // Hit the ground
            if (hit.collider.TryGetComponent(out IPlatform platform))
            {
                _platform = platform;
                _platform.OnGrounded(gameObject);
            }
        }
        else
        {
            // Missed
            gameObject.AddComponent<Rigidbody>();
            OnCubeFell?.Invoke(this, EventArgs.Empty);
            VibrationPlayer.TriggerVibration();
        }
    }
}
