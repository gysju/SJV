using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.Linq;

public class Player : Unit {

    private List<SixenseHand> hands;

    public enum MoveSystem { MoveSystem_type1, MoveSystem_type2, MoveSystem_type3 };
    public MoveSystem moveSystem = MoveSystem.MoveSystem_type1;

	public Vector3 destination = Vector3.zero;
	public float PlayerSpeed = 1.0f;

	public Vector2 XMinAndMax = Vector2.zero;
	public Vector2 YMinAndMax = Vector2.zero;

	protected override void Start ()
    {
        base.Start();
        hands = GetComponentsInChildren<SixenseHand>().ToList();
    }
	
	void Update ()
    {

		foreach(SixenseHand hand in hands)
		{
			if (hand.m_controller != null) 
			{
				if(hand.m_controller.GetButton(SixenseButtons.ONE))
				{
					PauseNavMesh ();
					orientationSystem (hand);
					return;
				}
				else
				{
					PointingSystem (hand);
				}
			}
		}
    }

	void Input()
	{

	}

	void orientationSystem( SixenseHand hand)
	{
		Vector3 dir = Vector3.zero;

		float x = hand.transform.rotation.x;
		float y = hand.transform.rotation.y;
		Debug.Log (hands [0].transform.rotation);

		const float NEUTRAL_Z = -0.3f;
		const float NEUTRAL_X = 0f;

		const float MAX_FORWARD = 0f;
		const float MIN_FORWARD = -0.2f;
		const float MAX_BACKWARD = -0.6f;
		const float MIN_BACKWARD = -0.4f;
		const float MAX_LEFT = -0.2f;
		const float MIN_LEFT = -0.1f;
		const float MAX_RIGHT = 0.2f;
		const float MIN_RIGHT = 0.1f;

		float zdir = 0;
		if (x > NEUTRAL_Z) //avant
			zdir = (Mathf.InverseLerp (MIN_FORWARD, MAX_FORWARD, x) * Time.deltaTime);
		if (x < NEUTRAL_Z) //arrière
			zdir = -(Mathf.InverseLerp (MIN_BACKWARD, MAX_BACKWARD, x) * Time.deltaTime);

		float xdir = 0;
		if (y < NEUTRAL_X) //gauche
			xdir = -(Mathf.InverseLerp (MIN_LEFT, MAX_LEFT, y) * Time.deltaTime);
		if (y > NEUTRAL_X) //droite
			xdir = (Mathf.InverseLerp (MIN_RIGHT, MAX_RIGHT, y) * Time.deltaTime);

			dir += new Vector3 (xdir, 0f, zdir);

		transform.Translate (dir);
	}

	void PointingSystem(SixenseHand hand)
    {
		if (hand.m_controller.GetButton(SixenseButtons.BUMPER))
		{
			RaycastHit hit;
			if (Physics.Raycast(hand.transform.position, hand.transform.forward, out hit))
			{
				LineRenderer line = hand.GetComponent<LineRenderer> ();
				line.SetPosition (0, transform.position);
				line.SetPosition (1, hit.point);
				destination = hit.point;
			}
		}

		if (destination != Vector3.zero && hand.m_controller.GetButtonUp(SixenseButtons.BUMPER))
        {
			navMeshAgent.Resume ();
			setDestination(destination);
			destination = Vector3.zero;
        } 
    }
}
