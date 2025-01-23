using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void LoadLevel(int levelIndex)
    {
        PlayerManager.playerManager.levelManager.LoadLevel(levelIndex);
    }
}
