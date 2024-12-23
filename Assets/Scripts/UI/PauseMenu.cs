using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI; // Reference to the pause menu panel
    private bool isPaused = false; // Track whether the game is paused
    private GameTimer gameTimer;   // Reference to the GameTimer instance

    void Start()
    {
        // Get the GameTimer instance
        gameTimer = GameTimer.Instance;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // Toggle pause on pressing Escape
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false); // Hide the pause menu
        Time.timeScale = 1f;          // Resume game time
        isPaused = false;

        // Resume the timer
        if (gameTimer != null)
        {
            gameTimer.StartTimer();
        }
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true); // Show the pause menu
        Time.timeScale = 0f;         // Freeze game time
        isPaused = true;

        // Stop the timer
        if (gameTimer != null)
        {
            gameTimer.StopTimer();
        }
    }

    public void Restart()
    {
        Time.timeScale = 1f; // Ensure time resumes
        if (gameTimer != null)
        {
            gameTimer.ResetTimer(); // Reset the timer
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload current scene
    }

    public void QuitGame()
    {
        Time.timeScale = 1f; // Ensure time resumes
        Debug.Log("Quitting Game...");
        Application.Quit();  // Quit the game (only works in a build)
    }


     public void ResumeGame()
    {
        pauseMenuUI.SetActive(false); // Hide the pause menu
        Time.timeScale = 1f;          // Resume game time
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f; // Ensure time resumes
        SceneManager.LoadScene("MainMenu"); // Replace "MainMenu" with the actual name of your main menu scene
    }
}
