using System.Collections;
using UnityEngine;

public class enemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnTimer = 2f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    void Start()
    {
        pickSpawnArea();
        StartCoroutine(SpawnEnemyRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnEnemy()
    {
        
    }
    IEnumerator SpawnEnemyRoutine()
    {
        while (baseScript.alive)
        {
            Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            pickSpawnArea();
            yield return new WaitForSeconds(spawnTimer);
            
        }        
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
