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
    public List<Unit> m_detectedEnemies = new List<Unit>();

    [SerializeField]
    //protected Unit m_currentTarget = null;

    [Header("Weapons")]
    [Tooltip("Unit's Weapons list.")]
    public List<Weapon> m_weapons = new List<Weapon>();

    #region Initialization
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
    #endregion

    #region HitPoints Related
    protected override void Die()
    {
        base.Die();

        m_radar.enabled = false;
    }
    #endregion

    #region Radar Related
    protected void UpdateRadarRange()
    {
        m_radar.radius = m_radarRange;
    }

    public void DetectedUnitDestroyed(Unit destroyedUnit)
    {
        m_detectedEnemies.Remove(destroyedUnit);
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

    #region Attack Related
    public void PressWeaponTrigger(int weaponID)
    {
        m_weapons[weaponID].TriggerPressed();
    }

    public void ReleaseWeaponTrigger(int weaponID)
    {
        m_weapons[weaponID].TriggerReleased();
    }
    #endregion

    #region Updates
    protected override void Update()
    {
        if (!m_destroyed)
        {
            base.Update();
        }
    }
    #endregion
}
