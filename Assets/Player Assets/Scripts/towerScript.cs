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
    int frameIndex = 0;
    public float animationSpeed; // Time between frames in seconds

    [SerializeField] private GameObject upgradeScreenPrefab;

    private GameObject playerStats;
    private bool statsInitialized = false;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
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
                    if (frameIndex == 7)
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

    // IEnumerator firingRoutine()
    // {
    //     while (true)
    //     {
    //         GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
    //         projectile.GetComponent<playerProjectile>().setProjectileStats(thisTowerType, damage, critChance, critDamage);
    //         projectile.GetComponent<SpriteRenderer>().sprite = projectileSprites[(int)thisTowerType];
    //         yield return new WaitForSeconds(fireRate);
    //     }
    // }

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
