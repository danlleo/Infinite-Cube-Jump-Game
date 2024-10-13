using UnityEngine;

public class PlayerInputHandler : Singleton<PlayerInputHandler>
{
    public bool IsMouseButtonDownThisFrame() => Input.GetMouseButtonDown(0);
}
