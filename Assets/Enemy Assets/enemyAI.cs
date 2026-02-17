using UnityEngine;

public class enemyAI : enemyTypes
{
    private Transform target;    
    public EnemyType thisEnemyType;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        target = GameObject.Find("PlayerBase").transform;
        randomiseEnemyTypes();
        setEnemyStats(thisEnemyType);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        if (health <= 0)
        {
            baseScript.experiencePoints += experienceValue;
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "PlayerBase")
        {
            GameObject.Find("PlayerBase").GetComponent<baseScript>().baseHP -= 10;
            Destroy(this.gameObject);
        }
    }
    //randomise enemy type on spewn and set values, 2/5 caveman 2/5 rockthrower 1/5 dinorider
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
