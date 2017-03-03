using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitManager : MonoBehaviour {

	public static HitManager Instance = null;

	public Hit DroneHit;
	public List<Hit> DroneHits = new List<Hit>();

	public Hit GroundHit;
	public List<Hit> GroundHits = new List<Hit>();

	public Hit TankHit;
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
