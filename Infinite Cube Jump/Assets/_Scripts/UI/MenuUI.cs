using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;

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

        ButtonExtensions.Add(_continueButton, () =>
        {
            HideUI();
        });

        ButtonExtensions.Add(_exitButton, () =>
        {
            QuitGame();
        });
    }

    private void OnDisable()
    {
        ControlsUI.OnMainMenuOpen -= ControlsUI_OnMainMenuOpen;
        Cube.OnCubeFell -= Cube_OnCubeFell;

        ButtonExtensions.Remove(_continueButton, () =>
        {
            HideUI();
        });

        ButtonExtensions.Remove(_exitButton, () =>
        {
            QuitGame();
        });
    }

    private void QuitGame()
        => Application.Quit();

    private void Cube_OnCubeFell(object sender, System.EventArgs e)
    {
        HideUI();
    }

    private void ControlsUI_OnMainMenuOpen(object sender, System.EventArgs e)
    {
        ShowUI();
        GameManager.Instance.StopTime();
    }

    private void ShowUI() => _menuUI.SetActive(true);

    private void HideUI()
    {
        GameManager.Instance.ResumeTime();
        _menuUI.SetActive(false);
    }
}
