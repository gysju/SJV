using UnityEngine;
using System.Collections;
<<<<<<< HEAD
using System.Collections.Generic;
=======
>>>>>>> 20668847ab5bf9a09bc7d4fc2a1eb6e0b556c58b

public class WaveManager : MonoBehaviour {
    
    public static WaveManager Instance { get; private set; }
<<<<<<< HEAD
=======
    public float ZaTime = 60.0f;
    public AnimationCurve UnitByTime;
	public int maxUnitByTime = 1;

	private float time;
	private int CurrentNbrOfUnit = 0;
>>>>>>> 20668847ab5bf9a09bc7d4fc2a1eb6e0b556c58b

	[System.Serializable] 
	public struct Wave 
	{
		public float WaveTime;
		public AnimationCurve TankByTime;
		public AnimationCurve DroneByTime;
		public int CallNumber;
	}

	public List<Wave> Waves = new List<Wave> ();
	private int CurrentWaveIndex;

	private int MaxTankByCurrentCall;
	private int MaxDroneByCurrentCall;

	private float time = 0.0f;
	private float secondeTimer = 0.0f;
    void Start ()
    {
        if (Instance == null) 
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
<<<<<<< HEAD

		MaxTankByCurrentCall = Mathf.FloorToInt( Waves[CurrentWaveIndex].TankByTime.Evaluate(0) );
		MaxDroneByCurrentCall = Mathf.FloorToInt( Waves[CurrentWaveIndex].DroneByTime.Evaluate(0) );
=======
>>>>>>> 20668847ab5bf9a09bc7d4fc2a1eb6e0b556c58b
	}
	
	void Update ()
    {
		time += Time.deltaTime;
<<<<<<< HEAD
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

			BattleManager.Instance.setcurrentNbrTankSinceLastWave ( 0 );
			BattleManager.Instance.setcurrentNbrDroneSinceLastWave ( 0 );
		}
	}

	public int getCurrentMaxTank()
	{
		return MaxTankByCurrentCall;
	}

	public int getCurrentMaxDrone()
	{
		return MaxDroneByCurrentCall;
=======
		Debug.Log (getMaxUnit());
	}

	public int getMaxUnit()
	{
		return Mathf.FloorToInt((UnitByTime.Evaluate(time / 60.0f) * maxUnitByTime));
>>>>>>> 20668847ab5bf9a09bc7d4fc2a1eb6e0b556c58b
	}

	public bool WaveIsOver()
	{
<<<<<<< HEAD
		if(Waves[CurrentWaveIndex].WaveTime < time)
=======
		if(ZaTime < time)
>>>>>>> 20668847ab5bf9a09bc7d4fc2a1eb6e0b556c58b
		{
			return true;
		}
		return false;
	}
}
