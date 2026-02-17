using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class baseScript : MonoBehaviour
{
    public static bool alive = true;
    public float baseHP = 100;
    private Image healthBar;
    public GameObject baseProjectile;
    public float fireRate = 1f;
    public float experiencePoints = 0f;
    public int level = 1;
    public float experienceToNextLevel = 100f;
    public float gold;
    private TMP_Text levelText;
    private Image levelUpBar;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthBar = GameObject.Find("baseHPGreen").GetComponent<Image>();
        levelText = GameObject.Find("currentLevelText").GetComponent<TMP_Text>();
        levelUpBar = GameObject.Find("baseXPBar").GetComponent<Image>();
        StartCoroutine(firingRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = baseHP / 100f;
        levelUpBar.fillAmount = experiencePoints / experienceToNextLevel;
        if (experiencePoints >= experienceToNextLevel)
        {
            LevelUp();
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    IEnumerator firingRoutine()
    {
        while (alive)
        {
            Instantiate(baseProjectile, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(fireRate);
        }
    }
    void LevelUp()
    {
        level++;
        experiencePoints = 0f;
        experienceToNextLevel *= 1.2f;
        levelText.text = level.ToString();
    }
    void chooseLevelUp()
    {
        
    }
}
