using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class gameManager : MonoBehaviour
{
    private static gameManager gameManagerInstance;

    public GameObject tower;

    [SerializeField] private GameObject[] innerTowerPositionPrefabs = new GameObject[4];

    private const int GameSceneBuildIndex = 1;

    private Transform[] innerTowerPositions;

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
    }

    void Start()
    {
        SpawnInnerTowerPositions();

        if (SceneManager.GetActiveScene().buildIndex == GameSceneBuildIndex)
            WireAddTowerButton();
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

        addTowerButton.onClick.RemoveAllListeners();
        addTowerButton.onClick.AddListener(instantiateBaseTower);
    }

    /// <summary>
    /// Places a default tower as a child of the first inner tower position that has no children.
    /// </summary>
    public void instantiateBaseTower()
    {
        if (innerTowerPositions == null)
        {
            Debug.LogError("gameManager: innerTowerPositions is not initialized.", this);
            return;
        }

        foreach (Transform slot in innerTowerPositions)
        {
            if (slot == null || slot.childCount > 0) continue;

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
        if (Input.GetKeyDown(KeyCode.Escape))
            Time.timeScale = Time.timeScale == 1f ? 0f : 1f;
    }

    public void openShop() { }

    public void levelUp() { }

    public void continueGame() => SceneManager.LoadScene(1);

    public void exitGame() => Application.Quit();

    public void mainMenu() => SceneManager.LoadScene(0);

    public void restartGame() => SceneManager.LoadScene(1);
}
