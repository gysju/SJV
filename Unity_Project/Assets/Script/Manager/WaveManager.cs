using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour {
    
    public static WaveManager Instance { get; private set; }
    public float ZaTime = 60.0f;
    public AnimationCurve UnitByTime;
	public int maxUnitByTime = 1;

	private float time;
	private int CurrentNbrOfUnit = 0;

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
		Debug.Log (getMaxUnit());
	}

	public int getMaxUnit()
	{
		return Mathf.FloorToInt((UnitByTime.Evaluate(time / 60.0f) * maxUnitByTime));
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
