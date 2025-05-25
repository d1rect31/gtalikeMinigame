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

        // ��������: ���� ����� ������ � �����, ���� ������ ����
        if (player != null && Vector3.Distance(player.position, targetSlot.transform.position) < 1.0f)
        {
            targetSlot.Release();
            TryAssignSlot();
            return;
        }

        Vector3 targetPos = targetSlot.transform.position;

        // ��������� ���������� �� Y (��� �� ������ ���)
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

    // ������� ������ ��������� ��������� ����

    private void TryAssignSlot()
    {
        targetSlot = FindNearestFreeSlot(cachedSlots);

        if (targetSlot != null)
        {
            if (targetSlot.IsOccupied)
            {
                // ������� goal � ������� ������ ��� ���� ���� ������� �������
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
            Debug.LogWarning("��� ��������� ������ ��� EnemyCar!");
        }
    }

    // �� ������� �� ������ ������ ���� ��������� ��������� ����
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

        // ���� � ������� ������, ���� ����, ����� ���� ���� ������� �������
        Vector3 goal = player != null ? player.position : (transform.position + Vector3.up * 10f);

        foreach (var slot in slots)
        {
            // ���������� ����, ���� �� ����� �������
            //if (player != null && Vector3.Distance(player.position, slot.transform.position) < 1.0f)
            //    continue;

            float dist = Vector3.Distance(transform.position, slot.transform.position);

            if (dist < minDist)
            {
                // ���� ���� �������� � ����� �������� ���
                if (!slot.IsOccupied)
                {
                    minDist = dist;
                    nearest = slot;
                    Debug.Log($"{this.name} ������ ��������� ��������� ����: {nearest.name}, ����������: {minDist}");
                }
                // ���� ���� ����� � ������� ������������ ������������
                else if (slot.Occupant != this)
                {
                    Debug.Log($"{this.name} ��������� ���� {slot.name} ����� {slot.Occupant?.name ?? "null"}, ������ ���������� ����� �������...");
                    slot.Occupant?.RequestMoveBySlotChain(goal);

                    // ����� ������������ ������ ��������� �����
                    if (!slot.IsOccupied)
                    {
                        minDist = dist;
                        nearest = slot;
                        Debug.Log($"{this.name} ���� {slot.name} ����������� ����� �������, ������� ���.");
                    }
                }
            }
        }
        if (nearest != null)
            Debug.Log($"{this.name} �������� ��������� ����: {nearest.name}, ����������: {minDist}");
        else
            Debug.Log($"{this.name} �� ������ ���������� ����!");

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

        // �������� ��� �������� �����
        List<VehicleSlot> candidates = new List<VehicleSlot>();
        if (targetSlot.LeftSlot != null) candidates.Add(targetSlot.LeftSlot);
        if (targetSlot.RightSlot != null) candidates.Add(targetSlot.RightSlot);
        if (targetSlot.UpSlot != null) candidates.Add(targetSlot.UpSlot);
        if (targetSlot.DownSlot != null) candidates.Add(targetSlot.DownSlot);

        // ��������� �� ���������� �� ���� (��������, �����)
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

        // ���� ��� ������ � ���������� ����������� ��������� � ����
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

        Debug.Log($"[EnemyCar:{name}] �� ���� ����������: ��� �������� ����� ������.");
    }

    private void MoveToSlot(VehicleSlot newSlot)
    {
        if (targetSlot != null)
            targetSlot.Release();

        targetSlot = newSlot;
        targetSlot.Occupy(this);
    }
}
