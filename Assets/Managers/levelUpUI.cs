using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class levelUpUI : MonoBehaviour
{
    public GameObject levelUpButtonPrefab;
    public Transform levelUpScreen;
    private GameObject playerStats;
    GameObject gameManager;
    public int currentLevel;
    GameObject[] Towers;
    GameObject enemyManager;
    private List<int> selectedPerkIndices = new List<int>();
    
    void Start()
    {
        Towers = GameObject.FindGameObjectsWithTag("Tower"); 
        foreach (GameObject tower in Towers)
        {
            tower.SetActive(false);
        }
        enemyManager = GameObject.Find("enemyManager");
        enemyManager.GetComponent<enemySpawner>().canSpawn = false;
        playerStats = GameObject.Find("playerStats");
        gameManager = GameObject.Find("gameManager");   
        GameObject levelUpText = GameObject.Find("text1");
        instantiateButtons();         
        levelUpText.GetComponent<TMP_Text>().text = $"Congratulations on reaching the next level!";
    }    
    
    void instantiateButtons()
    {
        string[] perkNames = new string[]
        {
            "Attack Speed +5%",
            "Health +5%",
            "Gold +5%",
            "Damage +5%",
            "Experience +5%"
        };

        List<int> availableIndices = new List<int> { 0, 1, 2, 3, 4 };

        for (int i = 0; i < 3; i++)
        {
            int randomListIndex = Random.Range(0, availableIndices.Count);
            int perkIndex = availableIndices[randomListIndex];
            availableIndices.RemoveAt(randomListIndex);
            
            selectedPerkIndices.Add(perkIndex);

            GameObject button = Instantiate(levelUpButtonPrefab, levelUpScreen);
            RectTransform buttonRect = button.GetComponent<RectTransform>();
            buttonRect.anchoredPosition = new Vector2(-200 + (i * 200), 0); 
            button.GetComponentInChildren<TMP_Text>().text = perkNames[perkIndex];
            
            int buttonIndex = i;
            button.GetComponent<Button>().onClick.AddListener(() => OnPerkButtonClicked(buttonIndex));
        }
    }

    void OnPerkButtonClicked(int buttonIndex)
    {
        int perkIndex = selectedPerkIndices[buttonIndex];
        
        switch (perkIndex)
        {
            case 0:
                ApplyAttackSpeed();
                break;
            case 1:
                ApplyHealth();
                break;
            case 2:
                ApplyGold();
                break;
            case 3:
                ApplyDamage();
                break;
            case 4:
                ApplyExperience();
                break;
        }
        foreach (GameObject tower in Towers)
        {
            tower.SetActive(true);
        }
        enemyManager.GetComponent<enemySpawner>().canSpawn = true;
        gameManager.GetComponent<gameManager>().continueGame();
    }

    void ApplyAttackSpeed()
    {
        playerStats.GetComponent<playerStats>().fireRateUpgradeMultiplier *= 1.05f;
    }
    
    void ApplyHealth()
    {
        playerStats.GetComponent<playerStats>().healthUpgradeMultiplier += 0.05f;
    }
    
    void ApplyGold()
    {
        playerStats.GetComponent<playerStats>().goldMultiplier += 0.05f;
    }
    
    void ApplyDamage()
    {
        playerStats.GetComponent<playerStats>().damageUpgradeMultiplier += 0.05f;
    }
    
    void ApplyExperience()
    {
        playerStats.GetComponent<playerStats>().xpMultiplierUpgradeAmount += 0.05f;
    }
}
