using UnityEngine;

public static class VibrationPlayer
{
    public static void TriggerVibration()
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            Handheld.Vibrate();
        }
    }
}
