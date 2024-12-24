using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour
{
    [Header("Wave Settings")]
    public int currentWave = 1;           // Current wave
    public int totalWaves = 15;           // Total number of waves
    public float waveInterval = 60f;      // Time interval between waves in seconds
    private float currentSpawnRate = 0.5f; // Adjusted spawn rate (less enemies per second)

    [Header("Enemy Prefabs")]
    public GameObject elvesPrefab;
    public GameObject snowmenPrefab;
    public GameObject reindeerPrefab;
    public GameObject bossPrefab;  // Santa Boss prefab

    [Header("Spawn Points")]
    public List<Transform> spawnPoints; // List of predefined spawn points

    private bool isSpawning = false;

    void Start()
    {
        if (spawnPoints == null || spawnPoints.Count == 0)
        {
            Debug.LogError("No spawn points assigned! Please add spawn points in the inspector.");
            return;
        }

        StartCoroutine(ManageWaves());
    }

    private IEnumerator ManageWaves()
    {
        while (currentWave <= totalWaves)
        {
            Debug.Log($"Wave {currentWave} Starting!");

            StartCoroutine(SpawnWave(currentWave));

            yield return new WaitForSeconds(waveInterval);

            currentWave++;
        }
    }

    private IEnumerator SpawnWave(int waveNumber)
    {
        float spawnRate = currentSpawnRate;

 

        // Spawn enemies based on the wave number
        switch (waveNumber)
        {
            case 1:
                yield return StartCoroutine(SpawnEnemies(snowmenPrefab, spawnRate));
                break;
            case 2:
                yield return StartCoroutine(SpawnEnemies(elvesPrefab, spawnRate));
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
                yield return StartCoroutine(SpawnEnemies(elvesPrefab, snowmenPrefab, reindeerPrefab, spawnRate));
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
            yield return new WaitForSeconds(1f / spawnRate);

            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
            Instantiate(enemyType, spawnPoint.position, Quaternion.identity);
        }
    }

    private IEnumerator SpawnEnemies(GameObject enemyType1, GameObject enemyType2, float spawnRate = 0.5f)
    {
        while (true)
        {
            yield return new WaitForSeconds(1f / spawnRate);

            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
            Instantiate(Random.Range(0f, 1f) > 0.5f ? enemyType1 : enemyType2, spawnPoint.position, Quaternion.identity);
        }
    }

    private IEnumerator SpawnEnemies(GameObject enemyType1, GameObject enemyType2, GameObject enemyType3, float spawnRate = 0.5f)
    {
        while (true)
        {
            yield return new WaitForSeconds(1f / spawnRate);

            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
            GameObject enemyToSpawn = Random.Range(0f, 1f) > 0.66f
                ? enemyType1
                : (Random.Range(0f, 1f) > 0.5f ? enemyType2 : enemyType3);
            Instantiate(enemyToSpawn, spawnPoint.position, Quaternion.identity);
        }
    }
    

    private IEnumerator SpawnSantaBoss()
    {
        Debug.LogWarning("Invalid boss spawn position. Retrying in 5 seconds.");
        yield return new WaitForSeconds(5f);

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
        Instantiate(bossPrefab, spawnPoint.position, Quaternion.identity);
    }

    public void StopSpawning()
    {
        StopAllCoroutines(); // Stops all enemy spawning coroutines when needed (like on game pause)
    }
}
