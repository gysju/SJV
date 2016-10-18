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

    protected override void Start()
    {
        base.Start();
        m_radar = GetComponent<SphereCollider>();
        m_radar.isTrigger = true;
        m_radar.radius = m_radarRange;
    }

    #region Targeting Related
    //protected void ChooseTarget()
    //{
    //    if (m_possibleTargets.Count > 0)
    //    {
    //        foreach (Unit potentialTarget in m_possibleTargets)
    //        {
    //            if (!m_currentTarget) m_currentTarget = potentialTarget;
    //            else
    //            {
    //                float currentTargetDistance = Vector3.Distance(m_currentTarget.transform.position, transform.position);
    //                float potentialTargetDistance = Vector3.Distance(potentialTarget.transform.position, transform.position);

    //                if (potentialTargetDistance < currentTargetDistance) m_currentTarget = potentialTarget;
    //            }
    //        }
    //    }
    //    else m_currentTarget = null;
    //}

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

    #region Attack Related
    //protected bool CheckAim(Weapon weapon)
    //{
    //    RaycastHit hit;
    //    if (Physics.Raycast(weapon.m_muzzle.position, weapon.m_muzzle.forward, out hit))
    //    {
    //        Unit unit = hit.collider.GetComponent<Unit>();

    //        if (unit == m_currentTarget)
    //            return true;
    //    }

    //    return false;
    //}

    //protected void CheckTargetDistanceToFire()
    //{
    //    if (m_currentTarget)
    //    {
    //        foreach (Weapon weapon in m_weapons)
    //        {
    //            if (weapon != null)
    //            {
    //                float currentTargetDistance = Vector3.Distance(m_currentTarget.transform.position, transform.position);
                    
    //                if (weapon.m_optimalRange > currentTargetDistance && CheckAim(weapon)) weapon.TriggerPressed();
    //            }
    //        }
    //    }
    //}
    #endregion

    #region Updates
    protected override void Update()
    {
        base.Update();
        //ChooseTarget();
        //CheckTargetDistanceToFire();
    }
    #endregion
}
