using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using UnityEngine;

[DisallowMultipleComponent]
public class GoogleLoginManager : MonoBehaviour
{
    private FirebaseAuth _auth;
    private FirebaseUser _user;

    public void Initialize()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                _auth = FirebaseAuth.DefaultInstance;
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {task.Result}");
            }
        });
    }

    private void LoginWithGoogle()
    {
        Credential credential = GoogleAuthProvider.GetCredential("<idToken>", "<accessToken>");

        // Входим с помощью Google
        _auth.SignInWithCredentialAsync(credential).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted && !task.IsFaulted)
            {
                _user = task.Result;
                Debug.Log($"Google Sign-In successful! User: {_user.DisplayName}, ID: {_user.UserId}");
                
                // Теперь ты можешь использовать _user.UserId для сохранения/загрузки highscore
            }
            else
            {
                Debug.LogError("Failed to log in with Google: " + task.Exception);
            }
        });
    }
}
