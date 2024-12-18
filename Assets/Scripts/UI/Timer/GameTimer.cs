using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public static GameTimer Instance { get; private set; } // Singleton instance

    public GameObject player;

    private float totalTime = 0f;  // Tracks total elapsed time
    private bool isTimerRunning = false;

    [Header("UI Components")]
    public TextMeshProUGUI timerText; // Reference to TextMeshPro UI element

    public float TotalTime => totalTime; // Public property to get total time

    void Awake()
    {
        // Singleton pattern to ensure only one GameTimer exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keeps timer persistent (optional)
        }
        else
        {
            Destroy(gameObject);
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
            totalTime += Time.deltaTime; // Increment total time
            UpdateTimerUI(); // Update the timer text on screen
        }
    }

    // Start the timer
    public void StartTimer()
    {
        isTimerRunning = true;
    }

    // Stop the timer
    public void StopTimer()
    {
        isTimerRunning = false;
    }

    // Reset the timer (optional)
    public void ResetTimer()
    {
        totalTime = 0f;
        UpdateTimerUI(); // Reset the UI display
    }

    // Update the TextMeshPro UI with the formatted time
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
