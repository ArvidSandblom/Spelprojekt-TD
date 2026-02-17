using UnityEngine;

public class playerStats : MonoBehaviour
{
    public float health;
    public float experiencePoints;
    public int level;
    public float experienceToNextLevel;
    public float fireRate;
    public float gold;
    GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        if (GameObject.Find("playerStats") != null)
        {
            Destroy(gameObject);
        }
        else
        {
            getPlayerStats();
        }
    }
    public void getPlayerStats()
    {
        player = GameObject.Find("Player");
        health = player.GetComponent<baseScript>().baseHP;
        experiencePoints = player.GetComponent<baseScript>().experiencePoints;
        level = player.GetComponent<baseScript>().level;
        experienceToNextLevel = player.GetComponent<baseScript>().experienceToNextLevel;
        fireRate = player.GetComponent<baseScript>().fireRate;
        gold = player.GetComponent<baseScript>().gold;
    }
    public void setPlayerStats()
    {
        player = GameObject.Find("Player");
        player.GetComponent<baseScript>().baseHP = health;
        player.GetComponent<baseScript>().experiencePoints = experiencePoints;
        player.GetComponent<baseScript>().level = level;
        player.GetComponent<baseScript>().experienceToNextLevel = experienceToNextLevel;
        player.GetComponent<baseScript>().fireRate = fireRate;
        player.GetComponent<baseScript>().gold = gold;
    }
    public void resetPlayerStats()
    {
        health = 100f;
        experiencePoints = 0f;
        level = 1;
        experienceToNextLevel = 100f;
        fireRate = 1f;
    }
}
