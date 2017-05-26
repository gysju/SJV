using UnityEngine;
using System.Collections;

[AddComponentMenu("MechaVR/Enemies/AirEnemy")]
public class AirEnemy : BaseEnemy
{
    [Header("Mobility")]
    public float m_maxSpeed = 2f;
    protected float m_currentSpeed = 0f;
    public float m_acceleration = 8f;
    public float m_rotationSpeed = 1f;

    protected Vector3 m_movement;
    protected Vector3 test;

    public float m_minimumDistance = 4f;

    public LayerMask m_layerToDodge;
	private Rigidbody rigid;

    [HideInInspector]
	[Range(0.0f,200.0f)]
	public float ForceIntensity = 0;
    [HideInInspector]
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
        test = m_attackPosition.Value;
        if (m_attackPosition.HasValue) StartMovement();
    }

    public override void ResetUnit(Vector3 spawn, Vector3 movementTarget)
    {
        base.ResetUnit(spawn, movementTarget);
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

        // play death sound
        SoundManager.Instance.PlaySoundOnShot("mecha_placeholder_explosion", audioSource);

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
    protected void LongDistanceMovementUpdate(Vector3 movementTarget)
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
        m_movement += movementDirection * m_acceleration * Time.deltaTime;
        m_movement = Vector3.ClampMagnitude(m_movement, m_maxSpeed * Time.deltaTime);
        m_transform.position += m_movement;
        if (IsPathCompleted(movementTarget)) MovementOver();
    }

    protected void CloseDistanceMovementUpdate(Vector3 movementTarget)
    {
        Vector3 movementDirection = (movementTarget - m_transform.position).normalized;
        m_movement = Vector3.RotateTowards(m_movement, movementDirection, m_rotationSpeed * Time.deltaTime, 0f);

        if (Vector3.Dot(m_movement, movementDirection) >= 0f)
        {
            m_movement += movementDirection * m_acceleration * Time.deltaTime;
            m_movement = Vector3.ClampMagnitude(m_movement, m_maxSpeed * Time.deltaTime);
        }
        else
        {
            Brake();
        }

        RaycastHit hit;
        Physics.SphereCast(m_transform.position, 3f, m_movement, out hit, m_minimumDistance, m_layerToDodge);
        if (hit.transform)
        {
            m_attackPosition = (test) + Random.insideUnitSphere * 5f;
        }
        else
        {
            //if (m_transform.position.y > movementTarget.y)
            //{
            //    Physics.SphereCast(m_transform.position, 3f, -m_transform.up, out hit, m_minimumDistance, m_layerToDodge);
            //    if (!hit.transform)
            //        movementDirection += Vector3.down / 2f;
            //}

        }
        m_transform.position += m_movement;
        if (IsPathCompleted(movementTarget)) m_attackPosition = (test) + Random.insideUnitSphere * 5f;
    }

    protected void MoveToTarget()
    {
        CloseDistanceMovementUpdate(Camera.main.transform.position + (new Vector3(m_target.forward.x, 0f, m_target.forward.z) * 10f) + (Random.insideUnitSphere * 2));
    }

    protected void Brake()
    {
        m_movement = Vector3.SmoothDamp(m_movement, Vector3.zero, ref m_movement, 0.05f);
    }

    protected bool IsPathCompleted(Vector3 movementTarget)
    {
        return (Vector3.Distance(m_transform.position, movementTarget) < 1f);
    }

    protected void MovementOver()
    {
        m_enemyState = EnemyState.EnemyState_Attacking;
        //LaserOn();
    }
    #endregion

    #region Attack related
    protected void Fire()
    {
        PressWeaponTrigger(0);
        //m_currentTimeToAttack = m_timeToAttack;
    }

    protected void TurnTowardTarget(Vector3 targetPosition)
    {
        Vector3 rotationDirection = ((targetPosition) - m_transform.position).normalized;
        rotationDirection.y = 0f;
        //Vector3 rotationVector = movementDirection;
        //rotationVector.y = 0;
        //Quaternion rotation = Quaternion.LookRotation(movementDirection);
        //transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * m_rotationSpeed);
        m_transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(m_transform.forward, rotationDirection, m_rotationSpeed * Time.deltaTime, 0f));
    }
    #endregion

    protected override bool IsTargetInRange()
    {
        return Vector3.Distance(m_target.position, m_transform.position) <= m_maxAttackDistance;
    }

    protected override void AttackMode()
    {
        m_enemyState = EnemyState.EnemyState_Attacking;
    }

    protected override void ChaseMode()
    {
        m_enemyState = EnemyState.EnemyState_Moving;
    }

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
                    LongDistanceMovementUpdate(m_attackPosition.Value);
                    //CloseDistanceMovementUpdate(m_attackPosition.Value);
                    break;
                case EnemyState.EnemyState_Attacking:
                    TurnTowardTarget(m_target.position);
                    CloseDistanceMovementUpdate(m_attackPosition.Value );
                    //MoveToTarget();
                    AimWeaponAt(m_target.position);
                    //m_currentTimeToAttack -= Time.deltaTime;
                    //if (m_currentTimeToAttack <= 0)
                    //if (IsWeaponOnTarget())
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
