using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SphereCollider))]
public class Radar : MonoBehaviour
{
    [SerializeField]
    private CombatUnit m_combatUnit;

	void Start ()
    {
        m_combatUnit = GetComponentInParent<CombatUnit>();
	}

    protected virtual void OnTriggerEnter(Collider col)
    {
        if (m_combatUnit.m_faction != Unit.UnitFaction.Neutral)
        {
            if (!col.isTrigger)
            {
                Unit detectedUnit = col.GetComponent<Unit>();
                if (detectedUnit != null && detectedUnit.m_faction != m_combatUnit.m_faction && !detectedUnit.IsDestroyed())
                {
                    m_combatUnit.m_detectedEnemies.Add(detectedUnit);
                }
            }
        }

    }

    protected virtual void OnTriggerExit(Collider col)
    {
        if (!col.isTrigger)
        {
            Unit detectedUnit = col.GetComponent<Unit>();
            if (detectedUnit != null && detectedUnit.m_faction != m_combatUnit.m_faction)
            {
                m_combatUnit.m_detectedEnemies.Remove(detectedUnit);
            }
        }
    }

    void Update ()
    {
	    
	}
}
