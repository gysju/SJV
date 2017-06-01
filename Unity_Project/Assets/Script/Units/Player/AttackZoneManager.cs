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

    public Transform ClosestBetterZone(Vector3 attackerPosition)
    {
        AttackZone closestBestZone = betterZone;

        foreach (AttackZone zone in zones)
        {
            if (zone.clearView)
            {
                if (Vector3.Distance(zone.m_transform.position, attackerPosition) < Vector3.Distance(closestBestZone.m_transform.position, attackerPosition))
                {
                    //if (zone.collidersNbr < closestBestZone.collidersNbr)
                        closestBestZone = zone;
                }
            }
        }

        return closestBestZone.m_transform;
    }
    
    void Update()
    {
        betterZone = null;
        foreach (AttackZone zone in zones)
        {
            if (zone.clearView)
            {
                if (!betterZone || zone.collidersNbr < betterZone.collidersNbr)
                    betterZone = zone;
            }
        }
    }
}
