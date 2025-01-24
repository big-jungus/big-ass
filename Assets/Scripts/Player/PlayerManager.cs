using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    [Header("References")]
    public PlayerStats playerStats;
    public PlayerCombat playerCombat;
    public PlayerController playerController;
    public PlayerUI playerUI;
    public GameObject playerObj;
    public LevelManager levelManager;

    [Header("Required Prefabs")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject uiPrefab;

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

        Setup();
    }

    private void Setup()
    {
        SceneManager.sceneLoaded += LevelLoaded;
    }

    public void LevelLoaded(Scene newScene, LoadSceneMode loadScene)
    {
        if (newScene.name == "MainMenu")
            return;

        Time.timeScale = 1f;

        playerObj = Instantiate(playerPrefab);
        playerObj.transform.position = Vector3.zero;
        playerController = playerObj.GetComponent<PlayerController>();
        playerCombat = playerObj.GetComponentInChildren<PlayerCombat>();

        GameObject ui = Instantiate(uiPrefab);
        playerUI = ui.GetComponentInChildren<PlayerUI>();
    }

    public void CollectableCollected(Collectable c)
    {
        playerStats.CollectableAdded(c);
        playerUI.CollectableAdded(c);
    }
}
