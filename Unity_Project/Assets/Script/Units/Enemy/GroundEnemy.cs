using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof (NavMeshAgent))]
[AddComponentMenu("MechaVR/Enemies/GroundEnemy")]
public class GroundEnemy : BaseEnemy
{
    protected NavMeshAgent m_navMeshAgent;

    [Header("Mobility")]
    public float m_maxSpeed = 2f;
    public float m_acceleration = 8f;
    public float m_rotationSpeed = 50f;
    
	#region Initialization
    protected override void Awake()
    {
        base.Awake();
        m_navMeshAgent = GetComponent<NavMeshAgent>();
        m_navMeshAgent.stoppingDistance = 0.5f;
        m_navMeshAgent.speed = m_maxSpeed;
        m_navMeshAgent.acceleration = m_acceleration;
        m_navMeshAgent.angularSpeed = m_rotationSpeed;
    }

    protected override void Start()
    {
        if(m_attackPosition.HasValue) m_navMeshAgent.SetDestination(m_attackPosition.Value);
    }

    public override void ResetUnit(Vector3 spawn, Vector3 movementTarget, Transform target)
    {
        base.ResetUnit(spawn, movementTarget, target);
		m_navMeshAgent.enabled = true;
		if(m_attackPosition.HasValue) m_navMeshAgent.SetDestination(m_attackPosition.Value);
    }
    #endregion

	#region HitPoints Related

	/// <summary>A appeler à la mort de l'unité.</summary>
	protected override void StartDying()
	{
		CompleteStop();
		m_navMeshAgent.enabled = false;
        base.StartDying();
        // play death sound
        SoundManager.Instance.PlaySoundOnShot("mecha_placeholder_explosion", audioSource);
	}

    protected override void FinishDying()
    {
        base.FinishDying();
    }
	#endregion

    #region Movement Related
    public virtual void MoveTo(Vector3 target)
    {
        if (m_navMeshAgent.SetDestination(target))
        {
            StartMovement();
        }
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
        AimWeaponAt(m_target.position);
        LaserOn();
		if (m_animator) m_animator.SetTrigger("Idle");
    }

    protected void CompleteStop()
    {
        m_navMeshAgent.ResetPath();
        m_navMeshAgent.velocity = Vector3.zero;
    }
    #endregion

    #region Attack related
    protected void Fire()
    {
        PressWeaponTrigger(0);
        m_currentTimeToAttack = m_timeToAttack;
        // play attack sound
        SoundManager.Instance.PlaySoundOnShot("mecha_placeholder_tir_2", m_weapons[0].m_shotSound);
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
                        MoveTo(m_attackPosition.Value);
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
                    AimWeaponAt(m_target.position);
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
