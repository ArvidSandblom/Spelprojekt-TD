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
    private int towerIndex = 0;

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
    /// Safe to call multiple times — skips already-created positions.
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
    /// Places a default tower at the next available inner tower position.
    /// </summary>
    public void instantiateBaseTower()
    {
        if (innerTowerPositions == null || towerIndex >= innerTowerPositions.Length)
        {
            Debug.LogWarning("gameManager: All tower positions are occupied.", this);
            return;
        }

        Transform slot = innerTowerPositions[towerIndex];
        if (slot == null)
        {
            Debug.LogError($"gameManager: innerTowerPositions[{towerIndex}] is null.", this);
            return;
        }

        GameObject instantiatedTower = Instantiate(tower, slot.position, Quaternion.identity);
        instantiatedTower.GetComponent<towerScript>().setTowerType(0);
        DontDestroyOnLoad(instantiatedTower);
        towerIndex++;
    }

    /// <summary>
    /// Adds a specific tower type at its predefined world position.
    /// </summary>
    public void addTower(string towerType)
    {
        Vector3 towerPos;
        towerBaseClass.TowerType type;

        switch (towerType)
        {
            case "ROCKTHROWER":
                type = towerBaseClass.TowerType.ROCKTHROWER;
                towerPos = new Vector3(2, 2, 0);
                break;
            case "SLINGSHOT":
                type = towerBaseClass.TowerType.SLINGSHOT;
                towerPos = new Vector3(-2, 2, 0);
                break;
            case "SPEARTHROWER":
                type = towerBaseClass.TowerType.SPEARTHROWER;
                towerPos = new Vector3(-2, -2, 0);
                break;
            case "ARCHER":
                type = towerBaseClass.TowerType.ARCHER;
                towerPos = new Vector3(2, -2, 0);
                break;
            default:
                type = towerBaseClass.TowerType.ROCKTHROWER;
                towerPos = new Vector3(2, 2, 0);
                break;
        }

        GameObject addedTower = Instantiate(tower, towerPos, Quaternion.identity);
        addedTower.transform.localScale = new Vector3(3, 3, 1);
        addedTower.GetComponent<towerScript>().setTowerType(type);
        DontDestroyOnLoad(addedTower);
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
