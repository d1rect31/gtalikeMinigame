using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float laneDistance = 2f;
    public float laneChangeSpeed = 10f;
    public int currentLane = 1;
    private Vector3 targetPosition;
    [SerializeField] private VehicleSlot currentSlot;
    public float slotCaptureDistance = 0.5f; // Радиус захвата слота
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
        targetPosition = startPosition;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<EnemyCar>() != null)
        {
            // Возврат на стартовую позицию
            transform.position = startPosition;
            targetPosition = startPosition;
            currentLane = 1;

            // TODO: Реализовать механику урона игроку
            Debug.Log("Игрок получил урон от столкновения с машиной!");
        }
    }
    // Update is called once per frame
    void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector2.up);

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

        // --- Новая логика освобождения слота ---
        if (currentSlot != null)
        {
            float distance = Vector3.Distance(transform.position, currentSlot.transform.position);
            if (distance > slotCaptureDistance)
            {
                currentSlot.Release();
                currentSlot = null;
            }
        }

        CaptureSlot();
    }

    void MoveToLane()
    {
        targetPosition = transform.position;
        targetPosition.x = (currentLane - 1) * laneDistance;
    }
    void CaptureSlot()
    {
        VehicleSlot closestSlot = null;
        float closestDistance = float.MaxValue;
        foreach (var slot in FindObjectsByType<VehicleSlot>(FindObjectsSortMode.None))
        {
            float distance = Vector3.Distance(transform.position, slot.transform.position);
            if (distance < closestDistance && distance < slotCaptureDistance && !slot.IsOccupied)
            {
                closestDistance = distance;
                closestSlot = slot;
            }
        }
        if (closestSlot != null)
        {
            if (currentSlot != null && currentSlot != closestSlot)
            {
                currentSlot.Release();
            }
            currentSlot = closestSlot;
            currentSlot.OccupyByPlayer();
        }
    }

    
}
