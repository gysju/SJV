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
		int mask = ( 1 << LayerMask.NameToLayer ("Ground")) | ( 1 << LayerMask.NameToLayer ("Unit"));
		if(Physics.Raycast( transform.position, transform.forward, out hit, 100.0f, mask))
		{
			lineRenderer.SetPosition (1, Vector3.forward * hit.distance);
			#if UNITY_PS4
			if ( move != null ) move.lookAtHit = hit.point;
			#endif
		}
		else
		{
			lineRenderer.SetPosition (1, Vector3.forward * MinimalDistance);
			#if UNITY_PS4
			if ( move != null ) move.lookAtHit = transform.position + transform.forward * 200.0f;
			#endif
		}
	}
}
