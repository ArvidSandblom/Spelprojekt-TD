using UnityEngine;
using TMPro;
using Mono.Cecil;

public class TowerUpgradeScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text statsText;

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

    /// <summary>
    /// Binds the upgrade screen to the tower that was clicked.
    /// Call this immediately after instantiation.
    /// </summary>
    public void Initialize(towerScript tower)
    {
        targetTower = tower;
        RefreshStats();
    }

    /// <summary>
    /// Increases the target tower's damage. Wire to your damage upgrade button's OnClick.
    /// </summary>
    public void UpgradeDamage()
    {
        targetTower.damage *= 1.05f;
        RefreshStats();
    }

    /// <summary>
    /// Decreases the target tower's fire rate interval (faster firing). Wire to your fire rate button's OnClick.
    /// </summary>
    public void UpgradeFireRate()
    {
        targetTower.fireRate *= 0.95f;
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

    /// <summary>
    /// Closes the upgrade screen and resumes the game. Wire to your close button's OnClick.
    /// </summary>
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
