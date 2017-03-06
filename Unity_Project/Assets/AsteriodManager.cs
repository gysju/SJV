using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteriodManager : MonoBehaviour 
{
	[Header("Asteriode")]
	public GameObject Asteroide;

	[Header("Spawn")]
	public int MaxNumberOfAsteriods = 25;

	[Space(5)]
	[Range(0.0f, 200.0f)]
	public float Force = 150.0f;
	[Range(0.0f, 100.0f)]
	public float Torque = 50.0f;

	[Space(5)]
	[Range(0.0f,500.0f)]
	public float MaxRangeSpawnY = 5;
	[Range(0.0f,500.0f)]
	public float MaxRangeSpawnZ = 5;

	[Space(5)]
	public Vector2 RandomScaleIntensity = new Vector2(0.5f,2.0f);

	[Space(5)]
	[Header("Time")]
	public float TimeBetweenAsteriods = 1.0f;
	[Range(0.0f,1.0f)]
	public float TimeRandom = 0.5f;
	[Range(0.1f,1.0f)]
	public float TimeRandomItensity = 0.5f;

	private List<GameObject> Asteriods = new List<GameObject>();

	private Transform Spawner;
	private float time = 0;
	private GameObject Instance;

	void Start () 
	{
		Spawner = transform.FindChild("Spawn");
		 
		Instance = new GameObject();
		Instance.transform.name = "Instance";
		Instance.transform.parent = transform;
	}
	
	void Update () 
	{
		if (Asteroide != null && time > TimeBetweenAsteriods + (TimeRandom * Random.Range(0.0f, TimeRandomItensity)))
		{
			time = 0;
			SpawnAsteroide();
		}
		time += Time.deltaTime;
	}

	void SpawnAsteroide()
	{
		if( Asteriods.Count < MaxNumberOfAsteriods)
		{
			GameObject ast = Instantiate (Asteroide, Vector3.zero, Quaternion.identity);
			RandomPosition(ast);

			ast.transform.parent = Instance.transform;
			ast.transform.localScale *= Random.Range (RandomScaleIntensity.x, RandomScaleIntensity.y);
			Asteriods.Add(ast);
		}
	}

	void RandomPosition( GameObject obj)
	{
		obj.transform.position = Spawner.position + new Vector3 (0.0f, Random.Range (-MaxRangeSpawnY, MaxRangeSpawnY), Random.Range (-MaxRangeSpawnZ, MaxRangeSpawnZ));

		Rigidbody rigid = obj.GetComponent<Rigidbody>();
		rigid.AddForce (-Spawner.right * ( Force * Random.Range(0.5f,1.0f)), ForceMode.Impulse);
		rigid.AddTorque (Random.onUnitSphere * ( Torque * Random.Range(0.5f,1.0f)), ForceMode.Impulse);
	}

	void OnTriggerEnter(Collider col)
	{
		RandomPosition(col.gameObject);
	}
}
