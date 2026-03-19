using System.Collections;
using UnityEngine;

public class enemyAI : enemyTypes
{
    private Transform target;    
    public EnemyType thisEnemyType;
    [SerializeField] SpriteRenderer[] CavemanAttack;
    [SerializeField] SpriteRenderer[] CavemanWalk;
    [SerializeField] SpriteRenderer[] Rockthrower;
    [SerializeField] SpriteRenderer[] RockthrowerAttack;
    [SerializeField] SpriteRenderer[] Dinorider;
    [SerializeField] SpriteRenderer[] DinoriderAttack;
    public Sprite[] currentAnimation;
    SpriteRenderer spriteRenderer;
    GameObject childSprite;
    int frameIndex = 0;
    public float animationSpeed;
    bool MovementState = true; //true = moving, false = attacking
    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        childSprite = transform.GetChild(0).gameObject;   
        spriteRenderer = childSprite.GetComponent<SpriteRenderer>();
        if (GameObject.Find("Player") != null)
        {
            target = GameObject.Find("Player").transform;
        }
        randomiseEnemyTypes();
        setEnemyStats(thisEnemyType);
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            attackPlayer();
        }
        if (health <= 0)
        {
            
        }
    }
     public void ChangeAnimation(Sprite[] animationToChangeTo)
    {
        if(currentAnimation != animationToChangeTo)
        {
            currentAnimation = animationToChangeTo;
            animationSpeed = attackRate / currentAnimation.Length;

            frameIndex = 0;            
        }
    }
    IEnumerator AnimationAndFiringRoutine()
    {
        while (true)
        {
            if (target == null)
            {
                frameIndex = 0;
                spriteRenderer.sprite = currentAnimation[frameIndex];
                yield return null;
                continue;
            }
            if (currentAnimation != null && currentAnimation.Length > 0)
            {
                if (frameIndex >= currentAnimation.Length)
                {
                    frameIndex = 0;
                }
                    spriteRenderer.sprite = currentAnimation[frameIndex];
                    frameIndex++;                                
                    yield return new WaitForSeconds(animationSpeed);
                
            }
            yield return null;
        }
    }
    void attackPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Tower")
        {
            MovementState = false;
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
