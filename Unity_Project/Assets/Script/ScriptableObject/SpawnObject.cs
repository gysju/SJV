using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnObject : ScriptableObject
{
	public BaseEnemy Unit;

	[Header("Pos info")]
	public Vector3 SpawnPosition;
	public Vector3 SpawnRotation;	
	public Vector3 AttackPosition;

}
