using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class gameManager : MonoBehaviour
{
    private static gameManager gameManagerInstance;

    public GameObject tower;

    public int enemiesDefeated = 0;

    private const float TowerBuildCost = 100f;
    private int towersBuilt = 0;

    [SerializeField] private GameObject[] innerTowerPositionPrefabs = new GameObject[4];

    private const int GameSceneBuildIndex = 1;

    private Transform[] innerTowerPositions;

    private GameObject cheatPanel;

    void Awake()
    {
        DontDestroyOnLoad(this);

        if (gameManagerInstance == null)
        {
            gameManagerInstance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
        cheatPanel = GameObject.Find("cheatPanel");
            if (cheatPanel != null){
                cheatPanel.SetActive(false);
            }
    }

    public void toggleCheatPanel()
    {
        if (cheatPanel != null)
        {
            cheatPanel.SetActive(!cheatPanel.activeSelf);
        }
    }

    void Start()
    {
        SpawnInnerTowerPositions();

        if (SceneManager.GetActiveScene().buildIndex == GameSceneBuildIndex)
        {
            WireAddTowerButton();
            //WireCheatPanelButton();
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)){
            timeScaleSwitch();
        }        
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex != GameSceneBuildIndex) return;

        WireAddTowerButton();
        //WireCheatPanelButton();
    }

    /// <summary>
    /// Instantiates each inner tower position prefab once and marks them DontDestroyOnLoad.
    /// </summary>
    private void SpawnInnerTowerPositions()
    {
        innerTowerPositions = new Transform[innerTowerPositionPrefabs.Length];

        for (int i = 0; i < innerTowerPositionPrefabs.Length; i++)
        {
            if (innerTowerPositionPrefabs[i] == null)
            {
                Debug.LogError($"gameManager: innerTowerPositionPrefabs[{i}] is not assigned.", this);
                continue;
            }

            GameObject posObj = Instantiate(innerTowerPositionPrefabs[i]);
            DontDestroyOnLoad(posObj);
            innerTowerPositions[i] = posObj.transform;
        }
    }

    // private void WireCheatPanelButton()
    // {
    //     GameObject openBtn = GameObject.Find("openCheatPanel");
    //     if (openBtn == null)
    //     {
    //         Debug.LogWarning("gameManager: Could not find 'openCheatPanel' in scene.", this);
    //         return;
    //     }

    //     // FindFirstObjectByType with inactive search so the panel can start disabled in the scene
    //     adminScripts admin = FindFirstObjectByType<adminScripts>(FindObjectsInactive.Include);
    //     if (admin == null)
    //     {
    //         Debug.LogWarning("gameManager: Could not find adminScripts component.", this);
    //         return;
    //     }

    //     Button openCheatPanelButton = openBtn.GetComponent<Button>();
    //     openCheatPanelButton.onClick.RemoveAllListeners();
    //     openCheatPanelButton.onClick.AddListener(admin.TogglePanel);
    // }

    private void WireAddTowerButton()
    {
        GameObject addTowerObj = GameObject.Find("addTower");
        if (addTowerObj == null)
        {
            Debug.LogWarning("gameManager: Could not find 'addTower' in scene.", this);
            return;
        }

        Button addTowerButton = addTowerObj.GetComponent<Button>();
        if (addTowerButton == null) return;

        TMPro.TMP_Text towerButtonName = addTowerButton.GetComponentInChildren<TMPro.TMP_Text>();
        if (towerButtonName != null){
            towerButtonName.text = $"Build Tower\n{TowerBuildCost:F0}g";
            Debug.Log("Not loading cost");
            }

        addTowerButton.onClick.RemoveAllListeners();
        addTowerButton.onClick.AddListener(instantiateBaseTower);
    }

    /// <summary>
    /// Places a default tower as a child of the first inner tower position that has no children.
    /// Costs <see cref="TowerBuildCost"/> gold; returns early if the player cannot afford it.
    /// </summary>
    public void instantiateBaseTower()
    {
        if (innerTowerPositions == null)
        {
            Debug.LogError("gameManager: innerTowerPositions is not initialized.", this);
            return;
        }

        GameObject playerStatsObj = GameObject.Find("playerStats");
        playerStats stats = playerStatsObj != null ? playerStatsObj.GetComponent<playerStats>() : null;

        bool isFirstTower = towersBuilt == 0;
        if (!isFirstTower && (stats == null || stats.gold < TowerBuildCost))
        {
            Debug.LogWarning("gameManager: Not enough gold to build a tower.", this);
            return;
        }

        foreach (Transform slot in innerTowerPositions)
        {
            if (slot == null || slot.childCount > 0) continue;

            if (!isFirstTower)
                stats.gold -= TowerBuildCost;

            towersBuilt++;
            GameObject instantiatedTower = Instantiate(tower, slot.position, Quaternion.identity, slot);
            instantiatedTower.GetComponent<towerScript>().setTowerType(0);
            return;
        }

        Debug.LogWarning("gameManager: All tower positions are occupied.", this);
    }

    /// <summary>
    /// Adds a specific tower type at the first available inner tower position, parented to that slot.
    /// </summary>
    public void addTower(string towerType)
    {
        if (innerTowerPositions == null)
        {
            Debug.LogError("gameManager: innerTowerPositions is not initialized.", this);
            return;
        }

        towerBaseClass.TowerType type = towerType switch
        {
            "ROCKTHROWER"  => towerBaseClass.TowerType.ROCKTHROWER,
            "SLINGSHOT"    => towerBaseClass.TowerType.SLINGSHOT,
            "SPEARTHROWER" => towerBaseClass.TowerType.SPEARTHROWER,
            "ARCHER"       => towerBaseClass.TowerType.ARCHER,
            _              => towerBaseClass.TowerType.ROCKTHROWER,
        };

        foreach (Transform slot in innerTowerPositions)
        {
            if (slot == null || slot.childCount > 0) continue;

            GameObject addedTower = Instantiate(tower, slot.position, Quaternion.identity, slot);
            addedTower.transform.localScale = new Vector3(3, 3, 1);
            towerScript script = addedTower.GetComponent<towerScript>();
            script.setTowerType(type);
            script.setAsParentObject(slot);
            return;
        }

        Debug.LogWarning("gameManager: All tower positions are occupied.", this);
    }

    void timeScaleSwitch()
    {
        Time.timeScale = Time.timeScale == 1f ? 0f : 1f;
        
    }

    public void openShop() { }

    public void levelUp() { }

    public void continueGame() => SceneManager.LoadScene(1);

    public void exitGame() => Application.Quit();

    public void mainMenu() => SceneManager.LoadScene(0);

    public void restartGame() => SceneManager.LoadScene(1);
}
