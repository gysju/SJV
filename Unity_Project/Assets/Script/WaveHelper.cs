using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveHelper : MonoBehaviour {

	public static WaveHelper Instance;
	public Mesh Tank;
	public Mesh Drone;

	static List<WaveScriptableObject.Spawn> ObjectToDraw = new List<WaveScriptableObject.Spawn>();

	void Start () 
	{
		if (WaveHelper.Instance != null)
			Destroy (this);
		
		Instance = this;	
	}
	
	void OnDrawGizmos()
	{
		foreach( WaveScriptableObject.Spawn spawn in ObjectToDraw)
		{
			Gizmos.color = Color.red;
			if (spawn.type == WaveScriptableObject.UnitType.UnitType_Tank)
				Gizmos.DrawWireMesh (Tank, spawn.SpawnPosition,  Quaternion.Euler( spawn.SpawnRotation ));
			else
				Gizmos.DrawWireMesh (Drone, spawn.SpawnPosition, Quaternion.Euler( spawn.SpawnRotation));

			Gizmos.color = Color.green;
			Gizmos.DrawLine (spawn.SpawnPosition + new Vector3(0.0f,1.0f,0.0f), spawn.AttackPosition + new Vector3(0.0f,1.0f,0.0f));

			if (spawn.type == WaveScriptableObject.UnitType.UnitType_Tank)
				Gizmos.DrawWireMesh (Tank, spawn.AttackPosition, Quaternion.Euler( spawn.SpawnRotation ));
			else
				Gizmos.DrawWireMesh (Drone, spawn.AttackPosition, Quaternion.Euler( spawn.SpawnRotation ));
		}
	}

	public static void DrawWave(List<WaveScriptableObject.Spawn> Spawns)
	{
		ObjectToDraw.AddRange (Spawns);
	}

	public static void ClearDraw()
	{
		ObjectToDraw.Clear ();
	}
}
