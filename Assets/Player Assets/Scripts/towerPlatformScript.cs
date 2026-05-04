using UnityEngine;
using UnityEngine.UI;

public class towerPlatformScript : towerClass
{
    [SerializeField] GameObject playerStats;
    [SerializeField] Sprite[] platformSpritesAnim;
    [SerializeField] GameObject parentStats;
    [SerializeField] Image healthBarIMG;

    SpriteRenderer spriteRenderer;
    int ageIndex;
    float currentHealth;
    float healthPercentage;
    //public Sprite[] currentAnimation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerStats = GameObject.Find("playerStats");
        spriteRenderer = GetComponent<SpriteRenderer>();
        // currentHealth uses the inherited 'health' value
    }

    // Update is called once per frame
    void Update()
    {
        
        currentHealth = parentStats.GetComponent<towerScript>().health;

        // Normalize/clamp to 0..100 so percentage checks are explicit
        healthPercentage = Mathf.Clamp(currentHealth, 0f, 100f);

        healthBarIMG.fillAmount = healthPercentage / 100f;

        ageIndex = playerStats.GetComponent<playerStats>().currentAge;

        setTowerSprite();
    }

    // Call this whenever you need to refresh the platform appearance (e.g. on damage)
    public void setTowerSprite()
    {
        // Determine which quarter band the current health is in (100-75, 74-50, 49-25, 24-0)
        //Sprite chosenSprite = null;

        if (healthPercentage >= 75f)
        {
            spriteRenderer.sprite = platformSpritesAnim[0]; // Set the sprite for 100-75% health
            //if (platformSpritesAnim.Length > 0) chosenSprite = platformSpritesAnim[0];
        }
        else if (healthPercentage >= 50f)
        {
            spriteRenderer.sprite = platformSpritesAnim[1]; // Set the sprite for 74-50% health�
            //if (platformSpritesAnim.Length > 1) chosenSprite = platformSpritesAnim[1];
        }
        else if (healthPercentage >= 25f)
        {
            spriteRenderer.sprite = platformSpritesAnim[2]; // Set the sprite for 49-25% health
            //if (platformSpritesAnim.Length > 2) chosenSprite = platformSpritesAnim[2];
        }
        else
        {
            spriteRenderer.sprite = platformSpritesAnim[3]; // Set the sprite for 24-0% health
            //if (platformSpritesAnim.Length > 3) chosenSprite = platformSpritesAnim[3];
        }

        // If your platformSpritesAnim entries are single-frame sprites, pass a single-element array
        /*if (chosenSprite != null)
        {
            ChangeAnimation(chosenSprite);
        }*/
    }

    /*public void ChangeAnimation(int animationToChangeTo)
    {
        currentAnimation[animationToChangeTo];
        // Fix: compare currentAnimation (existing field) to the new animation array
        //spriteRenderer.sprite = currentAnimation[0];
    }*/

}
