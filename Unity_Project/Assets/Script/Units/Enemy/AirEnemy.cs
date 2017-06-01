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
    
    protected Transform m_attackPosition = null;
    protected Vector3? m_evasivePosition = null;

    protected Vector3 m_movement;

    public float m_securityDistance = 4f;

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

    //protected override void Start()
    //{
    //    base.Start();
    //    if (m_attackPosition.HasValue) StartMovement();
    //}

    protected override void ChooseTargets()
    {
        m_weaponsTarget = m_player.m_targetPoint;
        m_attackPosition = m_player.m_attackZoneManager.betterZone.m_transform;
        m_evasivePosition = null;
    }
    #endregion

    #region HitPoints Related
    /// <summary>A appeler à la mort de l'unité.</summary>
    protected override void StartDying()
	{
		m_attackPosition = null;
        m_evasivePosition = null;

        rigid.isKinematic = false;
		rigid.useGravity = true;

		Vector3 dir = ( PlayerInputs.Instance.transform.position - transform.position).normalized;
		rigid.AddForce (-dir * ForceIntensity, ForceMode.Impulse);
		rigid.AddTorque (-dir * TorqueIntensity, ForceMode.Impulse);

        // play death sound
        SoundManager.Instance.PlaySoundOnShot("mecha_placeholder_explosion", audioSource);

		base.StartDying();
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
    protected void PlaneMovementUpdate(Vector3 movementTarget)
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

    protected void ChooseEvasivePosition()
    {
        m_attackPosition = m_player.m_attackZoneManager.ClosestBetterZone(m_transform.position);
        m_evasivePosition = (m_attackPosition.position) + Random.insideUnitSphere * 5f;
    }

    protected void EvasiveManeuvers(Vector3 movementTarget)
    {
        if (!m_evasivePosition.HasValue) ChooseEvasivePosition();

        Vector3 movementDirection = (m_evasivePosition.Value - m_transform.position).normalized;
        m_movement = Vector3.RotateTowards(m_movement, movementDirection, m_rotationSpeed * Time.deltaTime, 0f);

        if (Vector3.Dot(m_movement, movementDirection) >= 0f)
        {
            m_movement += movementDirection * m_acceleration/* * Time.deltaTime*/;
            m_movement = Vector3.ClampMagnitude(m_movement, m_maxSpeed /** Time.deltaTime*/);
        }
        else
        {
            Brake();
        }

        RaycastHit hit;
        Physics.SphereCast(m_transform.position, 3f, m_movement, out hit, m_securityDistance, m_layerToDodge);
        if (hit.transform)
        {
            ChooseEvasivePosition();
        }
        else
        {
            
        }
        m_transform.position += m_movement * Time.deltaTime;

        if (IsPathCompleted(m_evasivePosition.Value)) ChooseEvasivePosition();
    }

    protected void MoveToTarget()
    {
        EvasiveManeuvers(Camera.main.transform.position + (new Vector3(m_weaponsTarget.forward.x, 0f, m_weaponsTarget.forward.z) * 10f) + (Random.insideUnitSphere * 2));
    }

    protected void Brake()
    {
        m_movement = Vector3.SmoothDamp(m_movement, Vector3.zero, ref m_movement, 0.05f);
    }

    protected bool IsPathCompleted(Vector3 movementTarget)
    {
        return (Vector3.Distance(m_transform.position, movementTarget) < m_securityDistance);
    }

    protected void MovementOver()
    {
        AttackMode();
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
        return Vector3.Distance(m_weaponsTarget.position, m_transform.position) <= m_maxAttackDistance;
    }

    protected override void AttackMode()
    {
        m_enemyState = EnemyState.EnemyState_Attacking;
    }

    protected override void ChaseMode()
    {
        ChooseTargets();
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
                    if (m_attackPosition)
                    {
                        StartMovement();
                    }
                    break;
                case EnemyState.EnemyState_Moving:
                    ChooseTargets();
                    if (!m_attackPosition)
                    {
                        m_enemyState = EnemyState.EnemyState_Sleep;
                    }
                    else
                    {
                        PlaneMovementUpdate(m_attackPosition.position);
                    }
                    break;
                case EnemyState.EnemyState_Attacking:
                    TurnTowardTarget(m_weaponsTarget.position);
                    EvasiveManeuvers(m_attackPosition.position);

                    AimWeaponAt(m_weaponsTarget.position);

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
