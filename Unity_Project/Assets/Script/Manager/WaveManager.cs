using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour {
    
    public static WaveManager Instance { get; private set; }

	[System.Serializable] 
	public struct Wave 
	{
		public float WaveTime;
		public AnimationCurve TankByTime;
		public int maxTankByTime;
		public AnimationCurve DroneByTime;
		public int maxDroneByTime;
	}


	public List<Wave> Waves = new List<Wave> ();
	private int CurrentWaveIndex;

	private float time = 0.0f;

    void Start ()
    {
        if (Instance == null) 
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
	}
	
	void Update ()
    {
		time += Time.deltaTime;
		if (WaveIsOver()) 
		{
			time = 0;
			CurrentWaveIndex = ( CurrentWaveIndex + 1 ) % Waves.Count ;
		}
	}

	public int getCurrentMaxTank()
	{
		return Mathf.FloorToInt((Waves[CurrentWaveIndex].TankByTime.Evaluate(time / Waves[CurrentWaveIndex].WaveTime) * Waves[CurrentWaveIndex].maxTankByTime));
	}

	public int getCurrentMaxDrone()
	{
		return Mathf.FloorToInt((Waves[CurrentWaveIndex].DroneByTime.Evaluate(time / Waves[CurrentWaveIndex].WaveTime) * Waves[CurrentWaveIndex].maxTankByTime));
	}

	public bool WaveIsOver()
	{
		if(Waves[CurrentWaveIndex].WaveTime < time)
		{
			return true;
		}
		return false;
	}
}
