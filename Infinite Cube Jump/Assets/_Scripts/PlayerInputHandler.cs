using UnityEngine;

public class PlayerInputHandler : Singleton<PlayerInputHandler>
{
    protected override void Awake()
    {
        base.Awake();
    }

    public bool IsMouseButtonDownThisFrame() => Input.GetMouseButtonDown(0);
}
