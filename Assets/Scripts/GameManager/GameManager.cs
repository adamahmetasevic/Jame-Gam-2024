using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [Header("XP System")]
    public int playerXP = 0;               // Total XP collected
    public int xpToNextLevel = 100;        // XP required to level up
    public int xpIncreasePerLevel = 50;    // XP needed increase per level
    public int currentLevel = 1;           // Player's level

    [Header("Upgrades")]
    public List<UpgradeData> allUpgrades;  // List of all upgrades in the game
    public UpgradePanelController upgradePanelController; // Reference to the upgrade panel

    [Header("Game State")]
    public bool isGamePaused = false;      // To pause during upgrades

    void Start()
    {
        playerXP = 0;
        currentLevel = 1;
        Time.timeScale = 1f; // Ensure game starts unpaused
    }

    public void AddXP(int xpAmount)
    {
        playerXP += xpAmount;

        // Check for level up
        if (playerXP >= xpToNextLevel)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        currentLevel++;
        playerXP -= xpToNextLevel;
        xpToNextLevel += xpIncreasePerLevel;

        Debug.Log($"Level Up! Current Level: {currentLevel}");

        PauseGame();
        ShowUpgradeOptions();
    }

    private void ShowUpgradeOptions()
    {
        // Pick 2 or 3 random upgrades from all available upgrades
        List<UpgradeData> upgradesToShow = new List<UpgradeData>();
        while (upgradesToShow.Count < 3 && upgradesToShow.Count < allUpgrades.Count)
        {
            UpgradeData randomUpgrade = allUpgrades[Random.Range(0, allUpgrades.Count)];
            if (!upgradesToShow.Contains(randomUpgrade))
            {
                upgradesToShow.Add(randomUpgrade);
            }
        }

        // Show upgrade panel with the choices
        upgradePanelController.ShowUpgrades(upgradesToShow, OnUpgradeSelected);
    }

    private void OnUpgradeSelected(UpgradeData upgrade)
    {
        Debug.Log($"Upgrade Selected: {upgrade.upgradeName}");

        ApplyUpgradeEffect(upgrade);

        ResumeGame();
    }

    private void ApplyUpgradeEffect(UpgradeData upgrade)
    {
        switch (upgrade.upgradeType)
        {
            case UpgradeData.UpgradeType.MaceSpeedIncrease:
                FindObjectOfType<PlayerController>().IncreaseMaceSpeed(upgrade.value);
                break;
            case UpgradeData.UpgradeType.MaceProjectileAttack:
                FindObjectOfType<PlayerController>().EnableMaceProjectileAttack();
                break;
            case UpgradeData.UpgradeType.HealthIncrease:
                FindObjectOfType<PlayerController>().IncreaseHealth(upgrade.value);
                break;
            // Add more cases as needed
        }
    }

    private void PauseGame()
    {
        isGamePaused = true;
        Time.timeScale = 0f;
    }

    private void ResumeGame()
    {
        isGamePaused = false;
        Time.timeScale = 1f;
    }
}
