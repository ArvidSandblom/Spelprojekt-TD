using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class playerStats : MonoBehaviour
{    
    public static int currentAge = 1;
    public float health = 100f;
    public float experiencePoints = 0f;
    public float agePoints = 0f;
    public int level = 1;

    public float experienceToNextLevel = 100f;
    public float agePointsToNextAge = 1000f;
    public float fireRate = 1f;
    public float gold = 0f;
    public float damage = 20f;
    public float critChance = 10f;
    public float critDamage = 1.5f;
    public float xpMultiplierUpgradeAmount = 1f;
    public float goldMultiplier = 1f;
    public float damageUpgradeMultiplier = 1f;
    public float fireRateUpgradeMultiplier = 1f;
    public float healthUpgradeMultiplier = 1f;
    public GameObject player;
    public TMP_Text goldText;
    public static bool alive = true;
    private static playerStats playerInstance;
    private TMP_Text levelText;
    private TMP_Text ageText;
    private Image ageBar;
    private Image levelUpBar;
    private Image healthBar;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {               
        healthBar = GameObject.Find("baseHPGreen").GetComponent<Image>();
        levelText = GameObject.Find("currentLevelText").GetComponent<TMP_Text>();
        ageText = GameObject.Find("currentAgeText").GetComponent<TMP_Text>();
        ageBar = GameObject.Find("baseAgeBar").GetComponent<Image>();
        levelUpBar = GameObject.Find("baseXPBar").GetComponent<Image>();
        player = GameObject.Find("Player");
        goldText = GameObject.Find("gold").GetComponent<TMP_Text>();
        FindUIReferences();
        UpdateUI();
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
        if (ageText != null) ageText.text = "Age: " + currentAge.ToString();
        if (goldText != null) goldText.text = "Gold: " + gold.ToString("F0");
        if (healthBar != null) healthBar.fillAmount = health / 100f;
        if (levelUpBar != null) levelUpBar.fillAmount = experiencePoints / experienceToNextLevel;
        if (ageBar != null) ageBar.fillAmount = agePoints / agePointsToNextAge;
        if (experiencePoints >= experienceToNextLevel)
        {
            LevelUp();
        }
            if (agePoints >= agePointsToNextAge)
        {
            AgeUp();
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
    void AgeUp()
    {
        currentAge++;
        agePoints = 0f;
        agePointsToNextAge *= 1.5f;
        ageText.text = "Age: " + currentAge.ToString();
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindUIReferences();
        UpdateUI();
    }

    private void FindUIReferences()
    {
        GameObject ageTextObj = GameObject.Find("currentAgeText");
        if (ageTextObj != null) 
            ageText = ageTextObj.GetComponent<TMP_Text>();
        GameObject ageBarObj = GameObject.Find("baseAgeBar");
        if (ageBarObj != null) 
            ageBar = ageBarObj.GetComponent<Image>();
        GameObject healthBarObj = GameObject.Find("baseHPGreen");
        if (healthBarObj != null) 
            healthBar = healthBarObj.GetComponent<Image>();        
        GameObject levelTextObj = GameObject.Find("currentLevelText");
        if (levelTextObj != null) 
            levelText = levelTextObj.GetComponent<TMP_Text>();        
        GameObject levelUpBarObj = GameObject.Find("baseXPBar");
        if (levelUpBarObj != null) 
            levelUpBar = levelUpBarObj.GetComponent<Image>();        
        GameObject playerObj = GameObject.Find("Player");
        if (playerObj != null) 
            player = playerObj;
        GameObject goldTextObj = GameObject.Find("gold");
        if (goldTextObj != null) 
            goldText = goldTextObj.GetComponent<TMP_Text>();
    }

    private void UpdateUI()
    {
        if (levelText != null) 
            levelText.text = level.ToString();
        
        if (levelUpBar != null) 
            levelUpBar.fillAmount = experiencePoints / experienceToNextLevel;
        
        if (healthBar != null) 
            healthBar.fillAmount = health / 100f;

        if (goldText != null)            
            goldText.text = "Gold: " + gold.ToString("F0");
        if (ageText != null)
            ageText.text = "Age: " + currentAge.ToString();
    }
}
