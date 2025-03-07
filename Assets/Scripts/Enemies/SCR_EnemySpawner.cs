using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    public GameObject enemyPrefab; 
    public Transform spawnPoint; 
    public float spawnRadius = 1f; 
    [Range(0, 360)] public float spawnAngle = 180f; 
    public float spawnRate = 1f; 
    public float detectionRadius = 2f; 
    public string playerTag = "Player"; 

    private float spawnTimer;
    private bool playerDetected = false;

    private void Update() {
        playerDetected = DetectPlayer();

        if (playerDetected)
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= spawnRate)
            {
                SpawnEnemy();
                spawnTimer = 0f; 
            }
        }
    }

    bool DetectPlayer() {
        Collider[] playerInRange = Physics.OverlapSphere(transform.position, detectionRadius);
        foreach (Collider col in playerInRange)
        {
            if (col.CompareTag(playerTag))
            {
                return true; 
            }
        }
        return false; 
    }

    void SpawnEnemy() {
        if (enemyPrefab == null || spawnPoint == null) return;

        Vector3 spawnPosition = GetRandomSpawnPosition();
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        Debug.Log($"Enemy Spawned at {spawnPosition}");
    }

    Vector3 GetRandomSpawnPosition() {
        float halfAngle = spawnAngle / 2f;
        float angle = Random.Range(-halfAngle, halfAngle);

        Vector3 direction = Quaternion.Euler(0, angle, 0) * spawnPoint.forward;
        return spawnPoint.position + direction * Random.Range(0, spawnRadius);
    }

    private void OnDrawGizmos() {
        if (spawnPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(spawnPoint.position, 0.2f);

        Vector3 startDir = Quaternion.Euler(0, -spawnAngle / 2, 0) * spawnPoint.forward;
        Vector3 endDir = Quaternion.Euler(0, spawnAngle / 2, 0) * spawnPoint.forward;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(spawnPoint.position, spawnPoint.position + startDir * spawnRadius);
        Gizmos.DrawLine(spawnPoint.position, spawnPoint.position + endDir * spawnRadius);

        Gizmos.color = new Color(1f, 1f, 0f, 0.5f);
        for (float angle = -spawnAngle / 2; angle <= spawnAngle / 2; angle += 5f)
        {
            Vector3 direction = Quaternion.Euler(0, angle, 0) * spawnPoint.forward;
            Gizmos.DrawLine(spawnPoint.position, spawnPoint.position + direction * spawnRadius);
        }
    }
}
