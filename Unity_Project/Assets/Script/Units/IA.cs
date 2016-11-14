using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CombatUnit))]
public class IA : MonoBehaviour
{
    [Header("IA Commanders")]
    public IACommander m_allyCommander;
    public IACommander m_enemyCommander;
    
    protected Unit m_unit = null;
    protected CombatUnit m_combatUnit = null;
    protected MobileGroundUnit m_mobileGroundUnit = null;
    protected AirUnit m_airUnit = null;

    [Header("Combat Related")]
    [SerializeField]
    protected Unit m_currentTarget = null;
    [Tooltip("IA's imprecision angle.")]
    [Range(0.1f, 10.0f)]
    public float m_imprecisioAngle = 10.0f;
    
    [Header("Order Specifics")]
	[SerializeField]
    Capture_point m_pointToCapture;
    Unit m_unitToDestroy;

    #region Initialization
    void Start ()
    {
        m_allyCommander = GameObject.Find("Ally Commander").GetComponent<IACommander>();
        m_enemyCommander = GameObject.Find("Enemy Commander").GetComponent<IACommander>();

        m_unit = transform.GetComponent<Unit>();
        if (m_unit is CombatUnit)
        {
            m_combatUnit = (CombatUnit)m_unit;
        }
        if (m_unit is MobileGroundUnit)
        {
            m_mobileGroundUnit = (MobileGroundUnit)m_unit;
        }
        if (m_mobileGroundUnit is AirUnit)
        {
            m_airUnit = (AirUnit)m_mobileGroundUnit;
        }

        m_pointToCapture = null;
        m_unitToDestroy = null;
        if (m_mobileGroundUnit) AskOrder();
    }
    #endregion

    #region CombatUnit Related

    #region Targeting Related
    protected void TargetClosestDetectedEnemy()
    {
        if (m_combatUnit)
        {
            if (m_combatUnit.m_detectedEnemies.Count > 0)
            {
                for (int i = m_combatUnit.m_detectedEnemies.Count - 1; i > -1; i--)
                {
                    Unit potentialTarget = m_combatUnit.m_detectedEnemies[i];
                    if (potentialTarget)
                    {
                        if (!m_currentTarget) m_currentTarget = potentialTarget;
                        else
                        {
                            float currentTargetDistance = Vector3.Distance(m_currentTarget.m_targetPoint.position, transform.position);
                            float potentialTargetDistance = Vector3.Distance(potentialTarget.m_targetPoint.position, transform.position);

                            if (potentialTargetDistance < currentTargetDistance) m_currentTarget = potentialTarget;
                        }
                    }
                    else
                    {
                        m_combatUnit.m_detectedEnemies.Remove(potentialTarget);
                    }
                }
            }
            else
            {
                m_currentTarget = null;
                m_combatUnit.CeaseFire();
            }
        }
    }

    protected virtual void CheckCurrentTargetStatus()
    {
        if (m_currentTarget && m_currentTarget.IsDestroyed())
        {
            m_combatUnit.m_detectedEnemies.Remove(m_currentTarget);
            m_currentTarget = null;
        }
    }

    public void TargetedUnitDestroyed(Unit destroyedUnit)
    {
        if (destroyedUnit == m_currentTarget)
            m_currentTarget = null;
    }
    #endregion

    #region Aim Related
    protected bool IsWeaponAimingTarget(Weapon weapon)
    {
        return weapon.IsInAim(m_currentTarget.m_targetPoint.position, m_imprecisioAngle);
    }

    protected bool IsTargetInOptimalRange(Weapon weapon)
    {
        return weapon.IsInOptimalRange(m_currentTarget.m_targetPoint.position);
    }

    protected bool IsTargetInFullOptimalRange()
    {
        if (m_currentTarget)
        {
            if (m_combatUnit.m_weapons.Count > 0)
            {
                foreach (Weapon weapon in m_combatUnit.m_weapons)
                {
                    if (!weapon.IsInOptimalRange(m_currentTarget.m_targetPoint.position))
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

    #endregion

    #region MobileUnit Related

    #region Order Related
    public void AskOrder()
    {
        switch (m_mobileGroundUnit.m_faction)
        {
            case Unit.UnitFaction.Ally:
                m_allyCommander.AskMovementOrder(this);
                break;
            case Unit.UnitFaction.Neutral:
                break;
            case Unit.UnitFaction.Enemy:
                m_enemyCommander.AskMovementOrder(this);
                break;
            default:
                break;
        }
    }

    public void GiveCaptureOrder(Capture_point pointToCapture)
    {
        m_unitToDestroy = null;
        if (m_pointToCapture != pointToCapture)
        {
            m_pointToCapture = pointToCapture;
            m_mobileGroundUnit.SetDestination(m_pointToCapture.transform.position);
        }
    }

    private void CheckCaptureOrder()
    {
        if (m_pointToCapture.IsSameFaction(m_mobileGroundUnit.m_faction))
        {
            m_pointToCapture = null;
            CancelMoveOrder();
            AskOrder();
        }
    }

    public void GiverDestroyOrder(Unit unitToDestroy)
    {
        m_pointToCapture = null;
        m_unitToDestroy = unitToDestroy;
        m_mobileGroundUnit.SetDestination(m_unitToDestroy.transform.position);
    }

    private void CheckDestroyOrder()
    {
        if (m_unitToDestroy.IsDestroyed())
        {
            m_unitToDestroy = null;
            CancelMoveOrder();
            AskOrder();
        }
    }

    private void ResumeCurrentOrder()
    {
        if (m_pointToCapture)
            GiveCaptureOrder(m_pointToCapture);
        else if (m_unitToDestroy)
            GiverDestroyOrder(m_unitToDestroy);
        else
            AskOrder();
    }

    public void CancelMoveOrder()
    {
        m_mobileGroundUnit.CancelPath();
    }

    private void CheckCurrentOrder()
    {
        if (m_pointToCapture)
            CheckCaptureOrder();
        else if (m_unitToDestroy)
            CheckDestroyOrder();
    }
    #endregion

    #endregion

    #region IA
    protected void ChooseTarget()
    {
        TargetClosestDetectedEnemy();
    }

    protected void Behaviour()
    {
        if (m_currentTarget)
        {
            if (m_mobileGroundUnit)
            {
                if (IsTargetInFullOptimalRange())
                    m_mobileGroundUnit.PausePath();
                else
                    m_mobileGroundUnit.ResumePath();
            }


            m_combatUnit.AimWeaponAt(m_currentTarget.m_targetPoint.position);

            foreach (Weapon weapon in m_combatUnit.m_weapons)
            {
                if (IsTargetInOptimalRange(weapon) && IsWeaponAimingTarget(weapon))
                    weapon.TriggerPressed();
                else
                    weapon.TriggerReleased();
            }
        }
        else
        {
            if (m_mobileGroundUnit)
                ResumeCurrentOrder();
        }
    }
    #endregion

    #region Updates
    void Update ()
    {
        if (m_mobileGroundUnit)
        {
            CheckCurrentOrder();
        }
        if (m_combatUnit)
        {
            ChooseTarget();
            CheckCurrentTargetStatus();
        }
        Behaviour();
    }
    #endregion
}
