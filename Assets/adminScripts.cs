using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class adminScripts : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown goldDropdown;
    [SerializeField] private TMP_Dropdown xpMultiplierDropdown;

    private static readonly float[] GoldAmountOptions = { 100f, 200f, 300f, 400f, 500f, 1000f, 5000f, 10000f };
    private static readonly float[] XpMultiplierOptions = { 1f, 2f, 5f, 10f, 50f };

    private playerStats playerStatsInstance;

    void Start()
    {
        playerStatsInstance = FindFirstObjectByType<playerStats>();

        PopulateGoldDropdown();
        PopulateXpDropdown();
        goldDropdown.onValueChanged.AddListener(OnGoldAmountChanged);
        xpMultiplierDropdown.onValueChanged.AddListener(OnXpMultiplierChanged);
    }

    private void PopulateGoldDropdown()
    {
        goldDropdown.ClearOptions();
        var options = new List<string>();
        foreach (float amount in GoldAmountOptions)
            options.Add($"+{amount:F0}g");
        goldDropdown.AddOptions(options);
    }

    private void PopulateXpDropdown()
    {
        xpMultiplierDropdown.ClearOptions();
        var options = new List<string>();
        foreach (float multiplier in XpMultiplierOptions)
            options.Add($"{multiplier}x XP");
        xpMultiplierDropdown.AddOptions(options);
    }

    private void OnGoldAmountChanged(int index)
    {
        AddGold(GoldAmountOptions[index]);
    }

    private void OnXpMultiplierChanged(int index)
    {
        SetXpMultiplier(XpMultiplierOptions[index]);
    }

    /// <summary>Adds the given amount of gold to the player.</summary>
    private void AddGold(float amount)
    {
        if (playerStatsInstance != null)
            playerStatsInstance.gold += amount;
    }

    /// <summary>Sets the XP multiplier to an absolute value.</summary>
    private void SetXpMultiplier(float multiplier)
    {
        if (playerStatsInstance != null)
            playerStatsInstance.xpMultiplierUpgradeAmount = multiplier;
    }
}
