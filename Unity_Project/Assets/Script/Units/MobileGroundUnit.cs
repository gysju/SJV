using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("MechaVR/Units/DEV/Ground Unit")]
[RequireComponent(typeof(NavMeshAgent))]
public class MobileGroundUnit : CombatUnit
{
    protected NavMeshAgent m_navMeshAgent;
    protected Vector3? m_destination = null;
    public Balise m_targetBalise { get; private set; }
    protected UnitPath m_path;
    protected bool m_followTheWay = true;
    
    [Header("Mobility")]
    public float m_maxSpeed = 2f;
    public float m_acceleration = 8f;
    public float m_rotationSpeed = 50f;

    #region Initialization
    protected override void Reset()
    {
        base.Reset();
    }
    
    protected override void Awake()
    {
        base.Awake();
        m_navMeshAgent = GetComponent<NavMeshAgent>();
        m_navMeshAgent.stoppingDistance = 0.5f;
        m_navMeshAgent.speed = m_maxSpeed;
        m_navMeshAgent.acceleration = m_acceleration;
        m_navMeshAgent.angularSpeed = m_rotationSpeed;
        DisableNavMeshAgent();
    }

    protected override void Start()
    {
        base.Start();
    }

    public override void ResetUnit()
    {
        base.ResetUnit();
    }
    #endregion

    #region Hit Points Related
    protected override void StartDying()
    {
        base.StartDying();

        EnableNavMeshAgent();
        CancelPath();

        foreach (Capture_point capturePoint in m_currentlyCapturing)
        {
            capturePoint.CapturingUnitDestroyed(this);
        }
    }

    protected override void FinishDying()
    {
        base.FinishDying();
    }
    #endregion

    #region Movement Related
    private void EnableNavMeshAgent()
    {
        if (m_navMeshObstacle.enabled)
        {
            m_navMeshObstacle.enabled = false;
            m_navMeshAgent.enabled = true;
        }
    }

    private void DisableNavMeshAgent()
    {
        if (m_navMeshAgent.enabled)
        {
            m_navMeshAgent.enabled = false;
            m_navMeshObstacle.enabled = true;
        }
    }

    public void CancelPath()
    {
        if (m_navMeshAgent.enabled)
        {
            m_navMeshAgent.ResetPath();

            DisableNavMeshAgent();
        }
    }

    public bool CompareDestination(Vector3 otherDestination)
    {
        return (m_navMeshAgent.destination == otherDestination);
    }

    public void SetDestination(Vector3 newDestination)
    {
        EnableNavMeshAgent();
        NavMeshHit hit;
        //if (NavMesh.SamplePosition(newDestination, out hit, 5.0f, NavMesh.AllAreas))
        {
			//m_destination = hit.position;
            m_navMeshAgent.SetDestination(newDestination/* m_destination.Value*/);
        }
        //else
        //{
        //    CancelPath();
        //    m_destination = null;
        //}
    }

    public void PausePath()
    {
        if (m_navMeshAgent.isActiveAndEnabled)
        {
            m_navMeshAgent.Stop();

            DisableNavMeshAgent();
        }
    }

    public void ResumePath()
    {
        EnableNavMeshAgent();

		if (m_navMeshAgent.hasPath)
			m_navMeshAgent.Resume ();
        else
            DisableNavMeshAgent();
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

    protected void CheckPath()
    {
        if (IsPathCompleted())
        {
            m_destination = null;
            DisableNavMeshAgent();
        }
    }

    protected void MoveAlongPatrol(bool nextBalise)
    {
        if (m_navMeshAgent.destination != m_targetBalise.transform.position && m_navMeshAgent.remainingDistance > 0.01 && m_followTheWay == nextBalise)
        {
            SetDestination(m_targetBalise.transform.position);
        }
        else
        {
            m_followTheWay = nextBalise;
            if (nextBalise)
            {
                m_targetBalise = m_path.NextStep(m_targetBalise);
                SetDestination(m_targetBalise.transform.position);
            }
            else
            {
                m_targetBalise = m_path.PreviousStep(m_targetBalise);
                SetDestination(m_targetBalise.transform.position);
            }
        }
    }

    protected void MoveToDir(Vector3 dir)
    {
        EnableNavMeshAgent();
        m_navMeshAgent.Move(dir * m_maxSpeed * Time.deltaTime);
    }

    #endregion

    #region Updates
    protected override void Update()
    {
        if (!m_destroyed)
        {
            base.Update();
            if (m_navMeshAgent.isActiveAndEnabled)
                CheckPath();
        }
    }
    #endregion
}
