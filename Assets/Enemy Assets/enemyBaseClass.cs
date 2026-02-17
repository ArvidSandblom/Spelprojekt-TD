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
    public float health;
    public float damage;
    public float speed;
    public float experienceValue;

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
    public EnemyType enemyType
    {
        get;
        set;
    }
    public void Die()
    {
        baseScript.experiencePoints += experienceValue;
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

