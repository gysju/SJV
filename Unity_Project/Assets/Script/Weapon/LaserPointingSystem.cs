using UnityEngine;
using System.Collections;

public class LaserPointingSystem : MonoBehaviour {

	public float MinimalDistance = 1.0f;
	private LineRenderer lineRenderer;
	private RaycastHit hit;

	void Start () 
	{
		lineRenderer = GetComponent<LineRenderer> ();
	}
	
	void Update () 
	{
		if(Physics.Raycast( transform.position, transform.forward, out hit) )
		{
			lineRenderer.SetPosition (1, Vector3.forward * hit.distance);
		}
		else
		{
			lineRenderer.SetPosition (1, Vector3.forward * MinimalDistance);
		}
	}
}
