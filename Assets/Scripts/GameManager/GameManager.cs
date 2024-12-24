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

    public static GameManager Instance { get; private set; } 
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject); 
        }
    }
    private void Start()
    {
        playerXP = 0;
        currentLevel = 1;
        Time.timeScale = 1f; 
        ResetUpgrades(); 
        // NAMU TIMER ADDITION
        if (gameTimer != null)
        {
            gameTimer.ResetTimer();
            gameTimer.StartTimer();
        }
        else
        {
            Debug.LogError("GameTimer reference is missing!");
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void AddXP(int xpAmount)
    {
        playerXP += xpAmount;

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
    List<UpgradeData> upgradesToShow = new List<UpgradeData>();
    
    while (upgradesToShow.Count < 3 && upgradesToShow.Count < allUpgrades.Count)
    {
        UpgradeData randomUpgrade = allUpgrades[Random.Range(0, allUpgrades.Count)];

        if (!HasReachedUpgradeLimit(randomUpgrade))
        {
            if (!upgradesToShow.Contains(randomUpgrade))
            {
                upgradesToShow.Add(randomUpgrade);
            }
        }
    }

    upgradePanelController.ShowUpgrades(upgradesToShow, OnUpgradeSelected);
}

public bool HasReachedUpgradeLimit(UpgradeData upgrade)
{
    if (upgrade.currentApplications >= upgrade.maxApplications)
    {
        return true;
    }
    return false;
}


private void ResetUpgrades()
{
    foreach (UpgradeData upgrade in allUpgrades)
    {
 
            upgrade.currentApplications = 0; 
        
    }
    Debug.Log("All upgrades have been reset.");
}



    private void OnUpgradeSelected(UpgradeData upgrade)
    {
        Debug.Log($"Upgrade Selected: {upgrade.upgradeName}");

        ApplyUpgradeEffect(upgrade);

        ResumeGame();
    }

    private void ApplyUpgradeEffect(UpgradeData upgrade)
{
    upgrade.currentApplications++;

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
    if (scene.name == "MainScene")
    {
        Debug.Log("MainScene loaded: Resetting XP and level progression.");
        ResetGameState(); 
    }
}

private void ResetGameState()
{
    playerXP = 0;
    currentLevel = 1;
    xpToNextLevel = 100;
    ResetUpgrades(); 
}

private void OnDestroy()
{
    SceneManager.sceneLoaded -= OnSceneLoaded;

}
}
