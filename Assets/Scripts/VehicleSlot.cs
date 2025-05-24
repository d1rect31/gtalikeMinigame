using UnityEngine;

public class VehicleSlot : MonoBehaviour
{
    public bool IsOccupied { get; private set; } = false;
    public EnemyCar Occupant { get; private set; }

    public void Occupy(EnemyCar car)
    {
        IsOccupied = true;
        Occupant = car;
    }

    public void Release()
    {
        IsOccupied = false;
        Occupant = null;
    }
}
