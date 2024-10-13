using Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [DisallowMultipleComponent]
    public class MenuUI : MonoBehaviour
    {
        [SerializeField] private GameObject _menuUI;
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _exitButton;

        private void Awake()
        {
            HideUI();
        }

        private void OnEnable()
        {
            ControlsUI.OnMainMenuOpen += ControlsUI_OnMainMenuOpen;
            Cube.OnCubeFell += Cube_OnCubeFell;

            _continueButton.Add(HideUI);
            _exitButton.Add(QuitGame);
        }

        private void OnDisable()
        {
            ControlsUI.OnMainMenuOpen -= ControlsUI_OnMainMenuOpen;
            Cube.OnCubeFell -= Cube_OnCubeFell;

            _continueButton.Remove(HideUI);
            _exitButton.Remove(QuitGame);
        }

        private void QuitGame()
            => Application.Quit();

        private void ShowUI() => _menuUI.SetActive(true);

        private void HideUI()
        {
            GameManager.Instance.ResumeTime();
            _menuUI.SetActive(false);
        }

        private void Cube_OnCubeFell(object sender, System.EventArgs e)
        {
            HideUI();
        }

        private void ControlsUI_OnMainMenuOpen(object sender, System.EventArgs e)
        {
            ShowUI();
            GameManager.Instance.StopTime();
        }
    }
}
