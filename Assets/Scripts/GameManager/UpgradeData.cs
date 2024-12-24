using UnityEngine;

[CreateAssetMenu(fileName = "NewUpgrade", menuName = "Game/Upgrade")]
public class UpgradeData : ScriptableObject
{
    public string upgradeName;          // Name of the upgrade
    public string description;          // Description of what the upgrade does
    public Sprite icon;                 // Optional: Icon for the UI

    public UpgradeType upgradeType;     // Enum to classify upgrades
    public float value;                 // Effect of the upgrade

    public int currentApplications = 0; // Track how many times this upgrade was applied
    public int maxApplications = 1;     // Maximum number of times this upgrade can be applied

    public enum UpgradeType
    {
        MaceSpeedIncrease,
        ProjectileCountIncrease,
        ProjectileSpeedIncrease,
        ProjectileSizeIncrease,
        ProjectileDamageIncrease,
        HealthIncrease,
        MovementSpeedBoost,
        MaceSizeIncrease,
        MaceDamageMultiplierIncrease
    }
}

