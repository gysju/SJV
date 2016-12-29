using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveHelper : MonoBehaviour {

	public static WaveHelper Instance;
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
			if (spawn.Unit == null)
				return;
			
			Mesh mesh = spawn.Unit.GetComponentInChildren<MeshFilter> ().sharedMesh;

			if (mesh == null)
				return;
			Gizmos.color = Color.green;
			Gizmos.DrawWireMesh (mesh, spawn.SpawnPosition, Quaternion.Euler( spawn.SpawnRotation));

			Gizmos.color = Color.red;
			Gizmos.DrawLine (spawn.SpawnPosition + new Vector3(0.0f,1.0f,0.0f), spawn.AttackPosition + new Vector3(0.0f,1.0f,0.0f));

			Gizmos.DrawWireMesh (mesh, spawn.AttackPosition, Quaternion.Euler( spawn.SpawnRotation ));
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
