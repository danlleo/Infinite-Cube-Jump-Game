using System.Collections;
using Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    [DisallowMultipleComponent]
    public class GameOverUI : MonoBehaviour
    {
        private const float FadeInAnimationDurationInSeconds = .25f;
        
        [SerializeField] private GameObject _gameOverUI;
        [SerializeField] private CanvasGroup _gameOverUICanvasGroup;
        [SerializeField] private Button _replayButton;
        [SerializeField] private Button _exitButton;
        
        private void Awake()
        {
            HideUI();
        }

        private void OnEnable()
        {
            Cube.OnCubeFell += Cube_OnCubeFell;

            _replayButton.Add(ReloadCurrentScene);
            _exitButton.Add(QuitGame);
        }

        private void OnDisable()
        {
            Cube.OnCubeFell -= Cube_OnCubeFell;

            _replayButton.Remove(ReloadCurrentScene);
            _exitButton.Remove(QuitGame);
        }

        private void ReloadCurrentScene() 
            => SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        private void QuitGame()
            => Application.Quit();

        private void ShowUI() => _gameOverUI.SetActive(true);

        private void HideUI() => _gameOverUI.SetActive(false);

        private IEnumerator FadeInAnimationRoutine()
        {
            float animationTimer = 0f;

            while (animationTimer < FadeInAnimationDurationInSeconds)
            {
                animationTimer += Time.deltaTime;

                float animationProgress = animationTimer / FadeInAnimationDurationInSeconds;
                _gameOverUICanvasGroup.alpha = animationProgress;
                yield return null;
            }

            _gameOverUICanvasGroup.alpha = 1f;
        }

        private void Cube_OnCubeFell(object sender, System.EventArgs e)
        {
            StartCoroutine(FadeInAnimationRoutine());
            ShowUI();
        }
    }
}
