using UnityEngine;
using UnityEngine.SceneManagement;

public class DefeatManager : MonoBehaviour
{
    [Header("Defeat UI")]
    public GameObject defeatUIPanel; // Reference to the defeat UI panel
    public string mainMenuSceneName = "MainMenu"; // Name of the main menu scene

    [Header("Game State")]
    public bool isGamePaused = false;

    private void Start()
    {
        if (defeatUIPanel != null)
        {
            defeatUIPanel.SetActive(false); // Ensure the defeat UI is hidden at the start
        }
    }

    /// <summary>
    /// Triggers the defeat condition.
    /// </summary>
    public void TriggerDefeat()
    {
        Debug.Log("Defeat triggered!");
        PauseGame();
        ShowDefeatUI();
    }

    /// <summary>
    /// Pauses the game.
    /// </summary>
    private void PauseGame()
    {
        isGamePaused = true;
        Time.timeScale = 0f; // Freeze time
    }

    /// <summary>
    /// Resumes the game.
    /// </summary>
    private void ResumeGame()
    {
        isGamePaused = false;
        Time.timeScale = 1f; // Resume time
    }

    /// <summary>
    /// Displays the defeat UI.
    /// </summary>
    private void ShowDefeatUI()
    {
        if (defeatUIPanel != null)
        {
            defeatUIPanel.SetActive(true);
        }
        else
        {
            Debug.LogError("Defeat UI Panel is not assigned in the inspector!");
        }
    }

    /// <summary>
    /// Restarts the current scene.
    /// </summary>
    public void RestartScene()
    {
        ResumeGame(); // Resume time before reloading the scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Loads the main menu scene.
    /// </summary>
    public void LoadMainMenu()
    {
        ResumeGame(); // Resume time before loading the main menu
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
