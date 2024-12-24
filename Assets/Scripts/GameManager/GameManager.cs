using UnityEngine;
using UnityEngine.SceneManagement; // For scene management
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

    public GameTimer gameTimer;

    private void Start()
    {
        playerXP = 0;
        currentLevel = 1;
        Time.timeScale = 1f; // Ensure game starts unpaused

        // NAMU TIMER ADDITION
        if (gameTimer != null)
        {
            gameTimer.ResetTimer(); // Reset the timer
            gameTimer.StartTimer(); // Start the timer
        }
        else
        {
            Debug.LogError("GameTimer reference is missing!");
        }

        // Subscribe to the sceneLoaded event to handle XP reset when loading the MainScene
        SceneManager.sceneLoaded += OnSceneLoaded;
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

        // Check if the upgrade has already reached its limit before adding it
        if (!HasReachedUpgradeLimit(randomUpgrade))
        {
            if (!upgradesToShow.Contains(randomUpgrade))
            {
                upgradesToShow.Add(randomUpgrade);
            }
        }
    }

    // Show upgrade panel with the choices
    upgradePanelController.ShowUpgrades(upgradesToShow, OnUpgradeSelected);
}

public bool HasReachedUpgradeLimit(UpgradeData upgrade)
{
    try
    {
        CandyCaneMace candyCaneMace = FindObjectOfType<CandyCaneMace>();
        PlayerController playerController = FindObjectOfType<PlayerController>();
        MaceProjectile maceProjectile = FindObjectOfType<MaceProjectile>();

        // Check if required components exist before checking limits
        if (candyCaneMace == null || playerController == null || maceProjectile == null)
        {
            Debug.LogWarning("One or more required components not found: " +
                           $"CandyCaneMace: {candyCaneMace != null}, " +
                           $"PlayerController: {playerController != null}, " +
                           $"MaceProjectile: {maceProjectile != null}");
            return false; // Return false to allow upgrade if components are missing
        }

        switch (upgrade.upgradeType)
        {
            case UpgradeData.UpgradeType.MaceSpeedIncrease:
                return candyCaneMace.GetMaceSpeed() >= upgrade.maxValue;
            case UpgradeData.UpgradeType.MovementSpeedBoost:
                return playerController.GetMovementSpeed() >= upgrade.maxValue;
            case UpgradeData.UpgradeType.MaceSizeIncrease:
                return candyCaneMace.GetMaceSize() >= upgrade.maxValue;
            case UpgradeData.UpgradeType.ProjectileCountIncrease:
                return candyCaneMace.GetProjectileCount() >= upgrade.maxValue;
            case UpgradeData.UpgradeType.ProjectileSpeedIncrease:
                return candyCaneMace.GetProjectileSpeed() >= upgrade.maxValue;
            case UpgradeData.UpgradeType.ProjectileSizeIncrease:
                return candyCaneMace.GetProjectileSize() >= upgrade.maxValue;
            case UpgradeData.UpgradeType.ProjectileDamageIncrease:
                return candyCaneMace.GetProjectileDamage() >= upgrade.maxValue;
            case UpgradeData.UpgradeType.MaceDamageMultiplierIncrease:
                return candyCaneMace.GetMaceDamageMultiplier() >= upgrade.maxValue;
            default:
                Debug.LogWarning($"Unknown upgrade type: {upgrade.upgradeType}");
                return false;
        }
    }
    catch (System.Exception e)
    {
        Debug.LogError($"Error in HasReachedUpgradeLimit: {e.Message}");
        return false; // Return false to allow upgrade if there's an error
    }
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
            FindObjectOfType<CandyCaneMace>().IncreaseMaceSpeed(upgrade.value);
            break;
        case UpgradeData.UpgradeType.HealthIncrease:
            FindObjectOfType<PlayerController>().IncreaseHealth((int)upgrade.value);
            break;
        case UpgradeData.UpgradeType.MovementSpeedBoost:
            FindObjectOfType<PlayerController>().IncreaseSpeed((int)upgrade.value);
            break;
        case UpgradeData.UpgradeType.ProjectileCountIncrease:
            FindObjectOfType<CandyCaneMace>().UpgradeProjectileCount((int)upgrade.value);
            break;
        case UpgradeData.UpgradeType.ProjectileSpeedIncrease:
            FindObjectOfType<CandyCaneMace>().UpgradeProjectileSpeed(upgrade.value);
            break;
        case UpgradeData.UpgradeType.ProjectileSizeIncrease:
            FindObjectOfType<CandyCaneMace>().UpgradeProjectileSize(upgrade.value);
            break;
        case UpgradeData.UpgradeType.MaceSizeIncrease:
            FindObjectOfType<CandyCaneMace>().UpgradeMaceSize(upgrade.value); 
            break;
        case UpgradeData.UpgradeType.ProjectileDamageIncrease:
            FindObjectOfType<CandyCaneMace>().UpgradeProjectileDamage(upgrade.value);
            break;
        case UpgradeData.UpgradeType.MaceDamageMultiplierIncrease:
            FindObjectOfType<CandyCaneMace>().UpgradeMaceDamageMultiplier(upgrade.value); 
            break;

    }
}


    private void PauseGame()
    {
        isGamePaused = true;
        Time.timeScale = 0f;

        // NAMU TIMER ADDITION
        if (gameTimer != null)
        {
            gameTimer.StopTimer();
        }
        else
        {
            Debug.LogError("GameTimer is null while trying to pause the game.");
        }
    }

    private void ResumeGame()
    {
        isGamePaused = false;
        Time.timeScale = 1f;

        // NAMU TIMER ADDITION
        if (gameTimer != null)
        {
            gameTimer.StartTimer();
        }
        else
        {
            Debug.LogError("GameTimer is null while trying to resume the game.");
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reset XP and level progression if MainScene is loaded
        if (scene.name == "MainScene")
        {
            Debug.Log("MainScene loaded: Resetting XP and level progression.");
            playerXP = 0;
            currentLevel = 1;
            xpToNextLevel = 100; // Reset to initial value
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from the sceneLoaded event to prevent memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
