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
    void Start()
    {
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
        Time.timeScale = 1f;
        Destroy(gameObject);
    }

    private void RefreshStats()
    {
        string damageDisplay = targetTower.damage.ToString();
        string fireRateDisplay = targetTower.fireRate.ToString();
        string critChanceDisplay = targetTower.critChance.ToString();
        string critDamageDisplay = ((targetTower.critDamage -1 ) * 100).ToString();
        if (statsText != null)
            statsText.text = $"Damage: {damageDisplay:F1}\n\nFire Rate: {fireRateDisplay:F2}/s\n\nCrit Chance: {critChanceDisplay:F3}%\n\nCrit Damage: {critDamageDisplay:F4}";
    }
}
