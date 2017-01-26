using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnObject : ScriptableObject
{
	public BaseEnemy Unit;

	[Header("Unit info")]

	public float TimeToAttack;
	public float Speed;
	public int Life;
	public int Damage;
	public int Armor;
	public Color emissiveColor;

	[Header("Pos info")]
	public Vector3 SpawnPosition;
	public Vector3 SpawnRotation;	
	public Vector3 AttackPosition;

}
