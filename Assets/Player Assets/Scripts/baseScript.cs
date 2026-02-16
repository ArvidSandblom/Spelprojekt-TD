using UnityEngine;
using UnityEngine.UI;
public class baseScript : MonoBehaviour
{
    public static bool alive = true;
    public float baseHP = 100;
    private Image healthBar;
    public GameObject baseProjectile;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthBar = GameObject.Find("baseHPGreen").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = baseHP / 100f;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
}
