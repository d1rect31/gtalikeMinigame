using UnityEngine;

public class EnemyCar : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float stopDistance = .1f;
    private VehicleSlot targetSlot;
    private Vector3 velocity = Vector3.zero;
    public float smoothTime = 0.2f;
    private Transform player;
    private static VehicleSlot[] cachedSlots;
    void Start()
    {
        cachedSlots ??= FindObjectsOfType<VehicleSlot>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        TryAssignSlot();
    }

    void Update()
    {
        if (targetSlot == null) return;

        // Проверка: если игрок близко к слоту, ищем другой слот
        if (player != null && Vector3.Distance(player.position, targetSlot.transform.position) < 1.0f)
        {
            targetSlot.Release();
            TryAssignSlot();
            return;
        }

        Vector3 targetPos = targetSlot.transform.position;
        targetPos.z = transform.position.z;

        float distance = Vector3.Distance(transform.position, targetPos);
        if (distance > stopDistance)
        {
            transform.position = Vector3.SmoothDamp(
                transform.position,
                targetPos,
                ref velocity,
                smoothTime,
                moveSpeed
            );
        }
    }

    // Попытка занять ближайший свободный слот
    
    private void TryAssignSlot()
    {
        targetSlot = FindNearestFreeSlot(cachedSlots);

        if (targetSlot != null)
        {
            if (targetSlot.IsOccupied)
            {
                // Сигнал машине-обитателю освободить слот
                Debug.Log($"[EnemyCar] Слот {targetSlot.name} занят машиной {targetSlot.Occupant.name}. Запрос на перемещение к следующему слоту.");
                targetSlot.Occupant.RequestMoveToNextSlot();
            }
            else
            {
                targetSlot.Occupy(this);
            }
        }
        else
        {
            Debug.LogWarning("Нет свободных слотов для EnemyCar!");
        }
    }

    // По сигналу от другой машины ищем следующий свободный слот
    public void RequestMoveToNextSlot()
    {
        if (targetSlot != null)
            targetSlot.Release();

        TryAssignSlot();
    }

    private VehicleSlot FindNearestFreeSlot(VehicleSlot[] slots)
    {
        VehicleSlot nearest = null;
        float minDist = float.MaxValue;
        foreach (var slot in slots)
        {
            // Игнорируем слот, если он занят игроком
            if (player != null && Vector3.Distance(player.position, slot.transform.position) < 1.0f)
                continue;

            if (slot.IsOccupied && slot.Occupant != this) continue;

            float dist = Vector3.Distance(transform.position, slot.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = slot;
            }
        }
        return nearest;
    }

    private void OnDestroy()
    {
        if (targetSlot != null)
            targetSlot.Release();
    }
}
