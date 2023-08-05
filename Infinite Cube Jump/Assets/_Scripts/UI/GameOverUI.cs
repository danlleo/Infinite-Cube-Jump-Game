using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject _gameOverUI;
    [SerializeField] private CanvasGroup _gameOverUICanvasGroup;
    [SerializeField] private Button _replayButton;
    [SerializeField] private Button _exitButton;

    private float _fadeInAnimationDurationInSeconds = .25f;

    private void Awake()
    {
        HideUI();
    }

    private void OnEnable()
    {
        Cube.OnCubeFell += Cube_OnCubeFell;

        ButtonExtensions.Add(_replayButton, () =>
        {
            ReloadCurrentScene();
        });

        ButtonExtensions.Add(_exitButton, () =>
        {
            QuitGame();
        });
    }

    private void OnDisable()
    {
        Cube.OnCubeFell -= Cube_OnCubeFell;

        ButtonExtensions.Remove(_replayButton, () =>
        {
            ReloadCurrentScene();
        });

        ButtonExtensions.Remove(_exitButton, () =>
        {
            QuitGame();
        });
    }

    private void ReloadCurrentScene() 
        => SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    private void QuitGame()
        => Application.Quit();

    private void Cube_OnCubeFell(object sender, System.EventArgs e)
    {
        StartCoroutine(FadeInAnimationRoutine());
        ShowUI();
    }

    private void ShowUI() => _gameOverUI.SetActive(true);

    private void HideUI() => _gameOverUI.SetActive(false);

    private IEnumerator FadeInAnimationRoutine()
    {
        float animationTimer = 0f;

        while (animationTimer < _fadeInAnimationDurationInSeconds)
        {
            animationTimer += Time.deltaTime;

            float animationProgress = animationTimer / _fadeInAnimationDurationInSeconds;
            _gameOverUICanvasGroup.alpha = animationProgress;
            yield return null;
        }

        _gameOverUICanvasGroup.alpha = 1f;
    }
}
