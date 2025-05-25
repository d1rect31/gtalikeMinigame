using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Guard2D : MonoBehaviour
{
    public float speed = 5f;
    public float waitTime = 0.3f;
    public float turnSpeed = 180f; // Degrees per second
    public Transform pathHolder;

    [Header("Field of View Settings")]
    public float viewDistance = 5f;
    [Range(0, 360)]
    public float viewAngle = 45f;
    public LayerMask viewMask; // Assign layer for obstacles (like "Walls")

    [Header("Game Over UI")]
    public GameObject gameOverPanel; // Assign in the Inspector

    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        Vector2[] waypoints = new Vector2[pathHolder.childCount];
        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = pathHolder.GetChild(i).position;
        }

        StartCoroutine(FollowPath(waypoints));
    }

    void Update()
    {
        if (CanSeePlayer())
        {
            GameOver();
        }
    }

    IEnumerator FollowPath(Vector2[] waypoints)
    {
        transform.position = waypoints[0];
        int targetWaypointIndex = 1;
        Vector2 targetWaypoint = waypoints[targetWaypointIndex];

        while (true)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetWaypoint, speed * Time.deltaTime);

            if ((Vector2)transform.position == targetWaypoint)
            {
                yield return StartCoroutine(Scan360());

                targetWaypointIndex = (targetWaypointIndex + 1) % waypoints.Length;
                targetWaypoint = waypoints[targetWaypointIndex];

                yield return new WaitForSeconds(waitTime);
                yield return StartCoroutine(TurnToFace(targetWaypoint));
            }
            yield return null;
        }
    }

    IEnumerator TurnToFace(Vector2 lookTarget)
    {
        Vector2 dirToLookTarget = (lookTarget - (Vector2)transform.position).normalized;
        float targetAngle = Mathf.Atan2(dirToLookTarget.y, dirToLookTarget.x) * Mathf.Rad2Deg;

        while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.z, targetAngle)) > 0.05f)
        {
            float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.z, targetAngle, turnSpeed * Time.deltaTime);
            transform.eulerAngles = new Vector3(0, 0, angle);
            yield return null;
        }
    }

    IEnumerator Scan360()
    {
        float startAngle = transform.eulerAngles.z;
        float targetAngle = startAngle + 360f;
        float currentAngle = startAngle;

        while (currentAngle < targetAngle)
        {
            currentAngle += turnSpeed * Time.deltaTime;
            transform.eulerAngles = new Vector3(0, 0, currentAngle);
            yield return null;
        }

        transform.eulerAngles = new Vector3(0, 0, currentAngle % 360f);
    }

    bool CanSeePlayer()
    {
        if (player == null) return false;

        Vector2 dirToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < viewDistance)
        {
            float angleToPlayer = Vector2.Angle(transform.right, dirToPlayer);
            if (angleToPlayer < viewAngle)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, dirToPlayer, distanceToPlayer, viewMask);
                if (!hit)
                {
                    return true;
                }
            }
        }
        return false;
    }

    void GameOver()
    {
        if (gameOverPanel != null && !gameOverPanel.activeSelf)
        {
            gameOverPanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    void OnDrawGizmos()
    {
        if (pathHolder == null || pathHolder.childCount < 2) return;

        Vector3 startPosition = pathHolder.GetChild(0).position;
        Vector3 previousPosition = startPosition;

        foreach (Transform waypoint in pathHolder)
        {
            Gizmos.DrawSphere(waypoint.position, 0.2f);
            Gizmos.DrawLine(previousPosition, waypoint.position);
            previousPosition = waypoint.position;
        }
        Gizmos.DrawLine(previousPosition, startPosition);

        // Draw field of view in Scene view
        Gizmos.color = Color.yellow;
        Vector3 rightDir = Quaternion.Euler(0, 0, viewAngle) * transform.right;
        Vector3 leftDir = Quaternion.Euler(0, 0, -viewAngle) * transform.right;
        Gizmos.DrawLine(transform.position, transform.position + rightDir * viewDistance);
        Gizmos.DrawLine(transform.position, transform.position + leftDir * viewDistance);
    }
}
