using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class playerStats : MonoBehaviour
{
    public float health = 100f;
    public float experiencePoints = 0f;
    public int level = 1;
    public float experienceToNextLevel = 100f;
    public float fireRate = 1f;
    public float gold = 0f;
    public float damage = 50f;
    public GameObject player;
    public static bool alive = true;
    private static playerStats playerInstance;
    private TMP_Text levelText;
    private Image levelUpBar;
    private Image healthBar;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
        healthBar = GameObject.Find("baseHPGreen").GetComponent<Image>();
        levelText = GameObject.Find("currentLevelText").GetComponent<TMP_Text>();
        levelUpBar = GameObject.Find("baseXPBar").GetComponent<Image>();
        player = GameObject.Find("Player");
    }
    void Awake()
    {
        DontDestroyOnLoad (this);
            
        if (playerInstance == null) 
        {
            playerInstance = this;
        } 
        else 
        {
            Destroy(this.gameObject);
        }
    }
    void Update()
    {
        if (healthBar != null) healthBar.fillAmount = health / 100f;
        if (levelUpBar != null) levelUpBar.fillAmount = experiencePoints / experienceToNextLevel;
        if (experiencePoints >= experienceToNextLevel)
        {
            LevelUp();
        }
    }
    void LevelUp()
    {
        level++;
        experiencePoints = 0f;
        experienceToNextLevel *= 1.2f;
        levelText.text = level.ToString();
        SceneManager.LoadScene(2);
    }
    // public void resetPlayerStats()
    // {
    //     health = 100f;
    //     experiencePoints = 0f;
    //     level = 1;
    //     experienceToNextLevel = 100f;
    //     fireRate = 1f;
    //     damage = 20f;
    // }
}
