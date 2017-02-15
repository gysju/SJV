using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuicidalGroundEnemy : GroundEnemy
{

    #region Initialization
    protected override void Awake()
    {
        base.Awake();
        m_navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        m_navMeshAgent.stoppingDistance = 0.5f;
        m_navMeshAgent.speed = m_maxSpeed;
        m_navMeshAgent.acceleration = m_acceleration;
        m_navMeshAgent.angularSpeed = m_rotationSpeed;

        animator = GetComponent<Animator>();
    }

    protected override void Start()
    {
        m_target = FindObjectOfType<BaseMecha>().transform;
    }
    #endregion

    #region Movement Related
    public override void StartMovement()
    {
        if (m_navMeshAgent.SetDestination(m_target.position))
        {
            m_enemyState = EnemyState.EnemyState_Moving;
        }


        if (animator != false)
            animator.SetTrigger("Locomotion");
    }
    #endregion

    #region Attack related
    protected void Explode(BaseUnit target)
    {
        target.ReceiveDamages(1, 1);
        StartDying();
    }
    #endregion

    #region Updates
    protected override void Update()
    {
        if (!m_destroyed)
        {
            switch (m_enemyState)
            {
                case EnemyState.EnemyState_Sleep:
                    if (m_target)
                    {
                        StartMovement();
                    }
                    break;
                case EnemyState.EnemyState_Moving:
                    if (IsPathCompleted())
                    {
                        Explode(m_target.GetComponent<BaseUnit>());
                    }
                    break;
                case EnemyState.EnemyState_Attacking:
                    break;
                default:
                    break;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Transform collider = collision.transform;
        if (collider == m_target)
        {
            Explode(collider.GetComponent<BaseUnit>());
        }
    }

    #endregion
}
