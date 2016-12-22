using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Wave", menuName = "Databases/Waves", order = 1)]
public class WaveScriptableObject : ScriptableObject 
{
	[SerializeField]
	public class Info
	{
		public int PosX;
		public int PosY;

		public UnitType type;
	}

	public string ObjectName;
	public enum UnitType {UnitType_None = 0, UnitType_Tank, UnitType_Drone}

	public float timeBeforeNextWave = 10.0f;
	public bool waitPreviousWave = true;

	public List<Info> spawners = new List<Info>();
	public int SpawnSizeX = 0;
	public int SpawnSizeY = 0;
	public WaveManagerWindow.TemplateType SpawnerType;

	public List<Info> Destination = new List<Info>();
	public int DestinationSizeX = 0;
	public int DestinationSizeY = 0;
	public WaveManagerWindow.TemplateType DestinationType;
}
