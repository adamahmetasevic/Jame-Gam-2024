using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

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
            Text buttonText = buttonObj.GetComponentInChildren<Text>();

            buttonText.text = upgrade.upgradeName + "\n" + upgrade.description;
            button.onClick.AddListener(() => OnUpgradeSelected(upgrade));
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
