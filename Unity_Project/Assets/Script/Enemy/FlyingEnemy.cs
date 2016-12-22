using UnityEngine;
using System.Collections;

public class FlyingEnemy : BaseEnemy
{
    [Header("Mobility")]
    public float m_maxSpeed = 2f;
    public float m_acceleration = 8f;
    public float m_rotationSpeed = 1f;

    public LayerMask m_layerToDodge;

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

    public override void TestUnit()
    {
        ResetUnit(new Vector3(5f, 2f, 120f), new Vector3(5f, 10f, 25f), FindObjectOfType<Player>().transform);
    }
    #endregion

    #region Movement Related
    protected void TurnTowardTarget(Vector3 targetPosition)
    {
        Vector3 movementDirection = (targetPosition - m_transform.position).normalized;
        Vector3 rotationVector = movementDirection;
        rotationVector.y = 0;
        Quaternion rotation = Quaternion.LookRotation(movementDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * m_rotationSpeed);
    }

    protected void MovementUpdate()
    {
        Vector3 movementDirection = (m_attackPosition.Value - m_transform.position).normalized;
        RaycastHit hit;
        Physics.SphereCast(m_transform.position, 3f, movementDirection, out hit, m_maxSpeed/2f, m_layerToDodge);
        if (hit.transform) movementDirection = Vector3.up;
        m_transform.position += movementDirection * m_maxSpeed * Time.deltaTime;
        if (IsPathCompleted()) MovementOver();
    }

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
                    if (m_attackPosition.HasValue)
                    {
                        StartMovement();
                    }
                    break;
                case EnemyState.EnemyState_Moving:
                    TurnTowardTarget(m_attackPosition.Value);
                    MovementUpdate();
                    break;
                case EnemyState.EnemyState_Attacking:
                    //TurnTowardTarget(m_target.position);
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
