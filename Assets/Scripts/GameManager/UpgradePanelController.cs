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

        int buttonCount = upgrades.Count;

        for (int i = 0; i < buttonCount; i++)
        {
            // Instantiate button prefab at the correct position from the buttonPositions list
            if (i < buttonPositions.Length)
            {
                // Instantiate the button at the corresponding button position from the list
                GameObject buttonObj = Instantiate(upgradeButtonPrefab, buttonPositions[i].position, Quaternion.identity, buttonPositions[i]);
                Button button = buttonObj.GetComponent<Button>();

                // Use TextMeshProUGUI instead of Text
                TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();

                if (buttonText != null)
                {
                    buttonText.text = $"{upgrades[i].upgradeName}\n{upgrades[i].description}";
                }
                else
                {
                    Debug.LogError("Button prefab is missing a TextMeshProUGUI component.");
                }

                // Capture the current upgrade instance to prevent closure issue in the lambda
                UpgradeData capturedUpgrade = upgrades[i];
                button.onClick.AddListener(() => OnUpgradeSelected(capturedUpgrade));

                // Track the instantiated button
                instantiatedButtons.Add(buttonObj);
            }
            else
            {
                // If there are more upgrades than positions, you can either handle overflow or just stack them.
                Debug.LogWarning("More upgrades than defined positions.");
                break;  // Exit the loop if there are not enough positions to place the buttons
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