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
		public AnimationCurve DroneByTime;
		public int CallNumber;
	}

	public List<Wave> Waves = new List<Wave> ();
	private float time;
	private int CurrentWaveIndex;

	private int MaxTankByCurrentCall;
	private int MaxDroneByCurrentCall;

	private float secondeTimer = 0.0f;
    void Start ()
    {
        if (Instance == null) 
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

		MaxTankByCurrentCall = Mathf.FloorToInt( Waves[CurrentWaveIndex].TankByTime.Evaluate(0) );
		MaxDroneByCurrentCall = Mathf.FloorToInt( Waves[CurrentWaveIndex].DroneByTime.Evaluate(0) );
	}
	
	void Update ()
    {
		time += Time.deltaTime;
		secondeTimer += Time.deltaTime;

		if (WaveIsOver()) 
		{
			time = 0;
			CurrentWaveIndex = ( CurrentWaveIndex + 1 ) % Waves.Count ;
		}
		else if (secondeTimer > (Waves[CurrentWaveIndex].WaveTime / Waves[CurrentWaveIndex].CallNumber))
		{
			float value = time / Waves [CurrentWaveIndex].WaveTime;
			MaxTankByCurrentCall = Mathf.FloorToInt( Waves[CurrentWaveIndex].TankByTime.Evaluate(value) ); 
			MaxDroneByCurrentCall = Mathf.FloorToInt( Waves[CurrentWaveIndex].DroneByTime.Evaluate(value) );
			secondeTimer = 0.0f;
		}
	}

	public int getCurrentMaxTank()
	{
		return MaxTankByCurrentCall;
	}

	public int getCurrentMaxDrone()
	{
		return MaxDroneByCurrentCall;
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
