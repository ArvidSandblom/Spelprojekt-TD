using UnityEngine;

public class enemyBaseClass : MonoBehaviour
{
    //För att spawna olika typer av fiender, använd EnemyType lista
    public enum EnemyType
    {
        CAVEMAN,
        ROCKTHROWER,
        DINORIDER
    }
    public Sprite[] cavemanWalk;
    public Sprite[] cavemanAttack;
    public Sprite[] rockThrowerWalk;
    public Sprite[] rockThrowerAttack;
    public Sprite[] dinoriderWalk;
    public Sprite[] dinoriderAttack;
    public Sprite[] rockProjectileAnimation;
    public float health;
    public float damage;
    public float speed;
    public float experienceValue;    
    public float goldValue;
    public float attackRate;
    public float attackRange;
    public bool isRanged;

    public float AttackRate
    {
        get;
        set;
    }
    public float AttackRange
    {
        get;
        set;
    }
    public float Health
    {
        get;
        set;
    }
    public float Damage
    {
        get;
        set;
    }
    public float Speed
    {
        get;
        set;
    }
    public float ExperienceValue
    {
        get;
        set;
    }
    public float GoldValue
    {
        get;
        set;
    }
    public bool IsRanged
    {
        get;
        set;
    }
    public EnemyType enemyType
    {
        get;
        set;
    }
    public Sprite[] currentAnimation
    {
        get;
        set;
    }
    public Sprite[] projectileAnimation
    {
        get;
        set;
    }
    public void Die()
    {
        GameObject.Find("playerStats").GetComponent<playerStats>().experiencePoints += experienceValue;
        GameObject.Find("playerStats").GetComponent<playerStats>().agePoints += experienceValue;
        GameObject.Find("playerStats").GetComponent<playerStats>().gold += goldValue;
        enemySpawner.enemiesDestroyed++;
        enemySpawner.UnregisterEnemy(gameObject);
        Destroy(gameObject);
    }    
    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
        {
            Die();
        }
    }

}

