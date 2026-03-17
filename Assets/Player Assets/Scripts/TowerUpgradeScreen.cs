using UnityEngine;
using TMPro;
using Mono.Cecil;

public class TowerUpgradeScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text statsText;
    [SerializeField] private GameObject[] towerTypeButtons;
    private const float DamageUpgradeAmount = 5f;
    private const float FireRateReductionAmount = 0.1f;
    private const float MinFireRate = 0.1f;
    GameObject parent;
    private towerScript targetTower;
    GameObject[] Towers;
    GameObject[] Bulles;
    void Start()
    {
        Towers = GameObject.FindGameObjectsWithTag("Tower");
        foreach (GameObject tower in Towers)
        {
            tower.SetActive(false);
        }
        Bulles = GameObject.FindGameObjectsWithTag("Bullet");
        foreach (GameObject bullet in Bulles)
        {
            bullet.SetActive(false);
        }
        parent = GameObject.Find("Canvas");
        gameObject.transform.SetParent(parent.transform);
        transform.position = parent.transform.position;
        
    }

    public void Initialize(towerScript tower)
    {
        targetTower = tower;
        towerTypeButton();
        RefreshStats();
    }

    public void UpgradeDamage()
    {
        targetTower.damage *= 1.05f;
        RefreshStats();
    }

    public void UpgradeFireRate()
    {
        targetTower.fireRate *= 0.95f;
        targetTower.animationSpeed = targetTower.fireRate / targetTower.currentAnimation.Length;
        RefreshStats();
    }

    public void UpgradeCritChance()
    {
        targetTower.critChance *= 1.05f;
        RefreshStats();
    }

    public void UpgradeCritDamage()
    {
        targetTower.critDamage *= 1.05f;
        RefreshStats();
    }

    public void CloseScreen()
    {
        foreach (GameObject tower in Towers)
        {
            tower.SetActive(true);
        }
        foreach (GameObject bullet in Bulles)
        {
            bullet.SetActive(true);
        }
        Time.timeScale = 1f;
        Destroy(gameObject);
    }
    //Get this tower's type and upgrade it to the selected type, if it's already at the highest type then just refresh stats. Rockthrower -> Slingshot -> Spearthrower or Archer
    public void towerTypeButton()
    {
        switch (targetTower.thisTowerType)
        {
            case towerBaseClass.TowerType.ROCKTHROWER:
                towerTypeButtons[0].GetComponent<UnityEngine.UI.Button>().interactable = true;
                towerTypeButtons[1].GetComponent<UnityEngine.UI.Button>().interactable = false;
                towerTypeButtons[2].GetComponent<UnityEngine.UI.Button>().interactable = false;
                towerTypeButtons[0].GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => targetTower.setTowerType(towerBaseClass.TowerType.SLINGSHOT));
                towerTypeButtons[0].GetComponentInChildren<TMP_Text>().text = "Upgrade to Slingshot";
                towerTypeButtons[1].GetComponentInChildren<TMP_Text>().text = "No valid upgrade";
                towerTypeButtons[2].GetComponentInChildren<TMP_Text>().text = "No valid upgrade";
                break;
            case towerBaseClass.TowerType.SLINGSHOT:
                towerTypeButtons[0].GetComponent<UnityEngine.UI.Button>().interactable = true;
                towerTypeButtons[1].GetComponent<UnityEngine.UI.Button>().interactable = true;
                towerTypeButtons[2].GetComponent<UnityEngine.UI.Button>().interactable = false;
                towerTypeButtons[0].GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => targetTower.setTowerType(towerBaseClass.TowerType.SPEARTHROWER));
                towerTypeButtons[1].GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => targetTower.setTowerType(towerBaseClass.TowerType.ARCHER));
                towerTypeButtons[0].GetComponentInChildren<TMP_Text>().text = "Upgrade to Spearthrower";
                towerTypeButtons[1].GetComponentInChildren<TMP_Text>().text = "Upgrade to Archer";
                towerTypeButtons[2].GetComponentInChildren<TMP_Text>().text = "No valid upgrade";
                break;
        }
        RefreshStats();
    }

    private void RefreshStats()
    {
        float bulletsPerSecond = 1f / targetTower.fireRate;
        string damageDisplay = targetTower.damage.ToString("F1");
        string fireRateDisplay = bulletsPerSecond.ToString("F2");
        string critChanceDisplay = targetTower.critChance.ToString("F1");
        string critDamageDisplay = ((targetTower.critDamage -1 ) * 100).ToString("F0");        
        if (statsText != null)
            statsText.text = $"Damage: {damageDisplay}\n\nFire Rate: {fireRateDisplay}/s\n\nCrit Chance: {critChanceDisplay}%\n\nCrit Damage: {critDamageDisplay}%";
    }
}
