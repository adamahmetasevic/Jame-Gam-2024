using UnityEngine;

[CreateAssetMenu(fileName = "NewUpgrade", menuName = "Game/Upgrade")]
public class UpgradeData : ScriptableObject
{
    public string upgradeName;          // Name of the upgrade
    public string description;          // Description of what the upgrade does
    public Sprite icon;                 // Optional: Icon for the UI

    public UpgradeType upgradeType;     // Enum to classify upgrades
    public float value;                 // Current value (e.g., damage boost, size increase, etc.)
    public float maxValue;              // Maximum value for this upgrade

    public enum UpgradeType
    {
        MaceSpeedIncrease,         // Increases the mace speed
        ProjectileCountIncrease,   // Increases number of projectiles
        ProjectileSpeedIncrease,   // Increases projectile speed
        ProjectileSizeIncrease,    // Increases projectile size
        ProjectileDamageIncrease,
        HealthIncrease,            // Increases player's health
        MovementSpeedBoost,        // Increases player movement speed
        MaceSizeIncrease,          // Increases mace size
        MaceDamageMultiplierIncrease // Increases mace damage multiplier
    }
}

