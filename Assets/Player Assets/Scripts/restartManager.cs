using UnityEngine;
using UnityEngine.SceneManagement;

public class restartManager : MonoBehaviour
{
    GameObject playerStats;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerStats = GameObject.Find("playerStats");
    }
    public void loadGameScene()
    {
        SceneManager.LoadScene(1);
    }
    
}
