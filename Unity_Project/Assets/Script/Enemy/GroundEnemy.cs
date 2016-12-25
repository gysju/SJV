﻿using UnityEngine;
using System.Collections;

public class GroundEnemy : BaseEnemy
{
    protected UnityEngine.AI.NavMeshAgent m_navMeshAgent;

    [Header("Mobility")]
    public float m_maxSpeed = 2f;
    public float m_acceleration = 8f;
    public float m_rotationSpeed = 50f;

    #region Initialization
    protected override void Awake()
    {
        base.Awake();
        m_navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        m_navMeshAgent.stoppingDistance = 0.5f;
        m_navMeshAgent.speed = m_maxSpeed;
        m_navMeshAgent.acceleration = m_acceleration;
        m_navMeshAgent.angularSpeed = m_rotationSpeed;
    }

    protected override void Start()
    {
        base.Start();
        if(m_attackPosition.HasValue) m_navMeshAgent.SetDestination(m_attackPosition.Value);
    }

    public override void ResetUnit(Vector3 spawn, Vector3 movementTarget, Transform target)
    {
        m_navMeshAgent.ResetPath();
        base.ResetUnit(spawn, movementTarget, target);
    }
    #endregion

    #region Movement Related
    public override void StartMovement()
    {
        m_enemyState = EnemyState.EnemyState_Moving;
        m_navMeshAgent.SetDestination(m_attackPosition.Value);
    }

    protected bool IsPathCompleted()
    {
        if (!m_navMeshAgent.pathPending)
        {
            if (m_navMeshAgent.remainingDistance <= m_navMeshAgent.stoppingDistance)
            {
                if (!m_navMeshAgent.hasPath || m_navMeshAgent.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        }
        return false;
    }

    protected void MovementOver()
    {
        m_enemyState = EnemyState.EnemyState_Attacking;
        AimWeaponAt(m_target.gameObject.GetComponentInChildren<Renderer>().bounds.center);
    }
    #endregion

    #region Attack related
    protected void Fire()
    {
        FireWeapon(0);
        m_currentTimeToAttack = m_timeToAttack;
    }
    #endregion

    #region Updates
    protected override void Update()
    {
        if (!m_destroyed)
        {
            base.Update();
            switch (m_enemyState)
            {
                case EnemyState.EnemyState_Sleep:
                    if (m_attackPosition.HasValue)
                    {
                        StartMovement();
                    }
                    break;
                case EnemyState.EnemyState_Moving:
                    if (IsPathCompleted())
                    {
                        MovementOver();
                    }
                    break;
                case EnemyState.EnemyState_Attacking:
                    m_currentTimeToAttack -= Time.deltaTime;
                    if (m_currentTimeToAttack <= 0)
                    {
                        Fire();
                    }
                    break;
                default:
                    break;
            }
        }
    }
    #endregion
}
