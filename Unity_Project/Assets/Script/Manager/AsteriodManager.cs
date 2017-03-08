using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteriodManager : MonoBehaviour 
{
	[Header("Asteriode")]
	public GameObject Asteroide;
	public Vector2 RandomScale = new Vector2();

	[Header("Spawn")]
	public int MaxNumberOfAsteriods = 25;

	[Space(5)]
	public Vector2 Torque = new Vector2();

	[Space(5)]

	public Vector2 Radius = new Vector2 ();

	[Header("Rotation")]
	[Range(0.0f, 3.0f)]
	public float FirstJointSpeedRotation = 5.0f;
	[Range(0.0f, 3.0f)]
	public float SecondeJointSpeedRotation = 5.0f;
	[Range(0.0f, 3.0f)]
	public float ThirdJointSpeedRotation = 5.0f;

	private List<GameObject> Asteriods = new List<GameObject>();

	private GameObject Instance;

	GameObject FirstJoint;
	GameObject SecondeJoint;
	GameObject ThirdJoint;

	void Start () 
	{		 
		Instance = new GameObject();
		Instance.transform.name = "Instance";
		Instance.transform.parent = transform;

		FirstJoint = new GameObject ();
		SecondeJoint = new GameObject ();
		ThirdJoint = new GameObject ();

		FirstJoint.transform.parent = Instance.transform;
		SecondeJoint.transform.parent = Instance.transform;
		ThirdJoint.transform.parent = Instance.transform;

		FirstJoint.name = "FirstJoin";
		SecondeJoint.name = "SecondeJoin";
		ThirdJoint.name = "ThirdJoin";

		for(int i = 0; i < MaxNumberOfAsteriods; i++)
		{
			SpawnAsteroide ();
		}
	}
	
	void Update () 
	{
		RotateAsteriods ();
	}

	void RotateAsteriods()
	{
		FirstJoint.transform.Rotate (Vector3.up, FirstJointSpeedRotation * Time.deltaTime);
		SecondeJoint.transform.Rotate (Vector3.up, SecondeJointSpeedRotation * Time.deltaTime);
		ThirdJoint.transform.Rotate (Vector3.up, ThirdJointSpeedRotation * Time.deltaTime);
	}

	void SpawnAsteroide()
	{
		GameObject ast = Instantiate(Asteroide, Vector3.zero, Quaternion.identity, Instance.transform.GetChild(Random.Range(0,3)));
		RandomPosition(ast);

		ast.transform.localScale *= Random.Range (RandomScale.x, RandomScale.y);
		Asteriods.Add(ast);
	}

	void RandomPosition( GameObject obj)
	{
		Vector3 RandomDir = Random.onUnitSphere;
		obj.transform.position = new Vector3 (	RandomDir.x * Random.Range (Radius.x, Radius.y), 
												(Mathf.Abs( RandomDir.y )) * Random.Range (Radius.x, Radius.y), 
												RandomDir.z * Random.Range (Radius.x, Radius.y));


		Rigidbody rigid = obj.GetComponent<Rigidbody>();
		rigid.AddTorque (Random.onUnitSphere * ( Random.Range(Torque.x, Torque.y)), ForceMode.Impulse);
	}
}
