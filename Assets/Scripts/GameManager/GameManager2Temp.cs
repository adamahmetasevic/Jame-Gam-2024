using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [Header("Enemy Spawning")]
    public GameObject[] enemyPrefabs;        // Array of different enemy types
    public GameObject bossPrefab;            // The final boss prefab
    public float gameLength = 900f;          // 15 minutes in seconds
    public float minSpawnTime = 2f;          // Minimum time between spawns
    public float maxSpawnTime = 5f;          // Maximum time between spawns
    
    [Header("Spawn Area")]
    public float spawnRadius = 15f;          // How far from player enemies can spawn
    public float minSpawnDistance = 10f;     // Minimum distance from player

    [Header("Difficulty Scaling")]
    public float spawnRateIncrease = 0.1f;   // How much spawn rate increases over time
    public int maxEnemiesAlive = 10;         // Maximum enemies at once
    
    [Header("Events")]
    public UnityEvent onGameStart;
    public UnityEvent onBossSpawn;
    public UnityEvent onGameEnd;

    private Transform player;
    private float gameTimer = 0f;
    private bool isBossSpawned = false;
    private int currentEnemies = 0;
    private bool isGameActive = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogError("Player not found! Make sure player has 'Player' tag.");
            return;
        }

        StartGame();
    }

    void StartGame()
    {
        isGameActive = true;
        gameTimer = 0f;
        onGameStart?.Invoke();
        GameTimer.Instance?.StartTimer(); // Start the timer
        StartCoroutine(SpawnEnemyRoutine());
    }

    void Update()
    {
        if (!isGameActive) return;

        gameTimer += Time.deltaTime;

        // Check if it's time to spawn the boss
        if (gameTimer >= gameLength && !isBossSpawned)
        {
            SpawnBoss();
        }
    }

    IEnumerator SpawnEnemyRoutine()
    {
        while (isGameActive && gameTimer < gameLength)
        {
            if (currentEnemies < maxEnemiesAlive)
            {
                SpawnEnemy();
            }

            // Calculate next spawn time (decreases as game progresses)
            float progressRatio = gameTimer / gameLength;
            float currentMinTime = Mathf.Max(minSpawnTime, maxSpawnTime - (maxSpawnTime * progressRatio * spawnRateIncrease));
            float spawnDelay = Random.Range(currentMinTime, maxSpawnTime);
            
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    void SpawnEnemy()
    {
        if (player == null || enemyPrefabs.Length == 0) return;

        // Get random spawn position
        Vector2 spawnPos = GetRandomSpawnPosition();

        // Select random enemy type
        GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

        // Spawn enemy
        GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        currentEnemies++;

        // Add listener for enemy destruction
        Enemy enemyComponent = enemy.GetComponent<Enemy>();
        if (enemyComponent != null)
        {
            StartCoroutine(WatchForEnemyDestruction(enemy));
        }
    }

Vector2 GetRandomSpawnPosition()
    {
        float angle = Random.Range(0f, 360f);
        float distance = Random.Range(minSpawnDistance, spawnRadius);
        
        Vector2 offset = new Vector2(
            Mathf.Cos(angle * Mathf.Deg2Rad) * distance,
            Mathf.Sin(angle * Mathf.Deg2Rad) * distance
        );

        return (Vector2)player.position + offset;
    }

    IEnumerator WatchForEnemyDestruction(GameObject enemy)
    {
        while (enemy != null)
        {
            yield return new WaitForSeconds(0.5f);
        }
        currentEnemies--;
    }

    void SpawnBoss()
    {
        if (bossPrefab == null)
        {
            Debug.LogError("Boss prefab not assigned!");
            return;
        }

        // Stop normal enemy spawning
        isBossSpawned = true;
        StopAllCoroutines();

        // Optional: Clear existing enemies
        ClearExistingEnemies();

        // Spawn boss at a predetermined position or using spawn position logic
        Vector2 bossSpawnPos = GetRandomSpawnPosition();
        GameObject boss = Instantiate(bossPrefab, bossSpawnPos, Quaternion.identity);

        onBossSpawn?.Invoke();

        // Optional: Watch for boss defeat
        StartCoroutine(WatchForBossDefeat(boss));
    }

    void ClearExistingEnemies()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            Destroy(enemy.gameObject);
        }
        currentEnemies = 0;
    }

    IEnumerator WatchForBossDefeat(GameObject boss)
    {
        while (boss != null)
        {
            yield return new WaitForSeconds(0.5f);
        }
        
        // Boss was defeated
        EndGame(true);
    }

    public void EndGame(bool victory)
    {
        isGameActive = false;
        onGameEnd?.Invoke();
        
        GameTimer.Instance?.StopTimer(); // Stop the timer

        // Log the total time
        float totalTime = GameTimer.Instance?.TotalTime ?? 0f;
        if (victory)
        {
            Debug.Log("Game Won!");
            Debug.Log($"Total Game Time: {totalTime:F2} seconds");
        }
        else
        {
            Debug.Log("Game Over!");
        }
    }


    // Helper method to get current game progress (0 to 1)
    public float GetGameProgress()
    {
        return Mathf.Clamp01(gameTimer / gameLength);
    }

    // Helper method to get remaining time in seconds
    public float GetRemainingTime()
    {
        return Mathf.Max(0, gameLength - gameTimer);
    }

    // Optional: Methods to pause/resume the game
    public void PauseGame()
    {
        isGameActive = false;
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        isGameActive = true;
        Time.timeScale = 1;
    }

    // Optional: Method to manually trigger boss spawn (for testing)
    public void DebugSpawnBoss()
    {
        gameTimer = gameLength;
        SpawnBoss();
    }

    void OnDrawGizmosSelected()
    {
        // Visualize spawn area in editor
        if (player != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(player.position, spawnRadius);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(player.position, minSpawnDistance);
        }
    }
}