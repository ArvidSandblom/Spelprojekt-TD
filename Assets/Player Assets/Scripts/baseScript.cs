using System.Collections;
using UnityEngine;

public class baseScript : MonoBehaviour
{
    public static bool alive = true;
    public GameObject baseProjectile;
    private float fireRate;
    private playerStats playerStatsComponent;

    void Start()
    {
        GameObject playerStatsObj = GameObject.Find("playerStats");
        if (playerStatsObj != null)
        {
            playerStatsComponent = playerStatsObj.GetComponent<playerStats>();
            fireRate = playerStatsComponent.fireRate;
            StartCoroutine(firingRoutine());
        }
        else
        {
            Debug.LogError("playerStats GameObject not found!");
        }
    }
    void OnEnable()
    {
        GameObject playerStatsObj = GameObject.Find("playerStats");
        if (playerStatsObj != null)
        {
            playerStatsComponent = playerStatsObj.GetComponent<playerStats>();
            fireRate = playerStatsComponent.fireRate;
            StartCoroutine(firingRoutine());
        }
        else
        {
            Debug.LogError("playerStats GameObject not found!");
        }
    }
    void OnDisable()
    {
        StopAllCoroutines();

    }

    void Update()
    {
        if (playerStatsComponent != null && playerStatsComponent.health <= 0)
        {
            alive = false;
            gameObject.GetComponentInChildren<SpriteRenderer>().color = new Color(1f, 0f, 0f, 1f);
        }
    }

    IEnumerator firingRoutine()
    {
        while (alive)
        {
            if (playerStatsComponent != null)
            {
                
                float currentDamage = playerStatsComponent.damage * playerStatsComponent.damageUpgradeMultiplier;
                
                GameObject bullet = Instantiate(baseProjectile, transform.position, Quaternion.identity);
                playerProjectile projectileScript = bullet.GetComponent<playerProjectile>();
                bullet.GetComponent<playerProjectile>().findClosest = true;
                
                if (projectileScript != null)
                {
                    projectileScript.damage = currentDamage;
                }
            }
            
            yield return new WaitForSeconds(fireRate / playerStatsComponent.fireRateUpgradeMultiplier);
        }
    }
}
