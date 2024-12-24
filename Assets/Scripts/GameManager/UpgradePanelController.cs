using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.Collections;

public class UpgradePanelController : MonoBehaviour
{
    [Header("Upgrade UI Elements")]
    public GameObject upgradePanel;        // The entire panel
    public GameObject upgradeButtonPrefab; // Prefab for buttons
    public GameManager gameManager; // Reference to GameManager

    private List<UpgradeData> availableUpgrades; // Upgrades to pick from this level-up
    private System.Action<UpgradeData> onUpgradeSelected; // Callback when an upgrade is chosen

    // for placing the buttons
    public Transform[] buttonPositions; // holds positions
    private List<GameObject> instantiatedButtons = new List<GameObject>(); // holds created buttons for clearing later

 public void ShowUpgrades(List<UpgradeData> upgrades, System.Action<UpgradeData> callback)
{
    upgradePanel.SetActive(true);
    ClearExistingButtons();

    availableUpgrades = upgrades;
    onUpgradeSelected = callback;

    // Filter upgrades to remove those that have reached their max value
    List<UpgradeData> availableUpgradesToShow = new List<UpgradeData>();

    foreach (var upgrade in upgrades)
    {
        if (!gameManager.HasReachedUpgradeLimit(upgrade))
        {
            availableUpgradesToShow.Add(upgrade);
        }
    }

    // If no upgrades are available (all are maxed out), you can show a fallback message or hide the panel
    if (availableUpgradesToShow.Count == 0)
    {
        Debug.Log("All upgrades are maxed out!");
        // Optionally, display a message to the player or just hide the upgrade panel
        // e.g., Show a message like "No more upgrades available"
        return;
    }

    // If there are available upgrades, show them
    int buttonCount = availableUpgradesToShow.Count;
    
    for (int i = 0; i < buttonCount; i++)
    {
        // Instantiate the button at the correct position from the buttonPositions list
        if (i < buttonPositions.Length)
        {
            // Instantiate the button prefab
            GameObject buttonObj = Instantiate(upgradeButtonPrefab, buttonPositions[i].position, Quaternion.identity, buttonPositions[i]);
            Button button = buttonObj.GetComponent<Button>();

            TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = $"{availableUpgradesToShow[i].upgradeName}\n{availableUpgradesToShow[i].description}";

            // Capture the current upgrade instance to prevent closure issue in the lambda
            UpgradeData capturedUpgrade = availableUpgradesToShow[i];
            button.onClick.AddListener(() => OnUpgradeSelected(capturedUpgrade));

            instantiatedButtons.Add(buttonObj);
        }
    }
}




    private void OnUpgradeSelected(UpgradeData selectedUpgrade)
    {
        upgradePanel.SetActive(false);
        onUpgradeSelected?.Invoke(selectedUpgrade);
    }

    private void ClearExistingButtons()
    {
        // Destroy each button stored in the instantiatedButtons list
        foreach (GameObject button in instantiatedButtons)
        {
            Destroy(button);
        }

        // Clear the list after destroying the buttons
        instantiatedButtons.Clear();
    }


}