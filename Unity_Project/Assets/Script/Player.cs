using UnityEngine;
using System.Collections;

public class Player : Unit {

    public SixenseHand LeftHand;
    public SixenseHand RightHand;

	protected override void Start ()
    {
        base.Start();
    }
	
	void Update ()
    {
        PointingSystem();
    }

    void PointingSystem()
    {
        if (LeftHand.m_controller != null && LeftHand.m_controller.GetButtonDown(SixenseButtons.TRIGGER))
        {
            RaycastHit hit;

            if (Physics.Raycast(LeftHand.transform.position, LeftHand.transform.forward, out hit))
            {
                setDestination(hit.point);
            }
        }
        else if (RightHand.m_controller != null && RightHand.m_controller.GetButtonDown(SixenseButtons.TRIGGER))
        {
            RaycastHit hit;

            if (Physics.Raycast(RightHand.transform.position, RightHand.transform.forward, out hit))
            {
                setDestination(hit.point);
            }
        }
    }
}
