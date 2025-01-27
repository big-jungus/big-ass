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
    public EffectsManager effectsManager;

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

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= LevelLoaded;
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

        FindObjectOfType<CameraController>().Setup();
    }

    public void CollectableCollected(Collectable c)
    {
        if (c.collectableType == Collectable.CollectableTypes.Win)
        {
            // Win
            levelManager.LoadLevel(0);
            return;
        }

        playerStats.CollectableAdded(c.collectableType, 1);
        playerUI.CollectableAdded(c.collectableType);
    }
}
