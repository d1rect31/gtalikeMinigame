using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public GameObject coinPrefab;
    public GameObject enemyCarPrefab;
    public Transform player;
    public float spawnInterval = 5f;
    public float spawnIntervalCoin = 15f;
    public float spawnIntervalEnemyCar = 10f;
    public float[] lanePositions = { -2f, 0f, 2f };
    public float spawnDistanceAhead = 20f;
    public float spawnDistanceBehind = 10f;
    private int lastObstacleLane = -1;




    void Start()
    {
        InvokeRepeating(nameof(SpawnObstacle), 1f, spawnInterval);
        InvokeRepeating(nameof(SpawnCoin), 1f, spawnIntervalCoin);
        InvokeRepeating(nameof(SpawnEnemyCar), 2f, spawnIntervalEnemyCar);
    }

    void SpawnEnemyCar()
    {
        if (FindObjectsByType<EnemyCar>(FindObjectsSortMode.None).Length >= 7)
            return; // Ограничение на количество машин
        if (player == null || enemyCarPrefab == null) return;
        // Случайная полоса
        int lane = Random.Range(0, lanePositions.Length);
        Vector3 spawnPos = new(
            lanePositions[lane],
            player.position.y - spawnDistanceBehind,
            0
        );
        Instantiate(enemyCarPrefab, spawnPos, Quaternion.identity);
    }

    void SpawnObstacle()
    {
        if (player == null) return;
        int lane = Random.Range(0, lanePositions.Length);
        lastObstacleLane = lane;
        Vector3 spawnPos = new(lanePositions[lane], player.position.y + spawnDistanceAhead, 0);
        Instantiate(obstaclePrefab, spawnPos, Quaternion.identity);
    }

    void SpawnCoin()
    {
        if (player == null) return;
        System.Collections.Generic.List<int> possibleLanes = new System.Collections.Generic.List<int>();
        for (int i = 0; i < lanePositions.Length; i++)
        {
            if (i != lastObstacleLane)
                possibleLanes.Add(i);
        }

        if (possibleLanes.Count == 0) return;

        int lane = possibleLanes[Random.Range(0, possibleLanes.Count)];
        Vector3 spawnPos = new Vector3(lanePositions[lane], player.position.y + spawnDistanceAhead, 0);
        Instantiate(coinPrefab, spawnPos, Quaternion.identity);
    }
}
