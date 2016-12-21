using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Wave", menuName = "Databases/Waves", order = 1)]
public class WaveScriptableObject : ScriptableObject {
	public float waveTime = 10.0f;
	//public List<Spawner> spawners = new List<Spawner>();
}
