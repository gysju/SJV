using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("MechaVR/Units/DEV/Combat Unit")]
[RequireComponent(typeof(SphereCollider))]
public class CombatUnit : Unit
{
    [Header("Radar")]
    protected SphereCollider m_radar;

    [Tooltip("Unit's radar's range.")]
    [Range(0f, 50f)]
    public float m_radarRange = 50f;

    [SerializeField]
    protected List<Unit> m_possibleTargets = new List<Unit>();

    [SerializeField]
    protected Unit m_currentTarget = null;

    public List<Weapon> m_weapons = new List<Weapon>();

    protected override void Start()
    {
        base.Start();
        m_radar = GetComponent<SphereCollider>();
        m_radar.isTrigger = true;
        m_radar.radius = m_radarRange;
    }

    #region Targeting Related
    protected void ChooseTarget()
    {
        if (m_possibleTargets.Count > 0)
        {
            foreach (Unit potentialTarget in m_possibleTargets)
            {
                if (!m_currentTarget) m_currentTarget = potentialTarget;
                else
                {
                    float currentTargetDistance = Vector3.Distance(m_currentTarget.transform.position, transform.position);
                    float potentialTargetDistance = Vector3.Distance(potentialTarget.transform.position, transform.position);

                    if (potentialTargetDistance < currentTargetDistance) m_currentTarget = potentialTarget;
                }
            }
        }
        else m_currentTarget = null;
    }

    void OnTriggerEnter(Collider col)
    {
        if (!col.isTrigger)
        {
            Unit detectedUnit = col.GetComponent<Unit>();
            if (detectedUnit != null && detectedUnit.m_faction != m_faction)
            {
                m_possibleTargets.Add(detectedUnit);
            }
        }
    }

    void OnTriggerExit(Collider col)
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

    #endregion

    #region Updates
    protected override void Update()
    {
        base.Update();
        ChooseTarget();
    }
    #endregion
}
