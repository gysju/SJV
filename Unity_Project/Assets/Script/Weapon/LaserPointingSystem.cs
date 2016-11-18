using UnityEngine;
using System.Collections;

public class LaserPointingSystem : MonoBehaviour {

	public float MinimalDistance = 1.0f;

	[SerializeField]
	private LayerMask mask;

	private LineRenderer lineRenderer;
	private RaycastHit hit;
	private MoveController move;

	private int count = 0;
	void Start () 
	{
		lineRenderer = GetComponent<LineRenderer> ();
		move = GetComponent<MoveController> ();
	}
	
	void Update () 
	{
		if (count == 3) {
			if (Physics.Raycast (transform.position, transform.forward, out hit, 1000.0f, mask)) {
				lineRenderer.SetPosition (1, Vector3.forward * hit.distance);
				#if UNITY_PS4
				if (move != null)
					move.lookAtHit = hit.point;
				#endif
			} else {
				lineRenderer.SetPosition (1, Vector3.forward * MinimalDistance);
				#if UNITY_PS4
				if (move != null)
					move.lookAtHit = transform.position + transform.forward * 1000.0f;
				#endif
			}	
			count = 0;
		} 
		else 
			count += 1;
	}
}
