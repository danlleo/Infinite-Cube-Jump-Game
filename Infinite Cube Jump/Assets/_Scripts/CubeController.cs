using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class CubeController : MonoBehaviour
{
    public static event EventHandler OnCubeStartedJump;
    public static event EventHandler OnCubePerformedJump;

    [SerializeField] AnimationCurve _jumpingAnimationCurve;
    [SerializeField] GraphicRaycaster _uiRaycaster;

    [SerializeField] private float _jumpHeight = 2f;
    [SerializeField] private float _forwardSpeed = 5f;
    [SerializeField] private float _jumpDuration = 1f;
    
    private Coroutine _jumpCoroutine;

    private bool _canJump;
    
    private PointerEventData _pointerEventData;
    private EventSystem _eventSystem;

    private void Awake()
        => _canJump = true;

    private void Start()
    {
        _eventSystem = EventSystem.current;
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
        if (PlayerInputHandler.Instance.IsMouseButtonDownThisFrame() && _canJump && !IsPointerOverUI())
            Jump();
    }

    private bool IsPointerOverUI()
    {
        _pointerEventData = new PointerEventData(_eventSystem)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new();
        _uiRaycaster.Raycast(_pointerEventData, results);

        return results.Count > 0;
    }

    private void Jump()
    {
        if (_jumpCoroutine != null)
            return;

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

    private void Cube_OnCubeFell(object sender, EventArgs e)
    {
        _canJump = false;
    }
}
