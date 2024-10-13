using TMPro;
using UnityEngine;

[DisallowMultipleComponent]
public class ScoredAnimation : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoredText;

    public void Initialize(string scoreText)
        => _scoredText.text = $"+{scoreText}";
}
