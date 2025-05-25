using UnityEngine;

public class VehicleSlot : MonoBehaviour
{
    public bool IsOccupied { get; private set; } = false;
    public EnemyCar Occupant { get; private set; }
    public VehicleSlot LeftSlot;
    public VehicleSlot RightSlot;
    public VehicleSlot UpSlot;
    public VehicleSlot DownSlot;
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
    public void OccupyByPlayer()
    {
        IsOccupied = true;
        Occupant = null; // или специальная ссылка на Player, если нужно
    }
}
