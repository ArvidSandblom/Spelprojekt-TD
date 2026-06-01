using UnityEngine;

public class towerClass : towerBaseClass
{
    public void setTowerStats(TowerType type)
    {
        switch (type)
        {
                //Bestäm värden för torn, ändra
                //Exemepel värden                
            case TowerType.ROCKTHROWER:
                damage = 10f;
                fireRate = 1f;
                critChance = 10f;
                critDamage = 1.5f;
                health = 100f;
                findClosest = true;
                findStrongest = false;
                UpgradeCost = 50f;
                break;
            case TowerType.SLINGSHOT:
                damage = 5f;
                fireRate = 0.5f;                
                critChance = 10f;
                critDamage = 1.5f;
                health = 100f;
                findClosest = false;
                findStrongest = true;
                UpgradeCost = 75f;
                break;
            case TowerType.SPEARTHROWER:
                damage = 20f;
                fireRate = 1.5f;
                critChance = 10f;
                critDamage = 1.5f;
                health = 100f;
                findClosest = false;
                findStrongest = true;
                UpgradeCost = 100f;
                break;
            case TowerType.ARCHER:
                damage = 10f;
                fireRate = 0.5f;
                critChance = 10f;
                critDamage = 1.5f;
                health = 100f;
                findClosest = true;
                findStrongest = false;
                UpgradeCost = 125f;
                break;
        }        
    }
}
