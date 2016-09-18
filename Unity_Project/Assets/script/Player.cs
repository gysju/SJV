﻿using UnityEngine;
using System.Collections;

public class Player : Entity {

	[Range(1.0f,30.0f)]
	public float Speed = 2.0f;

	[Range(1.0f,30.0f)]
	public float rotationSpeed = 2.0f;

    public static Player Instance { get; private set; }

    private Transform pivot;
	private Arm leftArm;
	private Arm rightArm;

	private float leftAxisH;
	private float leftAxisV;
	private float rightAxisH;
	private float rightAxisV;

	protected override void Start () 
	{
        Instance = this;

        pivot = transform.FindChild("Pivot");
        rightArm = pivot.FindChild ("RightArm").GetComponent<Arm>();
		leftArm = pivot.FindChild("LeftArm").GetComponent<Arm>();
	}

	protected override void Update ()
	{
		leftAxisH = Input.GetAxis("Vertical") * Time.deltaTime;
		leftAxisV = Input.GetAxis("Horizontal") * Time.deltaTime;

		rightAxisH = Input.GetAxis("HorizontalR") * Time.deltaTime;
		rightAxisV = Input.GetAxis("VerticalR") * Time.deltaTime;

		Move ();
		Rotation ();
		Attack();
	}

	void Rotation()
	{
        pivot.rotation = Quaternion.Euler (new Vector3(rightAxisV * rotationSpeed + pivot.rotation.eulerAngles.x, 
                                                       rightAxisH * rotationSpeed + pivot.rotation.eulerAngles.y, 
                                                       0.0f));
	}

	void Move()
	{
		transform.position += pivot.forward * leftAxisH + pivot.right * leftAxisV;
	}

	void Attack()
	{
		if (Input.GetButtonDown ("Fire1")) 
		{
			rightArm.Shoot(this);
		}
		if (Input.GetButtonDown ("Fire2")) 
		{
			leftArm.Shoot(this);
		}
	}

    protected override void OnDeathEnter()
    {
        base.OnDeathEnter();
    }

    protected override void OnDeathUpdate()
    {

    }

    protected override void OnDeathExit()
    {

    }
}
