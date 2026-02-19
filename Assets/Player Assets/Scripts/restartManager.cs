using UnityEngine;

public class restartManager : MonoBehaviour
{
    GameObject playerStats;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerStats = GameObject.Find("playerStats");
    }
}
