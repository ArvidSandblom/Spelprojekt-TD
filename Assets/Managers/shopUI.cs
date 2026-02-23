using UnityEngine;

public class shopUI : MonoBehaviour
{
    GameObject playerStats;
    GameObject gameManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerStats = GameObject.Find("playerStats");
        gameManager = GameObject.Find("gameManager");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
