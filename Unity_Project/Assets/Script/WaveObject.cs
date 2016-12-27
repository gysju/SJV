using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Wave", menuName = "Databases/Waves", order = 1)]
public class WaveScriptableObject : ScriptableObject 
{
	[SerializeField]
	public class Spawn : ScriptableObject
	{
		public UnitType type;
		public Vector3 SpawnPosition;
		public Vector3 SpawnRotation;	
		public Vector3 AttackPosition;
	}

	public string ObjectName;
	public enum UnitType {UnitType_None = 0, UnitType_Tank, UnitType_Drone}

	public float timeBeforeNextWave = 10.0f;
	public bool waitPreviousWave = true;

	public List<Spawn> Spawns = new List<Spawn>();
}
