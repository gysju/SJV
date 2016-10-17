﻿using UnityEngine;
using System.Collections;

[AddComponentMenu("MechaVR/Units/DEV/Ground Unit")]
[RequireComponent(typeof(NavMeshAgent))]
public class MobileGroundUnit : CombatUnit
{
    protected NavMeshAgent m_navMeshAgent;
    public Balise m_targetBalise { get; private set; }
    protected UnitPath m_path;
    protected bool m_followTheWay = true;

    [ContextMenuItem("Set Destination", "SetDestinationTest")]
    [Header("Mobility")]
    public float m_maxSpeed = 2f;
    
    protected override void Awake()
    {
        base.Awake();
        m_navMeshAgent = GetComponent<NavMeshAgent>();
        if (m_navMeshAgent == null)
            m_navMeshAgent = GetComponentInParent<NavMeshAgent>();
        EnableNavMeshAgent();
        m_navMeshAgent.stoppingDistance = 0.5f;
    }

    protected override void Start()
    {
        base.Start();
        DisableNavMeshAgent();
    }

    #region Movement Related
    protected void SetDestinationTest()
    {
        SetDestination(new Vector3(21f, 2f, 10f));
    }

    private void EnableNavMeshAgent()
    {
        m_navMeshObstacle.enabled = false;
        m_navMeshAgent.enabled = true;
    }

    private void DisableNavMeshAgent()
    {
        m_navMeshAgent.enabled = false;
        m_navMeshObstacle.enabled = true;
    }

    protected void SetDestination(Vector3 newDestination)
    {
        EnableNavMeshAgent();
        NavMeshHit hit;
        if (NavMesh.SamplePosition(newDestination, out hit, 1.0f, NavMesh.AllAreas))
        {
            m_navMeshAgent.SetDestination(hit.position);
        }
        else ClearNavMesh();
    }

    protected void ClearNavMesh()
    {
        if (m_navMeshAgent.hasPath)
            m_navMeshAgent.ResetPath();

        DisableNavMeshAgent();
    }

    protected void PauseNavMesh()
    {
        //DisableNavMeshAgent();

        if (m_navMeshAgent.hasPath)
            m_navMeshAgent.Stop();
    }

    protected void ContinueNavMesh()
    {
        EnableNavMeshAgent();

        if (m_navMeshAgent.hasPath)
            m_navMeshAgent.Resume();
    }

    protected bool CheckDestination()
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
        PauseNavMesh();
        m_navMeshAgent.Move(dir * m_maxSpeed * Time.deltaTime);
    }

    #endregion

    #region Updates
    protected override void Update()
    {
        base.Update();
    }

    void FixedUpdate()
    {
        if (m_navMeshAgent.isActiveAndEnabled)
            if (CheckDestination()) PauseNavMesh();
    }
    #endregion
}
