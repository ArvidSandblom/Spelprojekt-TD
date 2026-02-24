using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class gameManager : MonoBehaviour
{
    private static gameManager gameManagerInstance;
    public GameObject tower;
    
    [SerializeField] private Transform[] innerTowerPositions = new Transform[4];
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    void Awake()
    {
        DontDestroyOnLoad (this);
            
        if (gameManagerInstance == null) 
        {
            gameManagerInstance = this;
        } 
        else 
        {
            Destroy(this.gameObject);
        }
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
        //GameObject.Find("playerStats").GetComponent<playerStats>().resetPlayerStats();
        SceneManager.LoadScene(1);

    }    
    public void addTower(string towerType)
    {
        Vector3 towerPos;
        towerBaseClass.TowerType type;
        GameObject addedTower = Instantiate(tower, new Vector3(0, 0, 0), Quaternion.identity); 
        switch (towerType)
        {
            case "ROCKTHROWER":
                type = towerBaseClass.TowerType.ROCKTHROWER;
                towerPos = new Vector3(2, 2, 0);
                break;
            case "SLINGSHOT":
                type = towerBaseClass.TowerType.SLINGSHOT;
                towerPos = new Vector3(-2, 2, 0);
                break;
            case "SPEARTHROWER":
                type = towerBaseClass.TowerType.SPEARTHROWER;
                towerPos = new Vector3(-2, -2, 0);
                break;
            case "ARCHER":
                type = towerBaseClass.TowerType.ARCHER;
                towerPos = new Vector3(2, -2, 0);
                break;
            default:
                type = towerBaseClass.TowerType.ROCKTHROWER;
                towerPos = new Vector3(2, 2, 0);
                break;
        }
        addedTower.transform.position = towerPos;
        addedTower.transform.localScale = new Vector3(3, 3, 1);
        addedTower.GetComponent<towerScript>().setTowerType(type);
    }    
    int towerIndex = 0;
    public void instantiateBaseTower()
    {
        if (towerIndex < innerTowerPositions.Length)
        {
            Instantiate(tower, innerTowerPositions[towerIndex].position, Quaternion.identity).GetComponent<towerScript>().setTowerType(0);
            towerIndex++;
        }
    }

}
