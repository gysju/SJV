using UnityEngine;
using System.Collections;

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
        base.Start();
        if(m_attackPosition.HasValue) m_navMeshAgent.SetDestination(m_attackPosition.Value);
    }

    public override void ResetUnit(Vector3 spawn, Vector3 movementTarget)
    {
        base.ResetUnit(spawn, movementTarget);
    }
    #endregion
}
