using System.Collections;
using UnityEngine;
public class baseScript : MonoBehaviour
{
    public static bool alive = true;
    public GameObject baseProjectile;
    public float fireRate;
    public float damage;
    GameObject playerStats;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerStats = GameObject.Find("playerStats");
        fireRate = playerStats.GetComponent<playerStats>().fireRate;
        damage = playerStats.GetComponent<playerStats>().damage;
        StartCoroutine(firingRoutine());
    }

    void Update()
    {
        if (playerStats.GetComponent<playerStats>().health <= 0)
        {
            alive = false;
            gameObject.GetComponent<SpriteRenderer>().color = new Color(255f, 0f, 0f, 1f);
        }
    }

    IEnumerator firingRoutine()
    {
        while (alive)
        {
            GameObject bullet = Instantiate(this.baseProjectile, transform.position, Quaternion.identity);
            bullet.GetComponent<playerProjectile>().damage = this.damage;
            yield return new WaitForSeconds(fireRate);
        }
    }
    
}
