using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitManager : MonoBehaviour {

	public static HitManager Instance = null;

	public Hit PlayerHit;
	[HideInInspector]
	public List<Hit> PlayerHits = new List<Hit>();

	[Space(10)]
	public Hit DroneHit;
	[HideInInspector]
	public List<Hit> DroneHits = new List<Hit>();

	[Space(10)]
	public Hit GroundHit;
	[HideInInspector]
	public List<Hit> GroundHits = new List<Hit>();

	[Space(10)]
	public Hit TankHit;
	[HideInInspector]
	public List<Hit> TankHits = new List<Hit>();

	void Start () {
		if (HitManager.Instance == null)
		{
			HitManager.Instance = this;
		}
		else
		{
			Destroy(this);
		}
	}
}
