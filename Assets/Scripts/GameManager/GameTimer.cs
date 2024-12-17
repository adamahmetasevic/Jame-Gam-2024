using UnityEngine;
using TMPro; // For TextMeshPro support

public class GameTimer : MonoBehaviour
{
    public static GameTimer Instance { get; private set; } // Singleton instance

    [SerializeField] private TextMeshProUGUI timerText; // UI Text reference
    private float totalTime = 0f;  // Tracks total elapsed time
    private bool isTimerRunning = false;

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
            totalTime += Time.deltaTime; // Increment total time
            UpdateTimerUI();
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
        UpdateTimerUI();
    }

    // Update the UI text to display the timer
    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(totalTime / 60);
            int seconds = Mathf.FloorToInt(totalTime % 60);
            timerText.text = $"{minutes:00}:{seconds:00}";
        }
    }

    // Allow GameManager to set the UI Text reference (optional)
    public void SetTimerText(TextMeshProUGUI text)
    {
        timerText = text;
    }
}
