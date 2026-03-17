using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class towerScript : towerClass
{
    public towerBaseClass.TowerType thisTowerType;
    public Sprite[] projectileSprites;
    public GameObject projectilePrefab;
    [SerializeField] Sprite[] rockthrower;
    [SerializeField] Sprite[] slinger;
    [SerializeField] Sprite[]  spearthrower;
    [SerializeField] Sprite[] archer;
    
    public Sprite[] currentAnimation;
    SpriteRenderer spriteRenderer;
    GameObject childSprite;
    int frameIndex = 0;
    public float animationSpeed; // Time between frames in seconds
    private Transform target;

    [SerializeField] private GameObject upgradeScreenPrefab;

    private GameObject playerStats;
    private bool statsInitialized = false;

    void Awake()
    {
        childSprite = transform.GetChild(0).gameObject;
        spriteRenderer = childSprite.GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        playerStats = GameObject.Find("playerStats");
        setTowerStats(thisTowerType);
        damage = this.damage * playerStats.GetComponent<playerStats>().damageUpgradeMultiplier;
        fireRate = this.fireRate * playerStats.GetComponent<playerStats>().fireRateUpgradeMultiplier;
        statsInitialized = true;
        if (currentAnimation != null && currentAnimation.Length > 0)
        {
            animationSpeed = fireRate / currentAnimation.Length;
        }
        StartCoroutine(AnimationAndFiringRoutine());

    }
    private const float TargetRefreshInterval = 0.2f;
    private float targetRefreshTimer = 0f;

    void Update()
{
    targetRefreshTimer -= Time.deltaTime;
    if (targetRefreshTimer <= 0f)
    {
        targetRefreshTimer = TargetRefreshInterval;
        UpdateTarget();
    }

    if (target != null)
    {
        Vector2 direction = (target.position - transform.position).normalized;

        // Clamp direction to upper hemisphere — prevents the sprite from
        // rotating past horizontal and appearing upside down
        if (direction.y < 0f)
            direction = new Vector2(direction.x, 0f);

        // Edge case: enemy is directly below (direction becomes zero after clamp)
        if (direction == Vector2.zero)
            direction = Vector2.right;
        else
            direction.Normalize();

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        childSprite.transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}


    private void UpdateTarget()
    {
        if (findStrongest)
            target = findStrongestEnemy();
        else if (findClosest)
            target = FindClosestEnemy();
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

        return strongestEnemy;
    }



    void OnEnable()
    {
        // On first activation Start() hasn't run yet, so stats aren't ready
        if (!statsInitialized) return;

        StartCoroutine(AnimationAndFiringRoutine());
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    public void setTowerType(towerBaseClass.TowerType type)
    {
        thisTowerType = type;
        setTowerStats(type);
        setTowerSprite();
    }

    public void setTowerSprite()
    {
        switch (thisTowerType)
        {
            case towerBaseClass.TowerType.ROCKTHROWER:
                ChangeAnimation(rockthrower);
                //GetComponent<SpriteRenderer>().sprite = towerSprites[0];
                break;
            case towerBaseClass.TowerType.SLINGSHOT:
                ChangeAnimation(slinger);
                //GetComponent<SpriteRenderer>().sprite = towerSprites[1];
                break;
            case towerBaseClass.TowerType.SPEARTHROWER:
                ChangeAnimation(spearthrower);
                //GetComponent<SpriteRenderer>().sprite = towerSprites[2];
                break;
            case towerBaseClass.TowerType.ARCHER:
                ChangeAnimation(archer);
                //GetComponent<SpriteRenderer>().sprite = towerSprites[3];
                break;
        }
    }
    public void ChangeAnimation(Sprite[] animationToChangeTo)
    {
        if(currentAnimation != animationToChangeTo)
        {
            currentAnimation = animationToChangeTo;
            animationSpeed = fireRate / currentAnimation.Length;

            frameIndex = 0;            
        }
    }
    IEnumerator AnimationAndFiringRoutine()
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

                    if (frameIndex == 7 && target != null)
                    {
                        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                        projectile.GetComponent<playerProjectile>().setProjectileStats(thisTowerType, damage, critChance, critDamage);
                        projectile.GetComponent<SpriteRenderer>().sprite = projectileSprites[(int)thisTowerType];
                    }

                
                    yield return new WaitForSeconds(animationSpeed);
                
            }
            yield return null;
        }
    }
    void OnMouseDown()
    {
        if (FindFirstObjectByType<TowerUpgradeScreen>() != null) return;

        Time.timeScale = 0f;
        upgradeTowerStats();
    }

    private void upgradeTowerStats()
    {
        Canvas canvas = FindFirstObjectByType<Canvas>();
        GameObject screen = Instantiate(upgradeScreenPrefab, canvas.transform);
        TowerUpgradeScreen upgradeScreen = screen.GetComponent<TowerUpgradeScreen>();
        upgradeScreen.Initialize(this);
    }
}
