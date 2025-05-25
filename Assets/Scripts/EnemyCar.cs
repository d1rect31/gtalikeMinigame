using UnityEngine;
using System.Collections.Generic;

public class EnemyCar : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float stopDistance = .1f;
    private VehicleSlot targetSlot;
    private Vector3 velocity = Vector3.zero;
    public float smoothTime = 0.2f;
    private Transform player;
    private static VehicleSlot[] cachedSlots;
    public float leadOffset = 2.0f;

    void Start()
    {
        cachedSlots ??= FindObjectsByType<VehicleSlot>(FindObjectsSortMode.None);
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

        // Добавляем опережение по Y (или по нужной оси)
        if (player != null)
            targetPos += Vector3.up * leadOffset;

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
                // Передаём goal — позицию игрока или чуть выше текущей позиции
                Vector3 goal = player != null ? player.position : (transform.position + Vector3.up * 10f);
                targetSlot.Occupant.RequestMoveBySlotChain(goal);
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

        // Цель — позиция игрока, если есть, иначе чуть выше текущей позиции
        Vector3 goal = player != null ? player.position : (transform.position + Vector3.up * 10f);

        foreach (var slot in slots)
        {
            // Игнорируем слот, если он занят игроком
            //if (player != null && Vector3.Distance(player.position, slot.transform.position) < 1.0f)
            //    continue;

            float dist = Vector3.Distance(transform.position, slot.transform.position);

            if (dist < minDist)
            {
                // Если слот свободен — сразу выбираем его
                if (!slot.IsOccupied)
                {
                    minDist = dist;
                    nearest = slot;
                    Debug.Log($"{this.name} Найден ближайший свободный слот: {nearest.name}, расстояние: {minDist}");
                }
                // Если слот занят — пробуем инициировать освобождение
                else if (slot.Occupant != this)
                {
                    Debug.Log($"{this.name} Ближайший слот {slot.name} занят {slot.Occupant?.name ?? "null"}, пробую освободить через цепочку...");
                    slot.Occupant?.RequestMoveBySlotChain(goal);

                    // После рекурсивного вызова проверяем снова
                    if (!slot.IsOccupied)
                    {
                        minDist = dist;
                        nearest = slot;
                        Debug.Log($"{this.name} Слот {slot.name} освободился после цепочки, выбираю его.");
                    }
                }
            }
        }
        if (nearest != null)
            Debug.Log($"{this.name} Итоговый ближайший слот: {nearest.name}, расстояние: {minDist}");
        else
            Debug.Log($"{this.name} Не найден подходящий слот!");

        return nearest;
    }

    private void OnDestroy()
    {
        if (targetSlot != null)
            targetSlot.Release();
    }
    public void RequestMoveBySlotChain(Vector3 goal)
    {
        if (targetSlot == null) return;

        // Собираем все соседние слоты
        List<VehicleSlot> candidates = new List<VehicleSlot>();
        if (targetSlot.LeftSlot != null) candidates.Add(targetSlot.LeftSlot);
        if (targetSlot.RightSlot != null) candidates.Add(targetSlot.RightSlot);
        if (targetSlot.UpSlot != null) candidates.Add(targetSlot.UpSlot);
        if (targetSlot.DownSlot != null) candidates.Add(targetSlot.DownSlot);

        // Сортируем по расстоянию до цели (например, вверх)
        candidates.Sort((a, b) =>
            Vector3.Distance(a.transform.position, goal).CompareTo(
            Vector3.Distance(b.transform.position, goal)));

        foreach (var slot in candidates)
        {
            if (!slot.IsOccupied)
            {
                MoveToSlot(slot);
                return;
            }
        }

        // Если все заняты — рекурсивно освобождаем ближайший к цели
        foreach (var slot in candidates)
        {
            if (slot.IsOccupied && slot.Occupant != this)
            {
                slot.Occupant.RequestMoveBySlotChain(goal);
                if (!slot.IsOccupied)
                {
                    MoveToSlot(slot);
                    return;
                }
            }
        }

        Debug.Log($"[EnemyCar:{name}] Не могу сдвинуться: все соседние слоты заняты.");
    }

    private void MoveToSlot(VehicleSlot newSlot)
    {
        if (targetSlot != null)
            targetSlot.Release();

        targetSlot = newSlot;
        targetSlot.Occupy(this);
    }
}
