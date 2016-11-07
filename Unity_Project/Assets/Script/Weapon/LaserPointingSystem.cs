using UnityEngine;
using System.Collections;

public class LaserPointingSystem : MonoBehaviour {

	public float MinimalDistance = 1.0f;
	private LineRenderer lineRenderer;
	private RaycastHit hit;
	private MoveController move;
	void Start () 
	{
		lineRenderer = GetComponent<LineRenderer> ();
		move = GetComponent<MoveController> ();
	}
	
	void Update () 
	{
		if(Physics.Raycast( transform.position, transform.forward, out hit) && hit.collider.tag != "Player")
		{
			lineRenderer.SetPosition (1, Vector3.forward * hit.distance);
			if ( move != null ) move.lookAtHit = hit.point;
		}
		else
		{
			lineRenderer.SetPosition (1, Vector3.forward * MinimalDistance);
			move.lookAtHit = Vector3.zero;
		}
	}
}
