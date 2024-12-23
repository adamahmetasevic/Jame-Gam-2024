using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryUIManager : MonoBehaviour
{
    public GameObject victoryPanel; // Assign the victory panel in the Inspector

    public void ShowVictoryUI()
    {
        victoryPanel.SetActive(true); // Show the victory panel
        Time.timeScale = 0f; // Pause the game
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Resume the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload current scene
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f; // Resume the game
        SceneManager.LoadScene("MainMenu"); // Load the Main Menu scene
    }
}
