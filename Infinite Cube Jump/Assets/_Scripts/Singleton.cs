using UnityEngine;

[DisallowMultipleComponent]
public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError($"There is more than one singleton: {Instance.name}");
            Destroy(gameObject);
            return;
        }

        Instance = this as T;
    }
}