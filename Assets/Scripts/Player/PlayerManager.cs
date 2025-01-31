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
    public SoundManager soundManager;
    public LevelTransition levelTransition;

    [Header("Required Prefabs")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject uiPrefab;

    public static PlayerManager playerManager;

    private bool isFirstLoad = true;

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
        if (isFirstLoad)
            isFirstLoad = false;
        else
            levelTransition.EndTransition();

        if (newScene.name == "MainMenu" || newScene.name == "WinScreen")
        {
            soundManager.TransitionMusic(SoundManager.MusicStates.MainMenu);
            return;
        }
        else
            soundManager.TransitionMusic(SoundManager.MusicStates.Gameplay);

        Time.timeScale = 1f;

        playerObj = Instantiate(playerPrefab);
        playerObj.transform.position = Vector3.zero;
        playerController = playerObj.GetComponent<PlayerController>();
        playerCombat = playerObj.GetComponentInChildren<PlayerCombat>();

        SetupUI();

        FindObjectOfType<CameraController>().Setup();
        soundManager.Setup();
        effectsManager.Setup();
        playerStats.currentLevelTimer = 0;
    }
    void SetupUI(){
        GameObject ui = Instantiate(uiPrefab);
        playerUI = ui.GetComponentInChildren<PlayerUI>();
        ui.GetComponent<Canvas>().worldCamera = FindObjectOfType<Camera>();
    }

    public void CollectableCollected(Collectable c)
    {
        if (c.collectableType == Collectable.CollectableTypes.Win)
        {
            playerStats.timerStart = false;
            playerUI.ShowScoreboard();
            return;
        }

        playerStats.CollectableAdded(c.collectableType, 1);
    }

    public void StartGameplay()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu" || SceneManager.GetActiveScene().name == "WinScreen")
            return;

        playerController.EnableCharge();
        playerStats.timerStart = true;
    }
}
