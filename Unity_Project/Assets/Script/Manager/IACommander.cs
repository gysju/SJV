using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IACommander : MonoBehaviour
{
    public Unit m_allyBaseCenter;
    public Unit m_enemyBaseCenter;

    public List<Capture_point> m_capturePoints = new List<Capture_point>();

    public Unit.UnitFaction m_faction;

    void Start ()
    {
	    
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

    public void AskOrder(HoverTank m_unitAskingOrder)
    {
        Vector3 unitPosition = m_unitAskingOrder.transform.position;
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

        if (order) m_unitAskingOrder.GiveCaptureOrder(order);
        else
        {
            switch (m_faction)
            {
                case Unit.UnitFaction.Ally:
                    if (m_enemyBaseCenter.IsDestroyed())
                        m_unitAskingOrder.CancelPath();
                    else
                        m_unitAskingOrder.GiverDestroyOrder(m_enemyBaseCenter);
                    break;
                case Unit.UnitFaction.Neutral:
                    break;
                case Unit.UnitFaction.Enemy:
                    break;
                default:
                    break;
            }
        }
    }

	void Update ()
    {
	    
	}
}
