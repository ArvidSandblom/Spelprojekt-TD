using System.Collections;
using UnityEngine;

public class enemyAI : enemyTypes
{
    private Transform target;
    private Transform playerTarget;
    private Transform towerTarget;
    public EnemyType thisEnemyType;

    
    // [SerializeField] Sprite[] CavemanAttack;
    // [SerializeField] Sprite[] CavemanWalk;
    // [SerializeField] Sprite[] RockthrowerWalk;
    // [SerializeField] Sprite[] RockthrowerAttack;
    // [SerializeField] Sprite[] DinoriderWalk;
    // [SerializeField] Sprite[] DinoriderAttack;
    [SerializeField] GameObject playerStats;
    // public Sprite[] currentAnimation;
    [SerializeField] GameObject projectile;
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
            playerTarget = GameObject.Find("Player").transform;
            target = playerTarget;
        }
        randomiseEnemyTypes();
        setEnemyStats(thisEnemyType);
        StartCoroutine(AnimationRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        // If the current tower target was destroyed, fall back to the player
        if (towerTarget == null && target != playerTarget)
        {
            towerTarget = null;
            target = playerTarget;
            MovementState = true;
            ChangeAnimation(currentAnimation);
        }

        if (target == null) return;

        float distanceToTarget = Vector2.Distance(transform.position, target.position);

        if (distanceToTarget <= attackRange && MovementState)
        {
            // Entered attack range — stop and switch to attack animation
            MovementState = false;
            ChangeAnimation(currentAttackAnimation);
        }
        else if (distanceToTarget > attackRange && !MovementState && towerTarget == null)
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
            if (currentAnimation == cavemanWalk || currentAnimation == rockThrowerWalk || currentAnimation == dinoriderWalk)
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
            if (currentAnimation == cavemanAttack || currentAnimation == rockThrowerAttack || currentAnimation == dinoriderAttack)
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
        if (target != null && isRanged == true)
        {
            GameObject projectileInstance = Instantiate(projectile, new Vector3(transform.position.x, transform.position.y - 0.5f, 0f), Quaternion.identity);
            projectileInstance.GetComponent<enemyProjectile>().ChangeAnimation(rockProjectileAnimation);
            projectileInstance.GetComponent<enemyProjectile>().damage = damage;
        }
        else if (towerTarget != null)
        {
            towerScript tower = towerTarget.GetComponent<towerScript>();
            if (tower != null)
                tower.TakeDamage(damage);
        }
        else if (target != null)
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
            towerTarget = collision.transform;
            target = towerTarget;
            MovementState = false;
            ChangeAnimation(currentAttackAnimation);
        }

    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Tower" && collision.transform == towerTarget)
        {
            towerTarget = null;
            target = playerTarget;
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
                currentAnimation = cavemanWalk;
                currentAttackAnimation = cavemanAttack;
                break;
            case 1:
                thisEnemyType = EnemyType.CAVEMAN;
                currentAnimation = cavemanWalk;
                currentAttackAnimation = cavemanAttack;
                break;
            case 2:
                thisEnemyType = EnemyType.ROCKTHROWER;
                currentAnimation = rockThrowerWalk;
                currentAttackAnimation = rockThrowerAttack;
                break;
            case 3:
                thisEnemyType = EnemyType.ROCKTHROWER;
                currentAnimation = rockThrowerWalk;
                currentAttackAnimation = rockThrowerAttack;
                break;
            case 4:
                randomiseEnemyTypes();
                // thisEnemyType = EnemyType.DINORIDER;
                // currentAnimation = dinoriderWalk;
                // currentAttackAnimation = dinoriderAttack;
                break;
        }
    }
}
