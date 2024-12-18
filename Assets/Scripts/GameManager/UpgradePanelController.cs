using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;  

public class UpgradePanelController : MonoBehaviour
{
    [Header("Upgrade UI Elements")]
    public GameObject upgradePanel;        // The entire panel
    public Transform upgradeButtonParent;  // Parent for dynamically created buttons
    public GameObject upgradeButtonPrefab; // Prefab for buttons

    private List<UpgradeData> availableUpgrades; // Upgrades to pick from this level-up
    private System.Action<UpgradeData> onUpgradeSelected; // Callback when an upgrade is chosen

    // Show the panel with upgrade choices
    public void ShowUpgrades(List<UpgradeData> upgrades, System.Action<UpgradeData> callback)
{
    upgradePanel.SetActive(true);
    ClearExistingButtons();

    availableUpgrades = upgrades;
    onUpgradeSelected = callback;

    foreach (UpgradeData upgrade in upgrades)
    {
        GameObject buttonObj = Instantiate(upgradeButtonPrefab, upgradeButtonParent);
        Button button = buttonObj.GetComponent<Button>();
        
        // Use TextMeshProUGUI instead of Text
        TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();

        if (buttonText != null)
        {
            buttonText.text = $"{upgrade.upgradeName}\n{upgrade.description}";
        }
        else
        {
            Debug.LogError("Button prefab is missing a TextMeshProUGUI component.");
        }

        // Capture the current upgrade instance to prevent closure issue in the lambda
        UpgradeData capturedUpgrade = upgrade;
        button.onClick.AddListener(() => OnUpgradeSelected(capturedUpgrade));
    }
}

    private void OnUpgradeSelected(UpgradeData selectedUpgrade)
    {
        upgradePanel.SetActive(false);
        onUpgradeSelected?.Invoke(selectedUpgrade);
    }

    private void ClearExistingButtons()
    {
        foreach (Transform child in upgradeButtonParent)
        {
            Destroy(child.gameObject);
        }
    }
}
