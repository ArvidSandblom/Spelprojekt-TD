using System.Collections;
using UnityEngine;

public class enemyProjectile : enemyTypes
{
    public float missileSpeed = 3f;
    private Transform target;
    private Vector2 targetDirection;
    // [SerializeField] Sprite[] rockProjectileAnimation;
    // public Sprite[] currentAnimation;
    SpriteRenderer spriteRenderer;
    int frameIndex = 0;
    GameObject playerStats;
    
    public float animationSpeed = 0.1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        target = FindClosestTarget();
        playerStats = GameObject.Find("playerStats");
        if (target != null)
        {
            targetDirection = (target.position - transform.position).normalized;
            Vector2 direction = target.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        else
        {
            targetDirection = Vector2.zero;
        }
        StartCoroutine(AnimationRoutine());
        
        Destroy(gameObject, 5f);
    }

    // Update is called once per frame
     void Update()
    {
        transform.position += (Vector3)targetDirection * missileSpeed * Time.deltaTime;       
    }
    public void ChangeAnimation(Sprite[] animationToChangeTo)
    {
        if(currentAnimation != animationToChangeTo)
        {
            currentAnimation = animationToChangeTo;
            frameIndex = 0;            
        }
    }
    Transform FindClosestTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Tower");
        Transform closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy.transform;
            }
        }
        if (closestEnemy == null)
        {            
            return null;
        }

        return closestEnemy;
    }
    IEnumerator AnimationRoutine()
    {
        while (true)
        {
            
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
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {            
            playerStats.GetComponent<playerStats>().TakeDamage(damage);
            Debug.Log("Player hit for " + damage + " damage!");
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag == "Tower")
        {
            collision.gameObject.GetComponent<towerScript>().TakeDamage(damage);
            Debug.Log("Tower hit for " + damage + " damage!");

            Destroy(gameObject);
        }
        
    }
}
