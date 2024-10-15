using UnityEngine;

public class TimeManager : Singleton<TimeManager>
{
    public void StopTime() => Time.timeScale = 0f;

    public void ResumeTime() => Time.timeScale = 1f;
}