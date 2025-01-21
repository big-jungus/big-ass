using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public PlayerStats playerStats;
    public PlayerCombat playerCombat;
    public PlayerController playerController;
    public PlayerUI playerUI;
    public GameObject playerObj;

    public static PlayerManager playerManager;

    private void Awake()
    {
        if (playerManager == null)
        {
            playerManager = this;
        }
        else if (playerManager != this)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }
}
