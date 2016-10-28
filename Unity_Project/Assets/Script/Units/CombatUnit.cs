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
    protected List<Unit> m_detectedEnemies = new List<Unit>();

    [SerializeField]
    protected Unit m_currentTarget = null;

    [Header("Weapons")]
    [Tooltip("Unit's Weapons list.")]
    public List<Weapon> m_weapons = new List<Weapon>();

    protected override void Reset()
    {
        base.Reset();
    }

    protected override void Start()
    {
        base.Start();

        m_radar = GetComponent<SphereCollider>();
        m_radar.isTrigger = true;
        UpdateRadarRange();
    }

    #region Radar Related
    protected void UpdateRadarRange()
    {
        m_radar.radius = m_radarRange;
    }

    protected virtual void CheckCurrentTarget()
    {
        if (m_currentTarget && m_currentTarget.IsDestroyed())
        {
            m_detectedEnemies.Remove(m_currentTarget);
            m_currentTarget = null;
        }
    }

    public void DetectedUnitDestroyed(Unit destroyedUnit)
    {
        m_detectedEnemies.Remove(destroyedUnit);
    }

    public void TargetedUnitDestroyed(Unit destroyedUnit)
    {
        if (destroyedUnit == m_currentTarget)
            m_currentTarget = null;
    }

    protected virtual void OnTriggerEnter(Collider col)
    {
        if (!col.isTrigger)
        {
            Unit detectedUnit = col.GetComponent<Unit>();
            if (detectedUnit != null && detectedUnit.m_faction != m_faction && !detectedUnit.IsDestroyed())
            {
                m_detectedEnemies.Add(detectedUnit);
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
                m_detectedEnemies.Remove(detectedUnit);
            }
        }
    }
    #endregion

    #region Attacks Related
    protected bool IsTargetInFullOptimalRange()
    {
        if (m_currentTarget)
        {
            if (m_weapons.Count > 0)
            {
                foreach (Weapon weapon in m_weapons)
                {
                    if (!weapon.IsTargetInOptimalRange(m_currentTarget.m_targetPoint.position))
                    {
                        return false;
                    }
                }
                return true;
            }
        }
        return false;
    }
    #endregion

    #region Updates
    protected override void Update()
    {
        base.Update();
    }
    #endregion
}
