using System;
using System.Collections;
using UnityEngine;

public class Platform : MonoBehaviour, IPlatform
{
    private const float CAMERA_SAFETY_MARGIN = 300f;

    public static event EventHandler<OnCubeGroundedArgs> OnCubeGrounded;
    public static event EventHandler<OnPlatformDisappearedArgs> OnPlatformDisappeared;

    [SerializeField] private Transform _gapAnchorPosition;
    [SerializeField] private int _scoreToAdd;

    private float _screenWidth;
    private float _moveSpeed = 2.5f;
    private float _landingAnimationDuration = .1f;
    private float _downAnimationValue = .25f;

    private PlatformLine _platformLine;
    private Camera _camera;
    private Vector3 _lineDirection;

    private void Awake()
    {
        _camera = Camera.main;
        _screenWidth = Screen.width;
    }

    private void Update()
    {
        MovePlatform();
        CheckOutOfBounds();
    }

    public void Initalize(PlatformLine platformLine, Vector3 lineDirection)
    {
        _platformLine = platformLine;
        _lineDirection = lineDirection;
    }

    public void OnGrounded(GameObject cube)
    {
        OnCubeGrounded?.Invoke(this, new OnCubeGroundedArgs(_scoreToAdd, _platformLine));
        cube.transform.SetParent(transform);
        StartCoroutine(LandingAnimationRoutine());
    }

    public void OnLeftGround(GameObject cube)
    {
        cube.transform.SetParent(null);
    }

    public Vector3 GetGapAnchorPosition() => _gapAnchorPosition.position;

    private void MovePlatform()
        => transform.position += _moveSpeed * Time.deltaTime * _lineDirection;
    
    private void CheckOutOfBounds()
    {
        Vector3 screenPosition = _camera.WorldToScreenPoint(transform.position);

        if (_lineDirection == Vector3.right)
        {
            // If the platform is behind the left side of the screen
            if (screenPosition.x < -CAMERA_SAFETY_MARGIN)
            {
                transform.SetParent(null);
                OnPlatformDisappeared?.Invoke(_platformLine, new OnPlatformDisappearedArgs(this));
                PlatformPool.Instance.ReturnToPool(this);
            }   
        }
        else if (_lineDirection == Vector3.left)
        {
            // If the platform is behind the right side of the screen
            if (screenPosition.x > _screenWidth + CAMERA_SAFETY_MARGIN)
            {
                transform.SetParent(null);
                OnPlatformDisappeared?.Invoke(_platformLine, new OnPlatformDisappearedArgs(this));
                PlatformPool.Instance.ReturnToPool(this);
            }
        }
    }

    private IEnumerator LandingAnimationRoutine()
    {
        float animationTimer = 0f;
        
        float initialHeight = transform.position.y;

        // Downward animation
        while (animationTimer < _landingAnimationDuration)
        {
            animationTimer += Time.deltaTime;

            float animationProgress = animationTimer / _landingAnimationDuration;

            transform.position = new Vector3(transform.position.x, 
                Mathf.Lerp(initialHeight, -_downAnimationValue, InterpolateUtils.EaseInOut(animationProgress)), 
                transform.position.z);

            yield return null;
        }

        animationTimer = 0f;

        // Return animation
        while (animationTimer < _landingAnimationDuration)
        {
            animationTimer += Time.deltaTime;

            float animationProgress = animationTimer / _landingAnimationDuration;

            transform.position = new Vector3(transform.position.x,
                Mathf.Lerp(-_downAnimationValue, initialHeight, InterpolateUtils.EaseInOut(animationProgress)),
                transform.position.z);

            yield return null;
        }

        transform.position = new Vector3(transform.position.x, initialHeight, transform.position.z);
    }
}
