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
    string perkText = "";
    public int currentLevel;    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerStats = GameObject.Find("playerStats");
        gameManager = GameObject.Find("gameManager");   
        GameObject levelUpText = GameObject.Find("text1");
        instantiateButtons();         
        //currentLevel = playerStats.GetComponent<playerStats>().level;
        levelUpText.GetComponent<TMP_Text>().text = $"Congratulations on reaching the next level!";
    }    
    void instantiateButtons()
    {
        List<System.Action> perks = new List<System.Action>
        {
            attackSpeed,
            health,
            gold,
            damage,
            experience
        };

        for (int i = 0; i < 3; i++)
        {
            int randomIndex = Random.Range(0, perks.Count);

            System.Action selectedPerk = perks[randomIndex];
            perks.RemoveAt(randomIndex);

            selectedPerk(); // sets perkText etc.

            GameObject button = Instantiate(levelUpButtonPrefab, levelUpScreen);
            RectTransform buttonRect = button.GetComponent<RectTransform>();
            buttonRect.anchoredPosition = new Vector2(-200 + (i * 200), 0); 
            button.GetComponentInChildren<TMP_Text>().text = perkText;
            button.GetComponent<Button>().onClick.AddListener(levelPerkPlaceHolder);
        }
    }

    void levelPerkPlaceHolder()
    {
        //placeholder for now, will be replaced with actual perks later
        //playerStats.GetComponent<playerStats>().level++;
        gameManager.GetComponent<generalManager>().continueGame();

    }

    void attackSpeed()
    {
        perkText = "Attack Speed";   
    }
    void health()
    {
        perkText = "Health";
    }
    void gold()
    {
        perkText = "Gold";
    }
    void damage()
    {
        perkText = "Damage";
    }
    void experience()
    {
        perkText = "Experience";
    }
}
