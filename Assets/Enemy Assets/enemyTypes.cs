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
                break;
            case EnemyType.ROCKTHROWER:
                health = 30f;
                speed = 3f;
                experienceValue = 15f;
                break;
            case EnemyType.DINORIDER:
                health = 80f;
                speed = 1.5f;
                experienceValue = 25f;
                break;
        }
    }
}
