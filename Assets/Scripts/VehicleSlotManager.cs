using UnityEngine;
using System.Collections.Generic;

public class VehicleSlotManager : MonoBehaviour
{
    public static VehicleSlotManager Instance { get; private set; }

    private List<VehicleSlot> slots = new List<VehicleSlot>();

    void Awake()
    {
        Instance = this;
        // Собираем все VehicleSlot среди дочерних объектов
        slots.Clear();
        foreach (Transform child in transform)
        {
            var slot = child.GetComponent<VehicleSlot>();
            if (slot != null)
                slots.Add(slot);
        }
    }

    public VehicleSlot FindNearestFreeSlot(Vector3 position, Transform player, float minPlayerDistance = 1.0f)
    {
        VehicleSlot nearest = null;
        float minDist = float.MaxValue;
        foreach (var slot in slots)
        {
            if (player != null && Vector3.Distance(player.position, slot.transform.position) < minPlayerDistance)
                continue;
            if (slot.IsOccupied)
                continue;
            float dist = Vector3.Distance(position, slot.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = slot;
            }
        }
        return nearest;
    }

    public void RegisterSlot(VehicleSlot slot)
    {
        if (!slots.Contains(slot))
            slots.Add(slot);
    }

    public void UnregisterSlot(VehicleSlot slot)
    {
        slots.Remove(slot);
    }

    public IEnumerable<VehicleSlot> AllSlots => slots;
}
