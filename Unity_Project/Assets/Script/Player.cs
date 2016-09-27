using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.Linq;

public class Player : Unit {

    private List<SixenseHand> hands;

    public enum MoveSystem { MoveSystem_type1, MoveSystem_type2, MoveSystem_type3 };
    public MoveSystem moveSystem = MoveSystem.MoveSystem_type1;

	protected override void Start ()
    {
        base.Start();
        hands = GetComponentsInChildren<SixenseHand>().ToList();
    }
	
	void Update ()
    {
        switch (moveSystem)
        {
            case MoveSystem.MoveSystem_type1:
                PointingSystem();
                break;
            case MoveSystem.MoveSystem_type2:
                break;
            case MoveSystem.MoveSystem_type3:
                break;
        }
    }

    void PointingSystem()
    {
        foreach (SixenseHand hand in hands)
        {
            if (hand.m_controller != null && hand.m_controller.GetButtonDown(SixenseButtons.BUMPER))
            {
                RaycastHit hit;

                if (Physics.Raycast(hand.transform.position, hand.transform.forward, out hit))
                {
                    setDestination(hit.point);
                }
            }
        }
    }
}
