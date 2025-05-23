using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public Transform player;
    public float spawnInterval = 5f;
    public float[] lanePositions = { -3f, 0f, 3f };
    public float spawnDistanceAhead = 20f;

    void Start()
    {
        InvokeRepeating(nameof(SpawnObstacle), 1f, spawnInterval);
    }

    void SpawnObstacle()
    {
        if (player == null) return;
        int lane = Random.Range(0, lanePositions.Length);
        Vector3 spawnPos = new Vector3(lanePositions[lane], player.position.y + spawnDistanceAhead, 0);
        Instantiate(obstaclePrefab, spawnPos, Quaternion.identity);
    }
}
