using UnityEngine;

public class enemyAI : enemyTypes
{
    private Transform target;    
    public EnemyType thisEnemyType;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        target = GameObject.Find("Player").transform;
        randomiseEnemyTypes();
        setEnemyStats(thisEnemyType);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        if (health <= 0)
        {
            GameObject.Find("Player").GetComponent<baseScript>().experiencePoints += experienceValue;
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            GameObject.Find("Player").GetComponent<baseScript>().baseHP -= 10;
            Destroy(this.gameObject);
        }
    }
    //randomise enemy type on spawn and set values, 2/5 caveman 2/5 rockthrower 1/5 dinorider
    void randomiseEnemyTypes()
    {
        int randomType = Random.Range(0, 5);
        switch (randomType)
        {
            case 0:
                thisEnemyType = EnemyType.CAVEMAN;                
                break;
            case 1:
                thisEnemyType = EnemyType.CAVEMAN;                
                break;
            case 2:
                thisEnemyType = EnemyType.ROCKTHROWER;
                break;
            case 3:
                thisEnemyType = EnemyType.ROCKTHROWER;
                break;
            case 4:
                thisEnemyType = EnemyType.DINORIDER;                
                break;
        }
    }
}
