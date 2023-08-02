using System;
using System.Collections;
using UnityEngine;

public class Platform : MonoBehaviour, IPlatform, IVisible
{
    public static event EventHandler<OnCubeGroundedArgs> OnCubeGrounded;

    [SerializeField] private Transform _gapAnchorPosition;
    [SerializeField] private int _scoreToAdd;

    private float _landingAnimationDuration = .1f;
    private float _downAnimationValue = .25f;

    public void OnGrounded(GameObject cube)
    {
        OnCubeGrounded?.Invoke(this, new OnCubeGroundedArgs(_scoreToAdd));
        cube.transform.SetParent(transform);
        StartCoroutine(LandingAnimationRoutine());
    }

    public void OnLeftGround(GameObject cube)
    {
        cube.transform.SetParent(null);
    }

    public Vector3 GetGapAnchorPosition() => _gapAnchorPosition.position;

    public void OnInvisible()
    {
        // PlatformPool.Instance.ReturnToPool(this);
    }

    private IEnumerator LandingAnimationRoutine()
    {
        float animationTimer = 0f;
        
        Vector3 initialPosition = transform.position;
        Vector3 targetPosition = initialPosition + Vector3.down * _downAnimationValue;

        // Downward animation
        while (animationTimer < _landingAnimationDuration)
        {
            animationTimer += Time.deltaTime;

            float animationProgress = animationTimer / _landingAnimationDuration;

            transform.position = Vector3.Lerp(initialPosition, targetPosition, InterpolateUtils.EaseInOut(animationProgress));
            yield return null;
        }

        animationTimer = 0f;

        // Return animation
        while (animationTimer < _landingAnimationDuration)
        {
            animationTimer += Time.deltaTime;

            float animationProgress = animationTimer / _landingAnimationDuration;

            transform.position = Vector3.Lerp(targetPosition, initialPosition, InterpolateUtils.EaseInOut(animationProgress));
            yield return null;
        }

        transform.position = initialPosition;
    }
}
