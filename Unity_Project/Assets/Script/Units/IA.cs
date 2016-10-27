using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CombatUnit))]
public class IA : MonoBehaviour
{
    Unit m_unit;
    
    bool m_mobileUnit;

    #region Initialization
    void Start ()
    {
        m_unit = transform.GetComponent<Unit>();
	}
    #endregion

    #region CombatUnit Related
    #region Targeting Related
    //protected void TargetClosestEnemy()
    //{
    //    if (m_detectedEnemies.Count > 0)
    //    {
    //        for (int i = m_detectedEnemies.Count - 1; i > -1; i--)
    //        {
    //            Unit potentialTarget = m_detectedEnemies[i];
    //            if (potentialTarget)
    //            {
    //                if (!m_currentTarget) m_currentTarget = potentialTarget;
    //                else
    //                {
    //                    float currentTargetDistance = Vector3.Distance(m_currentTarget.m_targetPoint.position, transform.position);
    //                    float potentialTargetDistance = Vector3.Distance(potentialTarget.m_targetPoint.position, transform.position);

    //                    if (potentialTargetDistance < currentTargetDistance) m_currentTarget = potentialTarget;
    //                }
    //            }
    //            else
    //            {
    //                m_detectedEnemies.Remove(potentialTarget);
    //            }
    //        }

    //        //foreach (Unit potentialTarget in m_detectedEnemies)
    //        //{
    //        //    if (potentialTarget)
    //        //    {
    //        //        if (!m_currentTarget) m_currentTarget = potentialTarget;
    //        //        else
    //        //        {
    //        //            float currentTargetDistance = Vector3.Distance(m_currentTarget.m_targetPoint.position, transform.position);
    //        //            float potentialTargetDistance = Vector3.Distance(potentialTarget.m_targetPoint.position, transform.position);

    //        //            if (potentialTargetDistance < currentTargetDistance) m_currentTarget = potentialTarget;
    //        //        }
    //        //    }
    //        //    else
    //        //    {
    //        //        m_detectedEnemies.Remove(potentialTarget);
    //        //    }
    //        //}
    //    }
    //    else
    //    {
    //        m_currentTarget = null;
    //        CeaseFire();
    //    }
    //}

    //protected override void CheckCurrentTarget()
    //{
    //    base.CheckCurrentTarget();

    //    TryAttack();
    //}
    #endregion
    #endregion

    #region MobileUnit Related

    #endregion

    #region Updates
    void Update ()
    {
	    
	}
    #endregion
}
