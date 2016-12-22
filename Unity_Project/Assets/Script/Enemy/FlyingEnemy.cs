using UnityEngine;
using System.Collections;

public class FlyingEnemy : BaseEnemy
{
    [Header("Mobility")]
    public float m_maxSpeed = 2f;
    public float m_acceleration = 8f;

    #region Initialization
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        if (m_attackPosition.HasValue) StartMovement();
    }

    public override void ResetUnit(Vector3 spawn, Vector3 movementTarget, Transform target)
    {
        base.ResetUnit(spawn, movementTarget, target);
    }
    #endregion

    #region Movement Related
    
    protected bool IsPathCompleted()
    {
        return (Vector3.Distance(m_transform.position, m_attackPosition.Value) < 1f);
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
