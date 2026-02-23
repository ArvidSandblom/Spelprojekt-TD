using UnityEngine;

public class enemyTypes : enemyBaseClass
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void setEnemyStats(EnemyType type)
    {
        switch (type)
        {
                //Bestäm värden för fiender, ändra
                //Exemepel värden                
            case EnemyType.CAVEMAN:
                speed = 2f;
                health = 20f;
                damage = 10f;
                experienceValue = 10f;
                goldValue = 3f;
                break;
            case EnemyType.ROCKTHROWER:
                speed = 3f;
                health = 30f;
                damage = 15f;
                experienceValue = 15f;
                goldValue = 5f;
                break;
            case EnemyType.DINORIDER:
                speed = 1.5f;
                health = 80f;
                damage = 25f;
                experienceValue = 25f;
                goldValue = 10f;
                break;
        }
    }
}
