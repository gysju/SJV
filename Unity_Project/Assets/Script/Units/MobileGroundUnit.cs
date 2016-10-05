using UnityEngine;
using System.Collections;

[AddComponentMenu("MechaVR/Units/DEV/Ground Unit")]
[RequireComponent(typeof(NavMeshAgent))]
public class MobileGroundUnit : Unit
{
    public NavMeshAgent navMeshAgent { get; private set; }
    public Balise targetBalise { get; private set; }
    protected UnitPath path;
    protected bool followTheWay = true;

    protected override void Start()
    {
        base.Start();
        navMeshAgent = GetComponent<NavMeshAgent>();
        if (navMeshAgent == null)
            navMeshAgent = GetComponentInParent<NavMeshAgent>();
    }

    #region Movement Related

    protected void setDestination(Vector3 pos)
    {
        if (navMeshAgent.destination != pos)
            navMeshAgent.destination = pos;
    }

    protected void PauseNavMesh()
    {
        if (navMeshAgent.hasPath)
            navMeshAgent.Stop();
    }

    protected void ContinueNavMesh()
    {
        if (navMeshAgent.hasPath)
            navMeshAgent.Resume();
    }

    protected void MoveAlongPath(bool nextBalise)
    {
        if (navMeshAgent.destination != targetBalise.transform.position && navMeshAgent.remainingDistance > 0.01 && followTheWay == nextBalise)
        {
            setDestination(targetBalise.transform.position);
        }
        else
        {
            followTheWay = nextBalise;
            if (nextBalise)
            {
                targetBalise = path.NextStep(targetBalise);
                setDestination(targetBalise.transform.position);
            }
            else
            {
                targetBalise = path.PreviousStep(targetBalise);
                setDestination(targetBalise.transform.position);
            }
        }
    }

    protected void MoveToDir(Vector3 dir)
    {
        navMeshAgent.destination = transform.position + dir.normalized;
    }

    #endregion

    #region Updates
    protected override void Update()
    {
        base.Update();
    }
    #endregion
}
