using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour {
    
    public static WaveManager Instance { get; private set; }

	[SerializeField]
	public struct Wave 
	{
		public float WaveTime;
		public AnimationCurve TankByTime;
		public int maxTankByTime;
		public AnimationCurve DroneByTime;
		public int maxDroneByTime;
	}
	[SerializeField]
	public List<Wave> Waves = new List<Wave> ();

    private float ZaTime = 0.0f;
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
	}

	public int getMaxUnit()
	{
		return 0;//return Mathf.FloorToInt((UnitByTime.Evaluate(time / 60.0f) * maxUnitByTime));
	}

	public bool WaveIsOver()
	{
		if(ZaTime < time)
		{
			return true;
		}
		return false;
	}
}
