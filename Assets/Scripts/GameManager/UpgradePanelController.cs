using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.Collections;

public class UpgradePanelController : MonoBehaviour
{
    [Header("Upgrade UI Elements")]
    public GameObject upgradePanel;        
    public GameObject upgradeButtonPrefab;
    public GameManager gameManager; 

    private List<UpgradeData> availableUpgrades; 
    private System.Action<UpgradeData> onUpgradeSelected; 

    public Transform[] buttonPositions; 
    private List<GameObject> instantiatedButtons = new List<GameObject>(); 

 public void ShowUpgrades(List<UpgradeData> upgrades, System.Action<UpgradeData> callback)
{
    upgradePanel.SetActive(true);
    ClearExistingButtons(); 
    availableUpgrades = upgrades;
    onUpgradeSelected = callback;

    List<UpgradeData> availableUpgradesToShow = new List<UpgradeData>();

    foreach (var upgrade in upgrades)
    {
        if (!gameManager.HasReachedUpgradeLimit(upgrade))
        {
            availableUpgradesToShow.Add(upgrade);
        }
    }

    if (availableUpgradesToShow.Count == 0)
    {
        Debug.Log("All upgrades are maxed out!");
        upgradePanel.SetActive(false);
        return;
    }

    for (int i = 0; i < availableUpgradesToShow.Count; i++)
    {
        if (i < buttonPositions.Length)
        {
            GameObject buttonObj = Instantiate(upgradeButtonPrefab, buttonPositions[i].position, Quaternion.identity, buttonPositions[i]);
            Button button = buttonObj.GetComponent<Button>();

            TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = $"{availableUpgradesToShow[i].upgradeName}\n{availableUpgradesToShow[i].description}";

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
        foreach (GameObject button in instantiatedButtons)
        {
            Destroy(button);
        }

        instantiatedButtons.Clear();
    }


}