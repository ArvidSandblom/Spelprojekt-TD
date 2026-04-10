using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class towerScript : towerClass
{
    public towerBaseClass.TowerType thisTowerType;
    public GameObject projectilePrefab;
    [SerializeField] Sprite[] projectileRock;
    [SerializeField] Sprite[] projectileSlinger;
    [SerializeField] Sprite[] projectileSpear;
    [SerializeField] Sprite[] projectileArcher;
    [SerializeField] Sprite[] rockthrower;
    [SerializeField] Sprite[] slinger;
    [SerializeField] Sprite[]  spearthrower;
    [SerializeField] Sprite[] archer;
    
    public int damageIndex;
    public int fireRateIndex;
    public int critChanceIndex;
    public int critDamageIndex;
    Image healthBar;

    
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
        healthBar = childSprite.transform.Find("towerHealth").GetComponent<Image>();
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
            spriteRenderer.flipX = target.position.x < transform.position.x;
        }
        if (healthBar != null)
        {
            healthBar.fillAmount = health / 100f;
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
        applyUpgrades();
        StartCoroutine(AnimationAndFiringRoutine());
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }


    public void applyUpgrades()
    {
        switch (thisTowerType)
        {
            case TowerType.ROCKTHROWER:
                damage = 10f; fireRate = 1f; critChance = 10f; critDamage = 1.5f;
                break;
            case TowerType.SLINGSHOT:
                damage = 5f; fireRate = 0.5f; critChance = 10f; critDamage = 1.5f;
                break;
            case TowerType.SPEARTHROWER:
                damage = 20f; fireRate = 1.5f; critChance = 10f; critDamage = 1.5f;
                break;
            case TowerType.ARCHER:
                damage = 10f; fireRate = 0.5f; critChance = 10f; critDamage = 1.5f;
                break;
        }

        for (int i = 0; i < damageIndex; i++)
            damage *= 1.05f;

        for (int i = 0; i < fireRateIndex; i++)
            fireRate *= 0.95f;

        for (int i = 0; i < critChanceIndex; i++)
            critChance *= 1.05f;

        for (int i = 0; i < critDamageIndex; i++)
            critDamage *= 1.05f;

        // Keep animationSpeed in sync whenever fireRate changes
        if (currentAnimation != null && currentAnimation.Length > 0)
            animationSpeed = fireRate / currentAnimation.Length;
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
                break;
            case towerBaseClass.TowerType.SLINGSHOT:
                ChangeAnimation(slinger);
                break;
            case towerBaseClass.TowerType.SPEARTHROWER:
                ChangeAnimation(spearthrower);
                break;
            case towerBaseClass.TowerType.ARCHER:
                ChangeAnimation(archer);
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

                    if (frameIndex == 7 && target != null)
                    {
                        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                        projectile.GetComponent<playerProjectile>().setProjectileStats(thisTowerType, damage, critChance, critDamage);
                        switch (thisTowerType)
                        {
                            case towerBaseClass.TowerType.ROCKTHROWER:
                                projectile.GetComponent<playerProjectile>().currentAnimation = projectileRock;
                                break;
                            case towerBaseClass.TowerType.SLINGSHOT:
                                projectile.GetComponent<playerProjectile>().currentAnimation = projectileSlinger;
                                break;
                            case towerBaseClass.TowerType.SPEARTHROWER:
                                projectile.GetComponent<playerProjectile>().currentAnimation = projectileSpear;
                                break;
                            case towerBaseClass.TowerType.ARCHER:
                                projectile.GetComponent<playerProjectile>().currentAnimation = projectileArcher;
                                break;
                        }
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
    public void setAsParentObject(Transform obj)
    {
        obj.transform.SetParent(transform);
        obj.transform.localPosition = Vector3.zero;
    }
    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0f)
        {
            Destroy(gameObject);
        }
    }

    private void upgradeTowerStats()
    {
        Canvas canvas = FindFirstObjectByType<Canvas>();
        GameObject screen = Instantiate(upgradeScreenPrefab, canvas.transform);
        TowerUpgradeScreen upgradeScreen = screen.GetComponent<TowerUpgradeScreen>();
        upgradeScreen.Initialize(this);
    }
}
