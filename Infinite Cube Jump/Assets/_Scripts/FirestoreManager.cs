using System.Collections.Generic;
using Firebase.Extensions;
using Firebase.Firestore;
using UnityEngine;

[DisallowMultipleComponent]
public class FirestoreManager : MonoBehaviour
{
    private FirebaseFirestore _database;
    
    public void Initialize()
        => _database = FirebaseFirestore.DefaultInstance;

    public void SaveHighScore(string playerId, int score)
    {
        Dictionary<string, object> playerData = new()
        {
            { "playerId", playerId },
            { "score", score }
        };

        _database.Collection("highscores").Document(playerId).SetAsync(playerData).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Highscores saved successfully");
                return;
            }

            Debug.LogError("Failed to save highscore: " + task.Exception);
        });
    }
}