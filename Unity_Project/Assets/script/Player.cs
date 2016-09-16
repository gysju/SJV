using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	[Range(1.0f,20.0f)]
	public float Speed = 2.0f;

	[Range(1.0f,20.0f)]
	public float rotationSpeed = 2.0f;

	private Arm leftArm;
	private Arm rightArm;

	private float leftAxisH;
	private float leftAxisV;
	private float rightAxisH;
	private float rightAxisV;

	void Start () 
	{
		rightArm = transform.FindChild ("RightArm").GetComponent<Arm>();
		leftArm = transform.FindChild("LeftArm").GetComponent<Arm>();
	}

	void Update ()
	{
		leftAxisH = Input.GetAxis("Vertical") * Time.deltaTime;
		leftAxisV = Input.GetAxis("Horizontal") * Time.deltaTime;

		rightAxisH = Input.GetAxis("HorizontalR") * Time.deltaTime;
		rightAxisV = Input.GetAxis("VerticalR") * Time.deltaTime;

		Move ();
		Rotation ();
		Shoot();
	}

	void Move()
	{
		transform.position += transform.forward * leftAxisH + transform.right * leftAxisV;
	}

	void Rotation()
	{
		transform.rotation = Quaternion.Euler (new Vector3(rightAxisV * rotationSpeed + transform.rotation.eulerAngles.x, rightAxisH * rotationSpeed + transform.rotation.eulerAngles.y, 0.0f));
	}

	void Shoot()
	{
		if (Input.GetButtonDown ("Fire1")) 
		{
			rightArm.Shoot();
		}
		if (Input.GetButtonDown ("Fire2")) 
		{
			leftArm.Shoot();
		}
	}
}
