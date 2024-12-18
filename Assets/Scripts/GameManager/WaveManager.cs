using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    [Header("Wave Settings")]
    public int currentWave = 1;           // Current wave
    public int totalWaves = 15;            // Total number of waves
    public float waveInterval = 60f;       // Time interval between waves in seconds
    public float spawnRadius = 10f;        // Radius around the spawner for enemy spawn
    private float currentSpawnRate = 0.5f; // Adjusted spawn rate (less enemies per second)

    [Header("Enemy Prefabs")]
    public GameObject elvesPrefab;
    public GameObject snowmenPrefab;
    public GameObject reindeerPrefab;
    public GameObject bossPrefab;  // Santa Boss prefab

    private bool isSpawning = false;

    void Start()
    {
        StartCoroutine(ManageWaves());
    }

    private IEnumerator ManageWaves()
    {
        while (currentWave <= totalWaves)
        {
            Debug.Log($"Wave {currentWave} Starting!");

            // Start spawning enemies for the current wave
            StartCoroutine(SpawnWave(currentWave));

            // Wait for the next wave
            yield return new WaitForSeconds(waveInterval);

            // Increase spawn rate for waves 7-14
            if (currentWave >= 7 && currentWave <= 14)
            {
                currentSpawnRate = Mathf.Max(0.5f, currentSpawnRate * 1.5f); // Slightly increase spawn rate for waves 7-14
                Debug.Log($"Spawn rate increased to: {currentSpawnRate}");
            }

            currentWave++;
        }
    }

    private IEnumerator SpawnWave(int waveNumber)
    {
        float spawnRate = currentSpawnRate;

        // Adjust spawn rate for waves 7-14
        if (waveNumber >= 7 && waveNumber <= 14)
        {
            spawnRate *= 1.5f;
        }

        // Spawn enemies based on the wave number
        switch (waveNumber)
        {
            case 1:
                yield return StartCoroutine(SpawnEnemies(elvesPrefab, spawnRate));
                break;
            case 2:
                yield return StartCoroutine(SpawnEnemies(snowmenPrefab, spawnRate));
                break;
            case 3:
                yield return StartCoroutine(SpawnEnemies(reindeerPrefab, spawnRate));
                break;
            case 4:
                yield return StartCoroutine(SpawnEnemies(elvesPrefab, snowmenPrefab, spawnRate));
                break;
            case 5:
                yield return StartCoroutine(SpawnEnemies(elvesPrefab, reindeerPrefab, spawnRate));
                break;
            case 6:
                yield return StartCoroutine(SpawnEnemies(elvesPrefab, snowmenPrefab, reindeerPrefab, spawnRate));
                break;
            case 7:
            case 8:
            case 9:
            case 10:
            case 11:
            case 12:
            case 13:
            case 14:
                yield return StartCoroutine(SpawnEnemies(elvesPrefab, snowmenPrefab, reindeerPrefab, spawnRate));
                break;
            case 15:
                SpawnSantaBoss(); // Special case for the Santa Boss
                break;
            default:
                break;
        }
    }

    private IEnumerator SpawnEnemies(GameObject enemyType, float spawnRate = 0.5f)
    {
        while (true)
        {
            yield return new WaitForSeconds(1f / spawnRate); // Adjust spawn interval based on rate

            Vector2 randomSpawnOffset = Random.insideUnitCircle * spawnRadius;
            Vector3 spawnPosition = transform.position + new Vector3(randomSpawnOffset.x, 0, randomSpawnOffset.y);

            if (IsValidSpawnPosition(spawnPosition))
            {
                Instantiate(enemyType, spawnPosition, Quaternion.identity);
            }
            else
            {
                Debug.LogWarning("Invalid spawn position. Retrying next interval.");
            }
        }
    }

    private IEnumerator SpawnEnemies(GameObject enemyType1, GameObject enemyType2, float spawnRate = 0.5f)
    {
        while (true)
        {
            yield return new WaitForSeconds(1f / spawnRate);

            Vector2 randomSpawnOffset = Random.insideUnitCircle * spawnRadius;
            Vector3 spawnPosition = transform.position + new Vector3(randomSpawnOffset.x, 0, randomSpawnOffset.y);

            if (IsValidSpawnPosition(spawnPosition))
            {
                Instantiate(Random.Range(0f, 1f) > 0.5f ? enemyType1 : enemyType2, spawnPosition, Quaternion.identity);
            }
            else
            {
                Debug.LogWarning("Invalid spawn position. Retrying next interval.");
            }
        }
    }

    private IEnumerator SpawnEnemies(GameObject enemyType1, GameObject enemyType2, GameObject enemyType3, float spawnRate = 0.5f)
    {
        while (true)
        {
            yield return new WaitForSeconds(1f / spawnRate);

            Vector2 randomSpawnOffset = Random.insideUnitCircle * spawnRadius;
            Vector3 spawnPosition = transform.position + new Vector3(randomSpawnOffset.x, 0, randomSpawnOffset.y);

            if (IsValidSpawnPosition(spawnPosition))
            {
                GameObject enemyToSpawn = Random.Range(0f, 1f) > 0.66f
                    ? enemyType1
                    : (Random.Range(0f, 1f) > 0.5f ? enemyType2 : enemyType3);
                Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);
            }
            else
            {
                Debug.LogWarning("Invalid spawn position. Retrying next interval.");
            }
        }
    }

    private IEnumerator SpawnSantaBoss()
    {
        Debug.LogWarning("Invalid boss spawn position. Retrying in 5 seconds."); // Log a message about retrying
        yield return new WaitForSeconds(5f); // Wait before retrying

        // Now retry the spawning
        Vector2 randomSpawnOffset = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPosition = transform.position + new Vector3(randomSpawnOffset.x, 0, randomSpawnOffset.y);

        if (IsValidSpawnPosition(spawnPosition))
        {
            Instantiate(bossPrefab, spawnPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Invalid boss spawn position again. Retrying...");
            StartCoroutine(SpawnSantaBoss()); // Retry spawning
        }
    }

    private bool IsValidSpawnPosition(Vector3 position)
    {
        // Check if the spawn position is free from obstacles or other enemies
        Collider[] colliders = Physics.OverlapSphere(position, 1f); // Check within a 1-unit radius to avoid overlap
        return colliders.Length == 0; // If no colliders, it's valid
    }

    public void StopSpawning()
    {
        StopAllCoroutines(); // Stops all enemy spawning coroutines when needed (like on game pause)
    }
}
