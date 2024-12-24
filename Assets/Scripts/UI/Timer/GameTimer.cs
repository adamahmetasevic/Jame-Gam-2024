using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public static GameTimer Instance { get; private set; } 

    public GameObject player;

    private float totalTime = 0f; 
    private bool isTimerRunning = false;

    [Header("UI Components")]
    public TextMeshProUGUI timerText; 

    public float TotalTime => totalTime; 

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "MainScene")
        {
            ResetTimer();
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
{
    if (scene.name == "MainScene")
    {
        player = GameObject.FindWithTag("Player"); 

        ResetTimer(); 
        StartTimer();
    }
}


    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    void Update()
    {
        if (isTimerRunning)
        {
            if (player == null)
            {
                StopTimer();
            }
            totalTime += Time.deltaTime;
            UpdateTimerUI(); 
        }
    }

    public void StartTimer()
    {
        isTimerRunning = true;
    }

    public void StopTimer()
    {
        isTimerRunning = false;
    }

    public void ResetTimer()
    {
        totalTime = 0f;
        UpdateTimerUI(); 
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(totalTime / 60);
            int seconds = Mathf.FloorToInt(totalTime % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        else
        {
            Debug.LogWarning("TimerText reference is not assigned in the Inspector.");
        }
    }
}
