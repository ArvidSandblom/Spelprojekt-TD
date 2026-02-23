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
                findClosest = true;
                findStrongest = false;
                break;
            case TowerType.SLINGSHOT:
                damage = 5f;
                fireRate = 0.5f;
                findClosest = false;
                findStrongest = true;
                break;
            case TowerType.SPEARTHROWER:
                damage = 20f;
                fireRate = 1.5f;
                findClosest = false;
                findStrongest = true;
                break;
            case TowerType.ARCHER:
                damage = 10f;
                fireRate = 0.5f;
                findClosest = true;
                findStrongest = false;
                break;
        }        
    }
}
