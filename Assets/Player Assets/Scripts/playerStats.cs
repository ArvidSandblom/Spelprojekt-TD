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
    public float xpMultiplierUpgradeAmount = 1f;
    public float goldMultiplier = 1f;
    [HideInInspector]public float damageUpgradeMultiplier = 1f;
    public float fireRateUpgradeMultiplier = 1f;
    public float healthUpgradeMultiplier = 1f;

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
}

private void UpdateUI()
{
    if (levelText != null) 
        levelText.text = level.ToString();
    
    if (levelUpBar != null) 
        levelUpBar.fillAmount = experiencePoints / experienceToNextLevel;
    
    if (healthBar != null) 
        healthBar.fillAmount = health / 100f;
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
