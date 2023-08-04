using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private GameObject _scoreUI;
    [SerializeField] private TextMeshProUGUI _scoreText;

    private void OnEnable()
    {
        Platform.OnCubeGrounded += Platform_OnCubeGrounded;
    }

    private void OnDisable()
    {
        Platform.OnCubeGrounded -= Platform_OnCubeGrounded;
    }

    private void Platform_OnCubeGrounded(object sender, OnCubeGroundedArgs e)
    {
        Score.Add(e.ScoreToAdd);
        _scoreText.text = Score.Current.ToString();
    }
}
