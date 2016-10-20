using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("MechaVR/Units/DEV/Combat Unit")]
[RequireComponent(typeof(SphereCollider))]
public class CombatUnit : Unit
{
    protected SphereCollider m_radar;

    [Header("Radar")]
    [Tooltip("Unit's radar's range.")]
    [Range(0f, 50f)]
    public float m_radarRange = 50f;

    [SerializeField]
    protected List<Unit> m_possibleTargets = new List<Unit>();

    [SerializeField]
    protected Unit m_currentTarget = null;

    [Header("Weapons")]
    [Tooltip("Unit's Weapons list.")]
    public List<Weapon> m_weapons = new List<Weapon>();

    protected override void Reset()
    {
        base.Reset();

        m_radar = GetComponent<SphereCollider>();
        m_radar.isTrigger = true;
        UpdateRadarRange();
    }

    protected override void Start()
    {
        base.Start();
        UpdateRadarRange();
    }

    #region Radar Related
    protected void UpdateRadarRange()
    {
        m_radar.radius = m_radarRange;
    }

    protected void CheckTargetsStatus()
    {
        foreach (Unit potentialTarget in m_possibleTargets)
        {
            if (potentialTarget.IsDestroyed()) m_possibleTargets.Remove(potentialTarget);
        }
    }

    protected virtual void OnTriggerEnter(Collider col)
    {
        if (!col.isTrigger)
        {
            Unit detectedUnit = col.GetComponent<Unit>();
            if (detectedUnit != null && detectedUnit.m_faction != m_faction && !detectedUnit.IsDestroyed())
            {
                m_possibleTargets.Add(detectedUnit);
            }
        }
    }

    protected virtual void OnTriggerExit(Collider col)
    {
        if (!col.isTrigger)
        {
            Unit detectedUnit = col.GetComponent<Unit>();
            if (detectedUnit != null && detectedUnit.m_faction != m_faction)
            {
                m_possibleTargets.Remove(detectedUnit);
            }
        }
    }
    #endregion

    #region Updates
    protected override void Update()
    {
        base.Update();
    }
    #endregion
}
