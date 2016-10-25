﻿using UnityEngine;
using System.Collections;

[AddComponentMenu("MechaVR/Units/Hover Tank")]
public class HoverTank : MobileGroundUnit
{
    [Header("IA Commanders")]
    public IACommander m_allyCommander;
    public IACommander m_enemyCommander;

    [Header("Order Specifics")]
    Capture_point m_pointToCapture;
    Unit m_unitToDestroy;

    [Header("Turret specifics")]
    public Transform m_turretBase;

    [Tooltip("Rotation speed in degrees of the turret")]
    [Range(0.1f, 360.0f)]
    public float m_turretDegreesPerSecond = 45.0f;

    [Tooltip("Rotation speed in degrees of the cannon")]
    [Range(0.1f, 360.0f)]
    public float m_cannonDegreesPerSecond = 45.0f;

    [Tooltip("Turret's cannon's max angle.")]
    [Range(0.0f, 180f)]
    public float m_maxCannonAngle = 45.0f;

    [Tooltip("Turret's imprecision angle.")]
    [Range(0.1f, 10.0f)]
    public float m_imprecisioAngle = 10.0f;

    #region Initialisation
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        m_allyCommander = GameObject.Find("Ally Commander").GetComponent<IACommander>();
        m_enemyCommander = GameObject.Find("Enemy Commander").GetComponent<IACommander>();
        m_pointToCapture = null;
        m_unitToDestroy = null;
        AskOrder();
    }
    #endregion

    #region Order Related
    public void AskOrder()
    {
        switch (m_faction)
        {
            case UnitFaction.Ally:
                m_allyCommander.AskOrder(this);
                break;
            case UnitFaction.Neutral:
                break;
            case UnitFaction.Enemy:
                m_enemyCommander.AskOrder(this);
                break;
            default:
                break;
        }
    }

    public void GiveCaptureOrder(Capture_point pointToCapture)
    {
        m_pointToCapture = pointToCapture;
        GiveMoveOrder(m_pointToCapture.transform.position);
    }

    private void CheckCaptureOrder()
    {
        if (m_pointToCapture.IsSameFaction(m_faction))
        {
            m_pointToCapture = null;
            m_navMeshAgent.ResetPath();
            AskOrder();
        }
    }
    #endregion

    #region Targeting Related
    protected void ChooseTarget()
    {
        m_currentTarget = null;
        if (m_possibleTargets.Count > 0)
        {
            foreach (Unit potentialTarget in m_possibleTargets)
            {
                if (!m_currentTarget) m_currentTarget = potentialTarget;
                else
                {
                    float currentTargetDistance = Vector3.Distance(m_currentTarget.m_targetPoint.position, transform.position);
                    float potentialTargetDistance = Vector3.Distance(potentialTarget.m_targetPoint.position, transform.position);

                    if (potentialTargetDistance < currentTargetDistance) m_currentTarget = potentialTarget;
                }
            }
        }
        else
        {
            m_currentTarget = null;
            CeaseFire();
        }
    }

    protected void CheckMoveOrder()
    {
        if (m_navMeshAgent.hasPath)
        {
            ResumeMoveOrder();
        }
        else
        {
            AskOrder();
        }
    }
    #endregion

    #region Attack Related
    private void AimTarget()
    {
        Quaternion qTurret;
        Quaternion qGun;

        float distanceToTarget = Vector3.Dot(m_turretBase.transform.up, m_currentTarget.m_targetPoint.position - m_turretBase.position);
        Vector3 planePoint = m_currentTarget.m_targetPoint.position - m_turretBase.transform.up * distanceToTarget;

        qTurret = Quaternion.LookRotation(planePoint - m_turretBase.position, transform.up);
        m_turretBase.rotation = Quaternion.RotateTowards(m_turretBase.rotation, qTurret, m_turretDegreesPerSecond * Time.deltaTime);

        Vector3 v3 = new Vector3(0.0f, distanceToTarget, (planePoint - m_turretBase.position).magnitude);
        qGun = Quaternion.LookRotation(v3);

        foreach (Weapon weapon in m_weapons)//le pivot de l'arme doit être au point d'ancrage
        {
            if (Quaternion.Angle(weapon.transform.localRotation, qGun) <= m_maxCannonAngle)
                weapon.transform.localRotation = Quaternion.RotateTowards(weapon.transform.localRotation, qGun, m_cannonDegreesPerSecond * Time.deltaTime);
        }
    }

    private bool IsTargetInAim(Weapon weapon)
    {
        Vector3 targetDir = m_currentTarget.m_targetPoint.position - weapon.m_muzzle.position;
        float angle = Vector3.Angle(targetDir, weapon.m_muzzle.forward);

        if (angle <= m_imprecisioAngle)
            return true;

        return false;
    }

    private void Shoot()
    {
        foreach (Weapon weapon in m_weapons)
        {
            if (IsTargetInAim(weapon) && weapon.IsTargetInOptimalRange(m_currentTarget.m_targetPoint.position) && m_currentTarget)
            {
                weapon.TriggerPressed();
            }
            else weapon.TriggerReleased();
        }
    }

    protected void TryAttack()
    {
        if (m_currentTarget)
        {
            AimTarget();
            Shoot();
        }
    }

    protected void CeaseFire()
    {
        foreach (Weapon weapon in m_weapons)
        {
            weapon.TriggerReleased();
        }
    }
    #endregion

    #region IA Related
    protected void IA()
    {
        if (m_pointToCapture)
            CheckCaptureOrder();
        ChooseTarget();
        if (IsTargetDestroyed())
            CheckMoveOrder();
        else
        {
            TryAttack();
            if (IsTargetInFullOptimalRange())
                PauseMoveOrder();
            else
                ResumeMoveOrder();
        }
        
    }
    #endregion

    #region Updates
    protected override void Update()
    {
        if (!m_destroyed)
        {
            base.Update();
            IA();
        }
    }
    #endregion
}
