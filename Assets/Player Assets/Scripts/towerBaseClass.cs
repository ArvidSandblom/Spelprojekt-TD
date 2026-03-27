using UnityEngine;

public class towerBaseClass : MonoBehaviour
{
    public enum TowerType
    {
        ROCKTHROWER,
        SLINGSHOT,
        SPEARTHROWER,
        ARCHER,

    }    
    public float damage;
    public float fireRate;
    public bool findClosest;
    public bool findStrongest;
    public float critChance;
    public float critDamage;
    public float health;
    public TowerType towerType
    {
        get;
        set;
    }
    public float Damage
    {
        get;
        set;
    }
    public float FireRate
    {
        get;
        set;
    }    
    public bool FindClosest
    {
        get;
        set;
    }
    public bool FindStrongest
    {
        get;
        set;
    }
    public float CritChance
    {
        get;
        set;
    }
    public float CritDamage
    {
        get;
        set;
    }
    public float Health
    {
        get;
        set;
    }


}

