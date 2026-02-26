using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class towerScript : towerClass
{
    //Tornet förstörs vid scenbyte. LÖS
    public towerBaseClass.TowerType thisTowerType;
    GameObject playerStats;
    public Sprite[] towerSprites;
    public Sprite[] projectileSprites;
    public GameObject projectilePrefab;    

    void Start()
    {
        playerStats = GameObject.Find("playerStats");
        setTowerStats(thisTowerType);
        damage = this.damage * playerStats.GetComponent<playerStats>().damageUpgradeMultiplier;
        fireRate = this.fireRate * playerStats.GetComponent<playerStats>().fireRateUpgradeMultiplier;
        StartCoroutine(firingRoutine());
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
            projectile.GetComponent<playerProjectile>().setProjectileStats(thisTowerType, damage);
            projectile.GetComponent<SpriteRenderer>().sprite = projectileSprites[(int)thisTowerType];
            yield return new WaitForSeconds(fireRate);
        }
    }
    void OnMouseDown() 
    {
        Time.timeScale = 0f;
        upgradeTower(gameObject);
    }
    void upgradeTower(GameObject tower)
    {
        
    }
}
