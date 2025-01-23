using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private InputActionReference pause;
    [SerializeField] private GameObject menu;

    private void Start()
    {
        pause.action.performed += Paused;

        menu.SetActive(false);
    }

    private void OnDestroy()
    {
        pause.action.performed -= Paused;
    }

    private void Paused(InputAction.CallbackContext context)
    {
        menu.SetActive(!menu.activeSelf);
        PlayerManager.playerManager.playerUI.isPaused = menu.activeSelf;

        if (menu.activeSelf)
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;
    }

    public void LoadMainMenu()
    {
        PlayerManager.playerManager.levelManager.LoadLevel(0);
    }

    public void ResumeGame()
    {
        Paused(new InputAction.CallbackContext());
    }

    public void ReloadLevel()
    {
        PlayerManager.playerManager.levelManager.ReloadLevel();
    }
}
