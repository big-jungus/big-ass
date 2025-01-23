using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private List<string> levels = new List<string>();

    // Save progress?

    public void LoadLevel(int level)
    {
        SceneManager.LoadScene(levels[level]);
    }
}
