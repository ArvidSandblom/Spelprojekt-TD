using System.Collections;
using UnityEngine;

public class enemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public static int enemiesDestroyed = 0; 
    public float spawnTimer = 2f;
    public bool canSpawn = true;
    private static enemySpawner enemyInstance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    void Start()
    {
        pickSpawnArea();
        StartCoroutine(SpawnEnemyRoutine());
    }
    void Awake()
    {
        DontDestroyOnLoad (this);
            
        if (enemyInstance == null) 
        {
            enemyInstance = this;
        } 
        else 
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator SpawnEnemyRoutine()
    {
        while (baseScript.alive)
        {
            if (canSpawn)
            {
                Instantiate(enemyPrefab, transform.position, Quaternion.identity);
                pickSpawnArea();
                if (enemiesDestroyed >= 10)
                {
                    changeSpawnRate(0.9f);
                }
            }
            yield return new WaitForSeconds(spawnTimer);
            
        }        
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
