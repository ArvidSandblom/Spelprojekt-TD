using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class generalManager : MonoBehaviour
{
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void timeScaleSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }
    }

    public void openShop()
    {
        
    }

    public void levelUp()
    {
        
    }

    public void continueGame()
    {
        SceneManager.LoadScene(1);
    }

    public void exitGame()
    {
        Application.Quit();
    }

    public void mainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void restartGame()
    {
        GameObject.Find("playerStats").GetComponent<playerStats>().resetPlayerStats();
        SceneManager.LoadScene(1);

    }
}
