using UnityEngine;
using UnityEngine.UIElements;

public class towerPlatformScript : MonoBehaviour
{
    [SerializeField] GameObject playerStats;
    [SerializeField] Sprite[] platformSprites;
    SpriteRenderer spriteRenderer;
    int ageIndex;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerStats = GameObject.Find("playerStats");
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        ageIndex = playerStats.GetComponent<playerStats>().currentAge;
        spriteRenderer.sprite = platformSprites[ageIndex];
    }

}
