using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private GameObject _scoreUI;
    [SerializeField] private TextMeshProUGUI _scoreText;

    private void Awake()
    {
        ShowUI();
    }

    private void OnEnable()
    {
        Platform.OnCubeGrounded += Platform_OnCubeGrounded;
        Cube.OnCubeFell += Cube_OnCubeFell;
    }

    private void OnDisable()
    {
        Platform.OnCubeGrounded -= Platform_OnCubeGrounded;
        Cube.OnCubeFell -= Cube_OnCubeFell;
    }

    private void Cube_OnCubeFell(object sender, System.EventArgs e)
    {
        HideUI();
    }

    private void Platform_OnCubeGrounded(object sender, OnCubeGroundedArgs e)
    {
        Score.Add(e.ScoreToAdd);
        _scoreText.text = Score.Current.ToString();
    }

    private void ShowUI() => _scoreUI.SetActive(true);

    private void HideUI() => _scoreUI.SetActive(false);
}
