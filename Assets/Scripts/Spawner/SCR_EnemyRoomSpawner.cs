using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class SCR_EnemyRoomSpawner : MonoBehaviour
{
    public List<GameObject> meleeEnemyPrefabs, lockedDoorsList; 
    public List<GameObject> rangeEnemyPrefabs;
    public int meleeEnemyCount = 3;
    public int rangeEnemyCount = 2;
    public List<Transform> meleeSpawnPoints;
    public List<Transform> rangeSpawnPoints;
    public float spawnPointRange;
    public float maxNavMeshCheckDistance = 5f;
    public List<SCR_SpawnTrigger> triggerList;
    [Header("Wave Settings")] public int totalWaves = 3;
    public float timeBetweenWaves = 5f;
    private int currentWave = 0;
    private bool isWaveInProgress = false;
    private bool hasSpawned = false;

    public enum RoomState
    {
        InActive,
        Active,
        Clear
    }

    public RoomState state;

    public void Start()
    {
        foreach (var trigger in triggerList)
        {
            trigger.onPlayerTouched += StartWaveSpawning;
            trigger.onPlayerTouched += UnSubFromSpawn;
        }

        state = RoomState.InActive;

        if (GetComponentInParent<SCR_RoomInfo>().IsSpawnRoom)
        {
            EventManager.Instance.OnRoomCompleted += HandleRoomCompleted;
            HandleRoomCompleted(gameObject);
        }
    }

    public void UnSubFromSpawn()
    {
        foreach (var trigger in triggerList)
        {
            trigger.onPlayerTouched -= StartWaveSpawning;
        }
    }

    IEnumerator CheckRoomClear()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);

            if (state == RoomState.Active && !SCR_EnemyController.Instance.activeEnemies.Any())
            {
                HandleRoomCompleted(gameObject);
                
            }
        }
    }

    private void OnDisable()
    {
        UnSubFromSpawn();
    }

    public void StartWaveSpawning()
    {
        if (hasSpawned) return;
        hasSpawned = true;
        StartCoroutine(SpawnWaves());
    }

    private IEnumerator SpawnWaves()
    {
        state = RoomState.Active;

        StartCoroutine(CheckRoomClear());

        EventManager.Instance.OnRoomCompleted += HandleRoomCompleted;

        while (currentWave < totalWaves)
        {
            isWaveInProgress = true;
            Debug.Log($"Spawning wave {currentWave + 1}/{totalWaves}");

            SpawnEnemyGroup(meleeEnemyPrefabs, meleeSpawnPoints, meleeEnemyCount);
            SpawnEnemyGroup(rangeEnemyPrefabs, rangeSpawnPoints, rangeEnemyCount);

            yield return new WaitForSeconds(timeBetweenWaves);

            // Wait until all enemies are defeated before spawning the next wave
            while (SCR_EnemyController.Instance != null && SCR_EnemyController.Instance.activeEnemies.Count > 2)
            {
                yield return null;
            }

            currentWave++;
        }

        
        
        //state = RoomState.Clear;
        Debug.Log("All waves cleared!");
        //HandleRoomCompleted();

    }

    private void HandleRoomCompleted(GameObject lastEnemy)
    {
        state = RoomState.Clear;
        Debug.Log("Room cleared: " + gameObject.name);
        EventManager.Instance.OnRoomCompleted -= HandleRoomCompleted;

        foreach (var door in lockedDoorsList)
        {
            if (!door) continue;
            door.SetActive(false);
        }

        StopAllCoroutines();

        SCR_HeadsUpDisplay.Instance.radarMap.SetActive(false);

        SCR_GameController.Instance.CheckIfFloorClear();
    }

    public void SpawnEnemies()
    {
        if (hasSpawned) return;
        hasSpawned = true;

        SpawnEnemyGroup(meleeEnemyPrefabs, meleeSpawnPoints, meleeEnemyCount);
        SpawnEnemyGroup(rangeEnemyPrefabs, rangeSpawnPoints, rangeEnemyCount);

        state = RoomState.Active;
        EventManager.Instance.OnRoomCompleted += HandleRoomCompleted;
    }

    private void SpawnEnemyGroup(List<GameObject> enemyPrefabs, List<Transform> spawnPoints, int count)
    {
        if (enemyPrefabs.Count == 0 || spawnPoints.Count == 0) return;

        for (int i = 0; i < count; i++)
        {
            Vector3 spawnPos = GetRandomNavMeshPosition(spawnPoints);
            if (spawnPos != Vector3.zero)
            {
                GameObject randomEnemy = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
                GameObject enemy = Instantiate(randomEnemy, spawnPos, Quaternion.identity);

                if (SCR_EnemyController.Instance != null)
                {
                    SCR_EnemyController.Instance.activeEnemies.Add(enemy);
                }
                else
                {
                    Debug.LogWarning("SCR_EnemyController instance not found!");
                }
            }
            else
            {
                Debug.LogWarning("Failed to find a valid NavMesh position for enemy spawn.");
            }
        }

        foreach (var door in lockedDoorsList)
        {
            if (!door) continue;
            door.SetActive(true);
        }
    }

    private Vector3 GetRandomNavMeshPosition(List<Transform> spawnPoints)
    {
        Vector3 spawnCenter = spawnPoints.Any()
            ? spawnPoints[Random.Range(0, spawnPoints.Count)].position
            : transform.position;

        for (int attempt = 0; attempt < 5; attempt++)
        {
            // Calculate a random position within a square centered around spawnCenter
            Vector3 randomPosition = spawnCenter + new Vector3(
                Random.Range(-spawnPointRange, spawnPointRange),
                0f,
                Random.Range(-spawnPointRange, spawnPointRange)
            );

            // Check if the random position is valid on the NavMesh
            if (NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, maxNavMeshCheckDistance, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }

        return Vector3.zero; // Return zero vector if no valid position is found
    }

    private void OnDrawGizmos()
    {
        // Draw green squares for melee spawn points
        Gizmos.color = Color.green;
        foreach (var spawnPoint in meleeSpawnPoints)
        {
            if (spawnPoint == null) continue;
            Gizmos.DrawWireCube(spawnPoint.position, new Vector3(spawnPointRange * 2, 0.1f, spawnPointRange * 2));
        }

        // Draw red squares for range spawn points
        Gizmos.color = Color.red;
        foreach (var spawnPoint in rangeSpawnPoints)
        {
            if (spawnPoint == null) continue;
            Gizmos.DrawWireCube(spawnPoint.position, new Vector3(spawnPointRange * 2, 0.1f, spawnPointRange * 2));
        }
    }
}
