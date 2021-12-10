using UnityEngine;

public class ShipOnRadar
{
    public Vector3 centerPos { get; private set; }
    public Team team { get; private set; }

    public ShipOnRadar(Vector3 pos, Team t)
    {
        centerPos = pos;
        team = t;
    }
}
