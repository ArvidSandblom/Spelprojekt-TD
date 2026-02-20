// Gammal Kod, icke kommenterad kod från Bezi. Feltesta och skriv om
// using System.Collections;
// using UnityEngine;
// public class baseScript : MonoBehaviour
// {
//     public static bool alive = true;
//     public GameObject baseProjectile;
//     public float fireRate;
//     public float damage;
//     GameObject playerStats;


//     // Start is called once before the first execution of Update after the MonoBehaviour is created
//     void Start()
//     {
//         playerStats = GameObject.Find("playerStats");
        
//         fireRate = playerStats.GetComponent<playerStats>().fireRate;
//         damage = playerStats.GetComponent<playerStats>().damage;
//         StartCoroutine(firingRoutine());
//     }

//     void Update()
//     {
//         if (playerStats.GetComponent<playerStats>().health <= 0)
//         {
//             alive = false;
//             gameObject.GetComponent<SpriteRenderer>().color = new Color(255f, 0f, 0f, 1f);
//         }
//     }

//     IEnumerator firingRoutine()
//     {
//         while (alive)
//         {
//             GameObject bullet = Instantiate(baseProjectile, transform.position, Quaternion.identity);
//             bullet.GetComponent<playerProjectile>().damage = playerStats.GetComponent<playerStats>().damage * playerStats.GetComponent<playerStats>().damageUpgradeMultiplier;
//             yield return new WaitForSeconds(fireRate);
//         }
//     }
    
// }
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

    void Update()
    {
        if (playerStatsComponent != null && playerStatsComponent.health <= 0)
        {
            alive = false;
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, 1f);
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
                
                if (projectileScript != null)
                {
                    projectileScript.damage = currentDamage;
                    Debug.Log($"Projectile instantiated with damage: {currentDamage}");
                }
            }
            
            yield return new WaitForSeconds(fireRate);
        }
    }
}
