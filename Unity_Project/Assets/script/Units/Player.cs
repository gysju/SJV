using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Player : Unit
{
    private Camera m_mainCamera;
    public Unit m_meca;

    public Weapon m_leftWeapon;
    public Weapon m_rightWeapon;

	[Header("Move system")]
	public float PlayerSpeed = 1.0f;

	public enum MoveSystem { MoveSystem_type1, MoveSystem_type2, MoveSystem_type3 };
	public MoveSystem moveSystem = MoveSystem.MoveSystem_type1;

	protected Vector3 destination = Vector3.zero;

	public Vector2 XMinAndMax = Vector2.zero;
	public Vector2 YMinAndMax = Vector2.zero;
    
	private List<SixenseHand> hands;

	protected override void Start()
    {
		base.Start();
        m_mainCamera = Camera.main;
		hands = GetComponentsInChildren<SixenseHand>().ToList();
    }

	void Update ()
    {
		aim ();
		shoot();
		move ();
    }

	void aim()
	{
		transform.Rotate(Vector3.up, Input.GetAxis("Mouse X"));
		m_mainCamera.transform.Rotate(Vector3.left, Input.GetAxis("Mouse Y"));
	}

	void shoot()
	{
		if (Input.GetMouseButtonDown(0) || hands[0].m_controller.GetButtonDown(SixenseButtons.TRIGGER))
			m_leftWeapon.TriggerPressed();
		if (Input.GetMouseButtonDown(1) || hands[1].m_controller.GetButtonDown(SixenseButtons.TRIGGER))
			m_rightWeapon.TriggerPressed();

		if (Input.GetMouseButtonUp(0) || hands[0].m_controller.GetButtonUp(SixenseButtons.TRIGGER))
			m_leftWeapon.TriggerReleased();
		if (Input.GetMouseButtonUp(1) || hands[1].m_controller.GetButtonUp(SixenseButtons.TRIGGER))
			m_rightWeapon.TriggerReleased();
	}

	void move()
	{
		bool test = false;
		foreach(SixenseHand hand in hands)
		{
			if (hand.m_controller != null) 
			{
				if(hand.m_controller.GetButton(SixenseButtons.ONE))
				{
					PauseNavMesh ();
					orientationSystem (hand);
					test = true;
				}
			}
		}
		foreach(SixenseHand hand in hands)
		{
			if (hand.m_controller != null && !test) 
			{
				PointingSystem (hand);
			}
		}
	}

	void orientationSystem( SixenseHand hand)
	{
		Vector3 dir = Vector3.zero;

		float x = hand.transform.localRotation.x;
		float y = hand.transform.localRotation.y;
		Debug.Log (hands [0].transform.localRotation);

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

		dir += new Vector3 (xdir, 0f, zdir) * PlayerSpeed;

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
