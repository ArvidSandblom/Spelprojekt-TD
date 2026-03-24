using System.Collections;
using UnityEngine;

public class enemyAI : enemyTypes
{
    private Transform target;    
    public EnemyType thisEnemyType;
    [SerializeField] Sprite[] CavemanAttack;
    [SerializeField] Sprite[] CavemanWalk;
    [SerializeField] Sprite[] RockthrowerWalk;
    [SerializeField] Sprite[] RockthrowerAttack;
    [SerializeField] Sprite[] DinoriderWalk;
    [SerializeField] Sprite[] DinoriderAttack;
    [SerializeField] GameObject playerStats;
    public Sprite[] currentAnimation;
    public Sprite[] currentAttackAnimation;
    SpriteRenderer spriteRenderer;
    GameObject childObject;
    BoxCollider2D boxCollider2D;
    int frameIndex = 0;
    public float animationSpeed;
    bool MovementState = true; //true = moving, false = attacking


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        childObject = transform.GetChild(0).gameObject;   
        spriteRenderer = childObject.GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        playerStats = GameObject.Find("playerStats");
        if (GameObject.Find("Player") != null)
        {
            target = GameObject.Find("Player").transform;
        }
        randomiseEnemyTypes();
        setEnemyStats(thisEnemyType);
        StartCoroutine(AnimationRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, target.position);

        if (distanceToPlayer <= attackRange && MovementState)
        {
            // Entered player attack range — stop and switch to attack animation
            MovementState = false;
            ChangeAnimation(currentAttackAnimation);
        }
        else if (distanceToPlayer > attackRange && !MovementState)
        {
            // Left player attack range — resume walking
            MovementState = true;
            ChangeAnimation(currentAnimation);
        }

        moveTowardPlayer();
        FaceTarget();
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
    IEnumerator AnimationRoutine()
    {
        while (true)
        {
            if (currentAnimation == CavemanWalk || currentAnimation == RockthrowerWalk || currentAnimation == DinoriderWalk)
            {
                animationSpeed = speed / currentAnimation.Length;
                animationSpeed *= 0.5f;
            }
            if (target == null)
            {
                frameIndex = 0;
                spriteRenderer.sprite = currentAnimation[frameIndex];
                yield return null;
                continue;
            }
            if (currentAnimation == CavemanAttack || currentAnimation == RockthrowerAttack || currentAnimation == DinoriderAttack)
            {
                
                if (frameIndex == currentAnimation.Length - 1)
                {
                    damagePlayer();
                }
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
    void moveTowardPlayer()
    {
        if (MovementState)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
        float direction = target.position.x - transform.position.x;

        if (direction > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (direction < 0)
        {
            spriteRenderer.flipX = true;
        }
    }
    void damagePlayer()
    {    
        if (target != null)
        {
            playerStats.GetComponent<playerStats>().TakeDamage(damage);
        }
        
    }
    void FaceTarget()
    {
        Vector2 direction = (target.position - transform.position).normalized;

        bool isFlipped = direction.x < 0;
        spriteRenderer.flipX = isFlipped;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        float maxTilt = 25f;
        angle = Mathf.Clamp(angle, -maxTilt, maxTilt);

        if (isFlipped)
        {
            angle *= -1f;
        }

        childObject.transform.rotation = Quaternion.Euler(0, 0, angle);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Tower")
        {
            MovementState = false;
            ChangeAnimation(currentAttackAnimation);
        }

    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Tower")
        {
            MovementState = true;
            ChangeAnimation(currentAnimation);
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
                currentAnimation = CavemanWalk;
                currentAttackAnimation = CavemanAttack;
                break;
            case 1:
                thisEnemyType = EnemyType.CAVEMAN;
                currentAnimation = CavemanWalk;
                currentAttackAnimation = CavemanAttack;
                break;
            case 2:
                thisEnemyType = EnemyType.CAVEMAN;
                currentAnimation = CavemanWalk;
                currentAttackAnimation = CavemanAttack;
                break;
            case 3:
                thisEnemyType = EnemyType.CAVEMAN;
                currentAnimation = CavemanWalk;
                currentAttackAnimation = CavemanAttack;
                break;
            case 4:
                thisEnemyType = EnemyType.CAVEMAN;
                currentAnimation = CavemanWalk;
                currentAttackAnimation = CavemanAttack;
                break;
        }
    }
}
