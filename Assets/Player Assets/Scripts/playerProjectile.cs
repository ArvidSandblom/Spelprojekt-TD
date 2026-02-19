using UnityEngine;

public class playerProjectile : MonoBehaviour
{
    private Transform target;
    private Vector2 targetDirection;
    public float missileSpeed = 5f;
    public float damage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = FindClosestEnemy();
        
        if (target != null)
        {
            targetDirection = (target.position - transform.position).normalized;
        }
        else
        {
            targetDirection = Vector2.zero;
        }
        
        Destroy(gameObject, 5f);

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (Vector3)targetDirection * missileSpeed * Time.deltaTime;
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

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            enemyAI enemy = collision.GetComponent<enemyAI>();
            if (enemy != null)
            {
                enemy.health -= damage;
            }
            Destroy(this.gameObject);
        }
    }
}
