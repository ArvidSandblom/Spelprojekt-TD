using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class enemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public static int enemiesDestroyed = 0;
    public float spawnTimer = 2f;
    public bool canSpawn = true;
    private static enemySpawner enemyInstance;
    public static enemySpawner Instance => enemyInstance;

    private readonly List<GameObject> activeEnemies = new List<GameObject>();

    void Awake()
    {
        DontDestroyOnLoad(this);

        if (enemyInstance == null)
        {
            enemyInstance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private const int GameSceneBuildIndex = 1;

    /// <summary>
    /// Moves all surviving enemies back into the Game scene so they continue
    /// to behave as normal scene objects. Skipped for non-game scenes.
    /// </summary>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex != GameSceneBuildIndex) return;

        activeEnemies.RemoveAll(e => e == null);

        foreach (GameObject enemy in activeEnemies)
        {
            SceneManager.MoveGameObjectToScene(enemy, scene);
        }
    }

    void Start()
    {
        pickSpawnArea();
        StartCoroutine(SpawnEnemyRoutine());
    }

    void Update() { }

    IEnumerator SpawnEnemyRoutine()
    {
        while (baseScript.alive)
        {
            if (canSpawn)
            {
                GameObject enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
                DontDestroyOnLoad(enemy);
                activeEnemies.Add(enemy);
                pickSpawnArea();

                if (enemiesDestroyed >= 10)
                {
                    changeSpawnRate(0.9f);
                }
            }
            yield return new WaitForSeconds(spawnTimer);
        }
    }

    /// <summary>
    /// Removes a dying enemy from the persistence list so it is no longer
    /// moved across scenes after it has been destroyed.
    /// </summary>
    public static void UnregisterEnemy(GameObject enemy)
    {
        enemyInstance?.activeEnemies.Remove(enemy);
    }

    void changeSpawnRate(float changeValue)
    {
        spawnTimer *= changeValue;
        enemiesDestroyed = 0;
    }
    
    public void pickSpawnArea()
    {
        int spawnArea = Random.Range(0, 4);
        switch (spawnArea)
        {
            case 0: // Top
                defineSpawnArea(-9f, 9f, 6f, 6f);
                break;
            case 1: // Bottom
                defineSpawnArea(-9f, 9f, -6f, -6f);
                break;
            case 2: // Left
                defineSpawnArea(-9f, -9f, -6f, 6f);
                break;
            case 3: // Right
                defineSpawnArea(9f, 9f, -6f, 6f);
                break;
        }
    }
    public void defineSpawnArea(float xMin, float xMax, float yMin, float yMax)
    {
        float randomX = Random.Range(xMin, xMax);
        float randomY = Random.Range(yMin, yMax);
        transform.position = new Vector2(randomX, randomY);
    }
}
