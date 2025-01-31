using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private List<string> levels = new List<string>();
    private int activeLevel;

    // Save progress?

    public void LoadLevel(int level)
    {
        PlayerManager.playerManager.levelTransition.StartTransition(level);
    }

    public void NextLevel()
    {
        int newLevel = activeLevel + 1;
        if (newLevel >= levels.Count)
            newLevel = 1;

        PlayerManager.playerManager.levelTransition.StartTransition(newLevel);
    }

    public void AnimationComplete(int level)
    {
        SceneManager.LoadScene(levels[level]);
        activeLevel = level;
    }

    public void ReloadLevel()
    {
        PlayerManager.playerManager.levelTransition.StartTransition(activeLevel);
    }
}
