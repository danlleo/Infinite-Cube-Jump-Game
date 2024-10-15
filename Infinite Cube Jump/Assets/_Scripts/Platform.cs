using System;
using System.Collections;
using DG.Tweening;
using Extensions;
using Interfaces;
using UnityEngine;
using Utils;

[DisallowMultipleComponent]
public class Platform : MonoBehaviour, IPlatform
{
    private static readonly int s_transparencyValue = Shader.PropertyToID("_TransparencyValue");
    private const float CameraSafetyMargin = 300f;

    public static event EventHandler<OnCubeGroundedArgs> OnCubeGrounded;
    public static event EventHandler<OnPlatformDisappearedArgs> OnPlatformDisappeared;

    [SerializeField] private MeshRenderer _visualRenderer;
    [SerializeField] private Transform _gapAnchorPosition;
    [SerializeField] private int _scoreToAdd;

    private float _moveSpeed;
    private float _screenWidth;
    private readonly float _defaultSpeed = 2.7f;
    private readonly float _extraSpeed = 3.75f;
    private readonly float _landingAnimationDuration = .1f;
    private readonly float _downAnimationValue = .25f;

    private Color _platformColor;
    private PlatformLine _platformLine;
    private Camera _camera;
    private Vector3 _lineDirection;

    private bool _canMove;
    private bool _isLeading;
    private static readonly int s_color = Shader.PropertyToID("_Color");

    private void Awake()
    {
        _camera = Camera.main;
        _screenWidth = Screen.width;
        _canMove = true;
    }

    private void OnEnable()
    {
        Cube.OnCubeFell += Cube_OnCubeFell;
    }

    private void OnDisable()
    {
        Cube.OnCubeFell -= Cube_OnCubeFell;
    }

    private void Update()
    {
        MovePlatform();
        CheckOutOfBounds();
    }

    public void Initialize(PlatformLine platformLine, Vector3 lineDirection, Color platformColor, bool isLeading)
    {
        _platformLine = platformLine;
        _lineDirection = lineDirection;
        _platformColor = platformColor;
        _isLeading = isLeading;

        if (_lineDirection == Vector3.left)
            _moveSpeed = _extraSpeed;
        else if (_lineDirection == Vector3.right)
            _moveSpeed = _defaultSpeed;
        
        _visualRenderer.material.SetColor(s_color, _platformColor);
        SetInitialAlpha();
    }

    public void SetLeading()
        => _isLeading = true;
    
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

    public void FadeOut()
        => _visualRenderer.material.DOFloat(0f, s_transparencyValue, 0.3f);

    private void SetInitialAlpha()
        => _visualRenderer.material.SetFloat(s_transparencyValue, 1f);
    
    private void Cube_OnCubeFell(object sender, EventArgs e)
    {
        _canMove = false;
    }

    private void MovePlatform()
    {
        if (!_canMove) return;

        transform.position += _moveSpeed * Time.deltaTime * _lineDirection;
    }
    
    private void CheckOutOfBounds()
    {
        if (!_isLeading)
            return;

        Vector3 screenPosition = _camera.WorldToScreenPoint(transform.position);

        if (_lineDirection == Vector3.right)
        {
            // If the platform is behind the left side of the screen
            if (!(screenPosition.x < -CameraSafetyMargin)) return;
            
            _isLeading = false;
            transform.SetParent(null);
            OnPlatformDisappeared?.Invoke(_platformLine, new OnPlatformDisappearedArgs(this));
            PlatformPool.Instance.ReturnToPool(this);
                
            if (ComponentExtensions.TryGetComponentInChildren(gameObject, out Cube cube))
            {
                cube.TriggerCubeFellEvent();
            }
        }
        else if (_lineDirection == Vector3.left)
        {
            // If the platform is behind the right side of the screen
            if (!(screenPosition.x > _screenWidth + CameraSafetyMargin)) return;
            _isLeading = false;
            transform.SetParent(null);
            OnPlatformDisappeared?.Invoke(_platformLine, new OnPlatformDisappearedArgs(this));
            PlatformPool.Instance.ReturnToPool(this);

            if (ComponentExtensions.TryGetComponentInChildren(gameObject, out Cube cube))
            {
                cube.TriggerCubeFellEvent();
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
