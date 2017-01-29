using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[CreateAssetMenu(fileName = "Wave", menuName = "Databases/Waves", order = 1)]
public class WaveObject : ScriptableObject 
{
	public string ObjectName = "";

	public float timeBeforeNextWave = 10.0f;
	public bool nextWaveWait = true;

	public List<SpawnObject> Spawns = new List<SpawnObject>();
}