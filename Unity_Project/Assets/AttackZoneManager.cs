using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackZoneManager : MonoBehaviour
{
    public List<AttackZone> zones = new List<AttackZone>();

    public AttackZone betterZone;

    void Start()
    {
        betterZone = zones[0];
    }
    
    void Update()
    {
        foreach (AttackZone zone in zones)
        {
            if (zone.collidersNbr < betterZone.collidersNbr)
            {
                betterZone = zone;
            }
        }
    }
}
