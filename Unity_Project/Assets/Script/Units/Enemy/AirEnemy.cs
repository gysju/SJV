﻿using UnityEngine;
using System.Collections;

[AddComponentMenu("MechaVR/Enemies/AirEnemy")]
public class AirEnemy : BaseEnemy
{
    [Header("Mobility")]
    public float m_maxSpeed = 2f;
    public float m_acceleration = 8f;
    public float m_rotationSpeed = 1f;

    public LayerMask m_layerToDodge;
	private Rigidbody rigid;

	[Range(0.0f,200.0f)]
	public float ForceIntensity = 0;
	[Range(0.0f,200.0f)]
	public float TorqueIntensity = 0;

    #region Initialization
    protected override void Awake()
    {
        base.Awake();
		rigid = GetComponent<Rigidbody> ();
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

	#region HitPoints Related
	/// <summary>A appeler à la mort de l'unité.</summary>
	protected override void StartDying()
	{
		m_attackPosition = null;
		m_target = null;
		m_enemyState = EnemyState.EnemyState_Sleep;
		base.StartDying();

		rigid.isKinematic = false;
		rigid.useGravity = true;

		Vector3 dir = ( PlayerInputs.Instance.transform.position - transform.position).normalized;
		rigid.AddForce (-dir * ForceIntensity, ForceMode.Impulse);
		rigid.AddTorque (-dir * TorqueIntensity, ForceMode.Impulse);
	}

	protected override void FinishDying()
	{
		base.FinishDying ();

		rigid.isKinematic = true;
		rigid.useGravity = false;

		rigid.angularVelocity = Vector3.zero;
		rigid.velocity = Vector3.zero;
	}
	#endregion

    #region Movement Related

    protected void MovementUpdate(Vector3 movementTarget)
    {
        Vector3 movementDirection = (movementTarget - m_transform.position).normalized;
        m_transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(m_transform.forward, movementDirection, m_rotationSpeed * Time.deltaTime, 0f));
        RaycastHit hit;
        Physics.SphereCast(m_transform.position, 3f, movementDirection, out hit, m_maxSpeed/2f, m_layerToDodge);
        if (hit.transform)
        {
            movementDirection = Vector3.up;
        }
        else
        {
            if (m_transform.position.y > movementTarget.y)
            {
                Physics.SphereCast(m_transform.position, 3f, -m_transform.up, out hit, m_maxSpeed / 2f, m_layerToDodge);
                if (!hit.transform)
                    movementDirection += Vector3.down /2f;
            }
            
        }
        m_transform.position += m_transform.forward * m_maxSpeed * Time.deltaTime;
        if (IsPathCompleted(movementTarget)) MovementOver();
    }

    protected bool IsPathCompleted(Vector3 movementTarget)
    {
        return (Vector3.Distance(m_transform.position, movementTarget) < 1f);
    }

    protected void MovementOver()
    {
        m_enemyState = EnemyState.EnemyState_Attacking;
        LaserOn();
    }
    #endregion

    #region Attack related
    protected void Fire()
    {
        PressWeaponTrigger(0);
        m_currentTimeToAttack = m_timeToAttack;
    }

    protected void TurnTowardTarget(Vector3 targetPosition)
    {
        Vector3 movementDirection = (targetPosition - m_transform.position).normalized;
        Vector3 rotationVector = movementDirection;
        rotationVector.y = 0;
        Quaternion rotation = Quaternion.LookRotation(movementDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * m_rotationSpeed);
    }

    protected void AttackMovementUpdate()
    {

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
                    MovementUpdate(m_attackPosition.Value);
                    break;
                case EnemyState.EnemyState_Attacking:
                    TurnTowardTarget(m_target.position);
                    AttackMovementUpdate();
                    AimWeaponAt(m_target.position + Random.insideUnitSphere * 2);
                    m_currentTimeToAttack -= Time.deltaTime;
                    if (m_currentTimeToAttack <= 0)
                    {
                        Fire();
                        //m_enemyState = EnemyState.EnemyState_Moving;
                        //m_attackPosition = (m_transform.position + Vector3.up) + Random.insideUnitSphere;
                    }
                    break;
                default:
                    break;
            }
        }
    }
    #endregion
}
