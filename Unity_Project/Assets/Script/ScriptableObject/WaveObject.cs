using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[CreateAssetMenu(fileName = "Wave", menuName = "Databases/Waves", order = 1)]
public class WaveObject : ScriptableObject 
{
	[System.Serializable]
	public class Spawn : ScriptableObject
	{
		public GameObject Unit;
		public Vector3 SpawnPosition;
		public Vector3 SpawnRotation;	
		public Vector3 AttackPosition;
	}

	public string ObjectName = "";

	public float timeBeforeNextWave = 10.0f;
	public bool nextWaveWait = true;

	public List<Spawn> Spawns = new List<Spawn>();
}