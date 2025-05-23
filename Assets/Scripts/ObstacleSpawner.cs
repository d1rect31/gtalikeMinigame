using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public GameObject coinPrefab;
    public Transform player;
    public float spawnInterval = 5f;
    public float spawnIntervalCoin = 15f;
    public float[] lanePositions = { -3f, 0f, 3f };
    public float spawnDistanceAhead = 20f;

    private int lastObstacleLane = -1;

    void Start()
    {
        InvokeRepeating(nameof(SpawnObstacle), 1f, spawnInterval);
        InvokeRepeating(nameof(SpawnCoin), 1f, spawnIntervalCoin);
    }

    void SpawnObstacle()
    {
        if (player == null) return;
        int lane = Random.Range(0, lanePositions.Length);
        lastObstacleLane = lane;
        Vector3 spawnPos = new Vector3(lanePositions[lane], player.position.y + spawnDistanceAhead, 0);
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
