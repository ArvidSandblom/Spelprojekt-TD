using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerUpgradeScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text statsText;
    [SerializeField] private TMP_Text towerTypeText;
    [SerializeField] private GameObject[] towerTypeButtons;

    // Stat upgrade buttons — assign in the prefab Inspector
    [SerializeField] private Button upgradeDamageButton;
    [SerializeField] private Button upgradeFireRateButton;
    [SerializeField] private Button upgradeCritChanceButton;
    [SerializeField] private Button upgradeCritDamageButton;

    // Type upgrade costs
    private const float RockthrowerToSlingshotCost = 150f;
    private const float SlingshotToSpearthrowerCost = 300f;
    private const float SlingshotToArcherCost = 350f;

    private playerStats playerStatsComp;
    private GameObject parent;
    private towerScript targetTower;
    private GameObject[] Towers;
    private GameObject[] Bullets;

    void Start()
    {
        GameObject playerStatsObj = GameObject.Find("playerStats");
        playerStatsComp = playerStatsObj.GetComponent<playerStats>();

        Towers = GameObject.FindGameObjectsWithTag("Tower");
        foreach (GameObject tower in Towers)
            tower.SetActive(false);

        Bullets = GameObject.FindGameObjectsWithTag("Bullet");
        foreach (GameObject bullet in Bullets)
            bullet.SetActive(false);

        parent = GameObject.Find("Canvas");
        gameObject.transform.SetParent(parent.transform);
        transform.position = parent.transform.position;
    }

    /// <summary>
    /// Sets the target tower and populates the upgrade screen.
    /// </summary>
    public void Initialize(towerScript tower)
    {
        targetTower = tower;
        towerTypeButton();
        RefreshStats();
    }

    // Returns the gold cost for the next stat upgrade at the given upgrade index.
    private float GetStatUpgradeCost(int currentIndex) =>
        Mathf.Round(targetTower.UpgradeCost * (1f + currentIndex * 0.5f));

    // Deducts cost and returns true; returns false without deducting if gold is insufficient.
    private bool TrySpendGold(float cost)
    {
        if (playerStatsComp.gold < cost) return false;
        playerStatsComp.gold -= cost;
        return true;
    }

    /// <summary>Upgrades the tower's damage stat if the player can afford it.</summary>
    public void UpgradeDamage()
    {
        float cost = GetStatUpgradeCost(targetTower.damageIndex);
        if (!TrySpendGold(cost)) return;
        targetTower.damageIndex += 1;
        targetTower.applyUpgrades();
        RefreshStats();
    }

    /// <summary>Upgrades the tower's fire rate stat if the player can afford it.</summary>
    public void UpgradeFireRate()
    {
        float cost = GetStatUpgradeCost(targetTower.fireRateIndex);
        if (!TrySpendGold(cost)) return;
        targetTower.fireRateIndex += 1;
        targetTower.applyUpgrades();
        RefreshStats();
    }

    /// <summary>Upgrades the tower's crit chance stat if the player can afford it.</summary>
    public void UpgradeCritChance()
    {
        float cost = GetStatUpgradeCost(targetTower.critChanceIndex);
        if (!TrySpendGold(cost)) return;
        targetTower.critChanceIndex += 1;
        targetTower.applyUpgrades();
        RefreshStats();
    }

    /// <summary>Upgrades the tower's crit damage stat if the player can afford it.</summary>
    public void UpgradeCritDamage()
    {
        float cost = GetStatUpgradeCost(targetTower.critDamageIndex);
        if (!TrySpendGold(cost)) return;
        targetTower.critDamageIndex += 1;
        targetTower.applyUpgrades();
        RefreshStats();
    }

    /// <summary>Closes the upgrade screen and resumes the game.</summary>
    public void CloseScreen()
    {
        foreach (GameObject tower in Towers)
            tower.SetActive(true);
        foreach (GameObject bullet in Bullets)
            bullet.SetActive(true);
        Time.timeScale = 1f;
        Destroy(gameObject);
    }

    /// <summary>
    /// Configures the tower-type upgrade buttons based on the current tower type.
    /// Rockthrower -> Slingshot -> Spearthrower or Archer.
    /// </summary>
    public void towerTypeButton()
    {
        if (targetTower.thisTowerType == towerBaseClass.TowerType.ROCKTHROWER)
        {
            SetTypeButton(0, true, towerBaseClass.TowerType.SLINGSHOT, RockthrowerToSlingshotCost, "Upgrade to Slingshot");
            SetTypeButton(1, false, towerBaseClass.TowerType.ROCKTHROWER, 0f, "No valid upgrade");
            SetTypeButton(2, false, towerBaseClass.TowerType.ROCKTHROWER, 0f, "No valid upgrade");
        }
        else if (targetTower.thisTowerType == towerBaseClass.TowerType.SLINGSHOT
                 && playerStatsComp.currentAge >= 1)
        {
            SetTypeButton(0, true, towerBaseClass.TowerType.SPEARTHROWER, SlingshotToSpearthrowerCost, "Upgrade to Spearthrower");
            SetTypeButton(1, true, towerBaseClass.TowerType.ARCHER, SlingshotToArcherCost, "Upgrade to Archer");
            SetTypeButton(2, false, towerBaseClass.TowerType.ROCKTHROWER, 0f, "No valid upgrade");
        }
    }

    // Wires a single type-upgrade button; clears previous listeners to prevent duplicates.
    private void SetTypeButton(int index, bool interactable, towerBaseClass.TowerType targetType, float cost, string label)
    {
        Button btn = towerTypeButtons[index].GetComponent<Button>();
        btn.onClick.RemoveAllListeners();
        btn.interactable = interactable;

        TMP_Text btnText = towerTypeButtons[index].GetComponentInChildren<TMP_Text>();
        if (interactable && cost > 0f)
        {
            btnText.text = $"{label}\n{cost:F0}g";
            btn.onClick.AddListener(() => UpgradeToType(targetType, cost));
        }
        else
        {
            btnText.text = label;
        }
    }

    // Attempts to upgrade the tower type, spending gold if affordable.
    private void UpgradeToType(towerBaseClass.TowerType type, float cost)
    {
        if (!TrySpendGold(cost)) return;
        targetTower.setTowerType(type);
        targetTower.applyUpgrades();
        towerTypeButton();
        RefreshStats();
    }

    private void RefreshStats()
    {
        float bulletsPerSecond = 1f / targetTower.fireRate;
        string damageDisplay = targetTower.damage.ToString("F1");
        string fireRateDisplay = bulletsPerSecond.ToString("F2");
        string critChanceDisplay = targetTower.critChance.ToString("F1");
        string critDamageDisplay = ((targetTower.critDamage - 1) * 100).ToString("F0");

        float damageUpgradeCost = GetStatUpgradeCost(targetTower.damageIndex);
        float fireRateUpgradeCost = GetStatUpgradeCost(targetTower.fireRateIndex);
        float critChanceUpgradeCost = GetStatUpgradeCost(targetTower.critChanceIndex);
        float critDamageUpgradeCost = GetStatUpgradeCost(targetTower.critDamageIndex);

        if (statsText != null)
            statsText.text = $"Damage: {damageDisplay} (Cost: {damageUpgradeCost:F0}g)\n\n" +
                             $"Fire Rate: {fireRateDisplay}/s (Cost: {fireRateUpgradeCost:F0}g)\n\n" +
                             $"Crit Chance: {critChanceDisplay}% (Cost: {critChanceUpgradeCost:F0}g)\n\n" +
                             $"Crit Damage: {critDamageDisplay}% (Cost: {critDamageUpgradeCost:F0}g)";

        if (towerTypeText != null)
            towerTypeText.text = $"Current Tower Type: {targetTower.thisTowerType}";

        UpdateStatButtonLabels();
    }

    // Updates stat button labels with the current upgrade cost and disables them if unaffordable.
    private void UpdateStatButtonLabels()
    {
        if (upgradeDamageButton != null)
            RefreshStatButton(upgradeDamageButton, "Damage", targetTower.damageIndex);
        if (upgradeFireRateButton != null)
            RefreshStatButton(upgradeFireRateButton, "Fire Rate", targetTower.fireRateIndex);
        if (upgradeCritChanceButton != null)
            RefreshStatButton(upgradeCritChanceButton, "Crit Chance", targetTower.critChanceIndex);
        if (upgradeCritDamageButton != null)
            RefreshStatButton(upgradeCritDamageButton, "Crit Damage", targetTower.critDamageIndex);
    }

    private void RefreshStatButton(Button btn, string label, int currentIndex)
    {
        float cost = GetStatUpgradeCost(currentIndex);
        btn.GetComponentInChildren<TMP_Text>().text = $"{label}\n{cost:F0}g";
        btn.interactable = playerStatsComp.gold >= cost;
    }
}
