using System;
using Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [DisallowMultipleComponent]
    public class ControlsUI : MonoBehaviour
    {
        public static event EventHandler OnMainMenuOpen;

        [SerializeField] private GameObject _controlsUI;
        [SerializeField] private Button _openMenuButton;

        private void Awake()
        {
            ShowUI();
        }

        private void OnEnable()
        {
            Cube.OnCubeFell += Cube_OnCubeFell;

            _openMenuButton.Add(OpenMainMenu);
        }

        private void OnDisable()
        {
            Cube.OnCubeFell -= Cube_OnCubeFell;

            _openMenuButton.Remove(OpenMainMenu);
        }

        private void Cube_OnCubeFell(object sender, EventArgs e)
        {
            HideUI();
        }

        private void OpenMainMenu()
            => OnMainMenuOpen?.Invoke(this, EventArgs.Empty);

        private void ShowUI() => _controlsUI.SetActive(true);

        private void HideUI() => _controlsUI.SetActive(false);
    }
}
