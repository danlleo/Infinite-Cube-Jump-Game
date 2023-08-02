using System;
using System.Collections;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    public static event EventHandler OnCubeStartedJump;
    public static event EventHandler OnCubePerformedJump;

    [SerializeField] AnimationCurve _jumpingAnimationCurve;

    [SerializeField] private float _jumpHeight = 2f;
    [SerializeField] private float _forwardSpeed = 5f;
    [SerializeField] private float _jumpDuration = 1f;

    [SerializeField] private AudioClip _jumpSound;
    
    private Coroutine _jumpCoroutine;

    private void Update()
    {
        if (PlayerInputHandler.Instance.IsMouseButtonDownThisFrame())
            Jump();
    }

    private void Jump()
    {
        if (_jumpCoroutine != null)
            return;

        AudioSource.PlayClipAtPoint(_jumpSound, transform.position);
        OnCubeStartedJump?.Invoke(this, EventArgs.Empty);
        _jumpCoroutine = StartCoroutine(JumpRoutine());
    }

    private IEnumerator JumpRoutine()
    {
        Vector3 startPosition = transform.position;
        Vector3 forwardDirection = transform.forward;
        
        float jumpTimer = 0f;

        while (jumpTimer < _jumpDuration)
        {
            jumpTimer += Time.deltaTime;

            float jumpProgress = jumpTimer / _jumpDuration;
            float jumpHeightValue = _jumpingAnimationCurve.Evaluate(jumpProgress) * _jumpHeight;

            Vector3 position = startPosition + _forwardSpeed * jumpProgress * forwardDirection;
            position.y = startPosition.y + jumpHeightValue;
            transform.position = position;

            yield return null;
        }

        OnCubePerformedJump?.Invoke(this, EventArgs.Empty);

        // Ensure the final position is accurate
        Vector3 finalPosition = startPosition + forwardDirection * _forwardSpeed;
        finalPosition.y = startPosition.y;
        transform.position = finalPosition;

        _jumpCoroutine = null;
    }
}
