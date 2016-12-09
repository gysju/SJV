using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IACommander : MonoBehaviour
{
    public Unit m_allyBaseCenter;
    public Unit m_enemyBaseCenter;

    protected Player m_player;

    public List<Capture_point> m_capturePoints = new List<Capture_point>();

    public Unit.UnitFaction m_faction;

    void Start ()
    {
        m_player = FindObjectOfType<Player>();
	}

    float PathLength(NavMeshPath path)
    {
        if (path.corners.Length < 2)
            return 0;

        Vector3 previousCorner = path.corners[0];
        float lengthSoFar = 0.0F;
        int i = 1;
        while (i < path.corners.Length)
        {
            Vector3 currentCorner = path.corners[i];
            lengthSoFar += Vector3.Distance(previousCorner, currentCorner);
            previousCorner = currentCorner;
            i++;
        }
        return lengthSoFar;
    }

    protected void SearchPlayerLastPosition(IA unitAskingOrder)
    {
        unitAskingOrder.GiverDestroyOrder(m_player);
    }

    protected void CaptureClosestPoint(IA unitAskingOrder)
    {
        Vector3 unitPosition = unitAskingOrder.transform.position;
        float smallestLength = float.MaxValue;
        Capture_point order = null;
        foreach (Capture_point capturePoint in m_capturePoints)
        {
            if (!capturePoint.IsSameFaction(m_faction))
            {
                NavMeshPath possiblePath = new NavMeshPath();
                if (NavMesh.CalculatePath(unitPosition, capturePoint.transform.position, 1, possiblePath))
                {
                    float possiblePathLength = PathLength(possiblePath);
                    if (possiblePathLength < smallestLength)
                    {
                        order = capturePoint;
                        smallestLength = possiblePathLength;
                    }
                }
            }
        }

        if (order) unitAskingOrder.GiveCaptureOrder(order);
        else
        {
            switch (m_faction)
            {
                case Unit.UnitFaction.Ally:
                    if (m_enemyBaseCenter.IsDestroyed())
                        unitAskingOrder.CancelMoveOrder();
                    else
                        unitAskingOrder.GiverDestroyOrder(m_enemyBaseCenter);
                    break;
                case Unit.UnitFaction.Neutral:
                    break;
                case Unit.UnitFaction.Enemy:
                    if (m_allyBaseCenter.IsDestroyed())
                        unitAskingOrder.CancelMoveOrder();
                    else
                        unitAskingOrder.GiverDestroyOrder(m_allyBaseCenter);
                    break;
                default:
                    break;
            }
        }
    }

    public void AskOrder(IA unitAskingOrder)
    {
        if (unitAskingOrder.GetComponent<Unit>() is AirUnit)
            SearchPlayerLastPosition(unitAskingOrder);
        else if(unitAskingOrder.GetComponent<Unit>() is HoverTank)
            CaptureClosestPoint(unitAskingOrder);
    }

	public void ReactionDroneSquadron()
	{
		
	}

	void Update ()
    {
	    
	}
}
