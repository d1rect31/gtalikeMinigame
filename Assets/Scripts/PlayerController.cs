using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float laneDistance = 2f;
    public float laneChangeSpeed = 10f;
    public int currentLane = 1;
    private Vector3 targetPosition;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);

        if ((Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) && currentLane > 0)
        {
            currentLane--;
            MoveToLane();
        }
        if ((Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) && currentLane < 2)
        {
            currentLane++;
            MoveToLane();
        }
        Vector3 newPosition = transform.position;
        newPosition.x = Mathf.Lerp(transform.position.x, targetPosition.x, laneChangeSpeed * Time.deltaTime);
        transform.position = newPosition;
    }

    void MoveToLane()
    {
        targetPosition = transform.position;
        targetPosition.x = (currentLane - 1) * laneDistance;
    }
}
