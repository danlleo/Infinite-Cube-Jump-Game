using UnityEngine;

[DisallowMultipleComponent]
public class GameManager : MonoBehaviour
{
    private GoogleLoginManager _googleLoginManager;
    private FirestoreManager _firestoreManager;

    private void Start()
    {
        _googleLoginManager.Initialize();
        _firestoreManager.Initialize();
    }
}