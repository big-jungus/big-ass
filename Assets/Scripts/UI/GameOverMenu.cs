using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] private GameObject gameOverScreen;

    private void Start()
    {
        PlayerManager.playerManager.playerCombat.PlayerDeath += OnPlayerDeath;

        gameOverScreen.SetActive(false);
    }

    private void OnDestroy()
    {
        PlayerManager.playerManager.playerCombat.PlayerDeath -= OnPlayerDeath;
    }

    private void OnPlayerDeath()
    {
        gameOverScreen.SetActive(true);
    }

    public void ReloadLevel()
    {
        PlayerManager.playerManager.levelManager.ReloadLevel();
    }

    public void LoadMainMenu()
    {
        PlayerManager.playerManager.levelManager.LoadLevel(0);
    }
}
