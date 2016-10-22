using UnityEngine;
using System.Collections;

[AddComponentMenu("MechaVR/Units/DEV/Ground Unit")]
[RequireComponent(typeof(NavMeshAgent))]
public class MobileGroundUnit : CombatUnit
{
    protected NavMeshAgent m_navMeshAgent;
    protected bool m_hasMoveOrder = false;
    public Balise m_targetBalise { get; private set; }
    protected UnitPath m_path;
    protected bool m_followTheWay = true;

    [ContextMenuItem("Set Destination", "SetDestinationTest")]
    [Header("Mobility")]
    public float m_maxSpeed = 2f;
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
        DisableNavMeshAgent();
    }

    protected override void Start()
    {
        base.Start();
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

    public void CancelMoveOrder()
    {
        m_navMeshAgent.ResetPath();

        DisableNavMeshAgent();
    }

    public void GiveMoveOrder(Vector3 newDestination)
    {
        EnableNavMeshAgent();
        NavMeshHit hit;
        if (NavMesh.SamplePosition(newDestination, out hit, 1.0f, NavMesh.AllAreas))
        {
            m_navMeshAgent.SetDestination(hit.position);
            m_hasMoveOrder = true;
        }
        else
        {
            CancelMoveOrder();
            m_hasMoveOrder = false;
        }
    }

    protected void PauseMoveOrder()
    {
        if (m_navMeshAgent.isActiveAndEnabled)
        {
            m_navMeshAgent.Stop();

            DisableNavMeshAgent();
        }
    }

    protected void ResumeMoveOrder()
    {
        EnableNavMeshAgent();

        if (m_navMeshAgent.hasPath)
            m_navMeshAgent.Resume();
        else
            DisableNavMeshAgent();
    }

    protected bool IsMoveOrderDone()
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

    protected void MoveAlongPath(bool nextBalise)
    {
        if (m_navMeshAgent.destination != m_targetBalise.transform.position && m_navMeshAgent.remainingDistance > 0.01 && m_followTheWay == nextBalise)
        {
            GiveMoveOrder(m_targetBalise.transform.position);
        }
        else
        {
            m_followTheWay = nextBalise;
            if (nextBalise)
            {
                m_targetBalise = m_path.NextStep(m_targetBalise);
                GiveMoveOrder(m_targetBalise.transform.position);
            }
            else
            {
                m_targetBalise = m_path.PreviousStep(m_targetBalise);
                GiveMoveOrder(m_targetBalise.transform.position);
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
        base.Update();
        if (m_navMeshAgent.isActiveAndEnabled)
            if (IsMoveOrderDone())
            {
                m_hasMoveOrder = false;
                DisableNavMeshAgent();
            }
    }

    void FixedUpdate()
    {
        
    }
    #endregion
}
