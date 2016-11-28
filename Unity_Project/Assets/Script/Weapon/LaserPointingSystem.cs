using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class LaserPointingSystem : MonoBehaviour {

	public float MinimalDistance = 1.0f;

	[SerializeField]
	private LayerMask mask;
	private MoveController moveController;

    [SerializeField]
	public MoveController OtherMoveController;

	private LineRenderer lineRenderer;
	private RaycastHit hit;

	private EventSystem eventSystem;

#if UNITY_PS4
	private MoveController move;
#endif
	[SerializeField]
    private int count = 1;
	void Start () 
	{
		eventSystem =  GameObject.Find("EventSystem").GetComponent<EventSystem>();
		lineRenderer = GetComponent<LineRenderer> ();
		moveController = GetComponent<MoveController> ();
#if UNITY_PS4
		move = GetComponent<MoveController> ();
#endif
    }

    void Update () 
	{
		if (count >= 1) {
			if (Physics.Raycast (transform.position, transform.forward, out hit, 1000.0f, mask)) {
					lineRenderer.SetPosition (1, Vector3.forward * hit.distance);

				Button but = hit.transform.GetComponent<Button>() ;
				if (but != null && OtherMoveController.currentUIButtonSelected == null) 
				{
					but.Select();
					moveController.currentUIButtonSelected = but;
				}
#if UNITY_PS4
				if (move != null)
				{
					move.lookAtHit = hit.point;
				}
#endif
			} 
			else 
			{
				lineRenderer.SetPosition (1, Vector3.forward * MinimalDistance);
				moveController.currentUIButtonSelected = null;

				if (eventSystem != null && OtherMoveController.currentUIButtonSelected == null)
					eventSystem.SetSelectedGameObject(null);
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
