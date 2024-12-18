using UnityEngine;

[CreateAssetMenu(fileName = "NewUpgrade", menuName = "Game/Upgrade")]
public class UpgradeData : ScriptableObject
{
    public string upgradeName;          // Name of the upgrade
    public string description;          // Description of what the upgrade does
    public Sprite icon;                 // Optional: Icon for the UI

    public UpgradeType upgradeType;     // Enum to classify upgrades
    public int value;                 // Generic value (e.g., damage boost, cooldown reduction)

public enum UpgradeType
{
    MaceSpeedIncrease,
    ProjectileCountIncrease,
    ProjectileSpeedIncrease,
    ProjectileSizeIncrease,
    HealthIncrease,
    MovementSpeedBoost,
}

}
