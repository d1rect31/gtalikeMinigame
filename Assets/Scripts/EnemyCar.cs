using UnityEngine;

public class EnemyCar : MonoBehaviour
{
    public float maxSpeed = 15f;
    public float minSpeed = 5f;
    public float acceleration = 20f;
    public float stopDistance = 5f;
    public float catchUpDistance = 15f;
    public float[] lanePositions = { -2f, 0f, 2f };
    public float laneChangeSpeed = 10f;

    private float currentSpeed;
    private Transform player;

    void Start()
    {
        currentSpeed = minSpeed;
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    private int GetPlayerLane()
    {
        var playerScript = player.GetComponent<PlayerController>();
        if (playerScript != null)
            return playerScript.currentLane;
        return 1;
    }

    void Update()
    {
        if (player == null) return;

        float distance = player.position.y - transform.position.y;
        if (distance > catchUpDistance)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed, acceleration * Time.deltaTime);
        }
        else if (distance > stopDistance)
        {
            float t = Mathf.InverseLerp(stopDistance, catchUpDistance, distance);
            float targetSpeed = Mathf.Lerp(minSpeed, maxSpeed, t);
            currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, acceleration * Time.deltaTime);
        }

        transform.position += Vector3.up * currentSpeed * Time.deltaTime;

        int playerLane = GetPlayerLane();
        float targetX = lanePositions[playerLane];
        Vector3 newPos = transform.position;
        newPos.x = Mathf.Lerp(transform.position.x, targetX, laneChangeSpeed * Time.deltaTime);
        transform.position = newPos;
    }
}
