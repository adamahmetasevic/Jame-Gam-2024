using UnityEngine;
using UnityEngine.SceneManagement;

public class DefeatManager : MonoBehaviour
{
    [Header("Defeat UI")]
    public GameObject defeatUIPanel; 
    public string mainMenuSceneName = "MainMenu"; 

    [Header("Game State")]
    public bool isGamePaused = false;

    private void Start()
    {
        if (defeatUIPanel != null)
        {
            defeatUIPanel.SetActive(false); 
        }
    }


    public void TriggerDefeat()
    {
        Debug.Log("Defeat triggered!");
        PauseGame();
        ShowDefeatUI();
    }


    private void PauseGame()
    {
        isGamePaused = true;
        Time.timeScale = 0f; 
    }

    private void ResumeGame()
    {
        isGamePaused = false;
        Time.timeScale = 1f; 
    }


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


    public void RestartScene()
    {
        ResumeGame(); 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

 
    public void LoadMainMenu()
    {
        ResumeGame(); 
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
