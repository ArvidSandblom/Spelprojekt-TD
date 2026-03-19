using System.Collections;
using UnityEngine;

public class playerProjectile : MonoBehaviour
{
    public bool findStrongest = false;
    public bool findClosest = false;    
    private Transform target;
    private Vector2 targetDirection;
    public float missileSpeed = 5f;
    public float damage;
    public float critChance;
    public float critDamage;
    public Sprite[] currentAnimation;
    SpriteRenderer spriteRenderer;
    GameObject childSprite;
    int frameIndex = 0;
    public float animationSpeed = 0.1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (findStrongest)
        {
            target = findStrongestEnemy();
        }
        else if (findClosest)
        {
            target = FindClosestEnemy();
        }
        
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

    Transform FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
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
            Destroy(gameObject);
            return null;
        }

        return closestEnemy;
    }
    Transform findStrongestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform strongestEnemy = null;
        float highestHealth = 0f;

        foreach (GameObject enemy in enemies)
        {
            enemyAI enemyScript = enemy.GetComponent<enemyAI>();
            if (enemyScript != null && enemyScript.health > highestHealth)
            {
                highestHealth = enemyScript.health;
                strongestEnemy = enemy.transform;
            }
        }
        if (strongestEnemy == null)
        {
            Destroy(gameObject);
            return null;
        }

        return strongestEnemy;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            enemyAI enemy = collision.GetComponent<enemyAI>();
            if (enemy != null)
            {
                int critRoll = Random.Range(0,101);
                if (critRoll < critChance)
                {
                    enemy.TakeDamage(damage * critDamage);
                }
                else 
                {
                    enemy.TakeDamage(damage);
                }
            }
            Destroy(this.gameObject);
        }
    }
    public void setProjectileStats(towerBaseClass.TowerType towerType, float towerDamage, float towerCritChance, float towerCritDamage)
    {
        damage = towerDamage;
        critChance = towerCritChance;
        critDamage = towerCritDamage;
        switch (towerType)
        {
            case towerBaseClass.TowerType.ROCKTHROWER:
                findClosest = true;
                findStrongest = false;
                break;
            case towerBaseClass.TowerType.SLINGSHOT:
                findClosest = false;
                findStrongest = true;
                break;
            case towerBaseClass.TowerType.SPEARTHROWER:
                findClosest = false;
                findStrongest = true;
                break;
            case towerBaseClass.TowerType.ARCHER:
                findClosest = true;
                findStrongest = false;
                break;
        }
    }
}
