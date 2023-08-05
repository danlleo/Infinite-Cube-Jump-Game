using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    protected override void Awake()
    {
        base.Awake();
    }

    public void StopTime() => Time.timeScale = 0f;

    public void ResumeTime() => Time.timeScale = 1f;
}
