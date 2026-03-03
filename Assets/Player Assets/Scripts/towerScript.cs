using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class towerScript : towerClass
{
    public towerBaseClass.TowerType thisTowerType;
    public Sprite[] towerSprites;
    public Sprite[] projectileSprites;
    public GameObject projectilePrefab;

    [SerializeField] private GameObject upgradeScreenPrefab;

    private GameObject playerStats;
    private bool statsInitialized = false;

    void Start()
    {
        playerStats = GameObject.Find("playerStats");
        setTowerStats(thisTowerType);
        damage = this.damage * playerStats.GetComponent<playerStats>().damageUpgradeMultiplier;
        fireRate = this.fireRate * playerStats.GetComponent<playerStats>().fireRateUpgradeMultiplier;
        statsInitialized = true;
        StartCoroutine(firingRoutine());
    }

    void OnEnable()
    {
        // On first activation Start() hasn't run yet, so stats aren't ready
        if (!statsInitialized) return;

        StartCoroutine(firingRoutine());
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
                GetComponent<SpriteRenderer>().sprite = towerSprites[0];
                break;
            case towerBaseClass.TowerType.SLINGSHOT:
                GetComponent<SpriteRenderer>().sprite = towerSprites[1];
                break;
            case towerBaseClass.TowerType.SPEARTHROWER:
                GetComponent<SpriteRenderer>().sprite = towerSprites[2];
                break;
            case towerBaseClass.TowerType.ARCHER:
                GetComponent<SpriteRenderer>().sprite = towerSprites[3];
                break;
        }
    }

    IEnumerator firingRoutine()
    {
        while (true)
        {
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            projectile.GetComponent<playerProjectile>().setProjectileStats(thisTowerType, damage, critChance, critDamage);
            projectile.GetComponent<SpriteRenderer>().sprite = projectileSprites[(int)thisTowerType];
            yield return new WaitForSeconds(fireRate);
        }
    }

    void OnMouseDown()
    {
        if (FindFirstObjectByType<TowerUpgradeScreen>() != null) return;

        Time.timeScale = 0f;
        upgradeTower();
    }

    private void upgradeTower()
    {
        if (upgradeScreenPrefab == null)
        {
            Debug.LogError("towerScript: upgradeScreenPrefab is not assigned.", this);
            return;
        }

        Canvas canvas = FindFirstObjectByType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("towerScript: No Canvas found in the scene to parent the upgrade screen.", this);
            return;
        }

        GameObject screen = Instantiate(upgradeScreenPrefab, canvas.transform);
        TowerUpgradeScreen upgradeScreen = screen.GetComponent<TowerUpgradeScreen>();
        upgradeScreen.Initialize(this);
    }
}
