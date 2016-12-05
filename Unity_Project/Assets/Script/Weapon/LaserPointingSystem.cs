using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class LaserPointingSystem : MonoBehaviour {

	public float MinimalDistance = 1.0f;

	[SerializeField]
	private LayerMask mask;

	public LaserPointingSystem OtherLaser;

	private LineRenderer lineRenderer;
	private RaycastHit hit;

	private EventSystem eventSystem;

	private MoveController move;
	private Button buttonSelected;
	[SerializeField] 
    private int count = 1; 

	void Start () 
	{
		eventSystem =  GameObject.Find("EventSystem").GetComponent<EventSystem>();
		lineRenderer = GetComponent<LineRenderer> ();
		move = GetComponent<MoveController> ();
    }

    void Update () 
	{
		InputUI(); 

		if (count >= 1)  
		{
			if (Physics.Raycast (transform.position, transform.forward, out hit, 1000.0f, mask))  
			{
				lineRenderer.SetPosition (1, Vector3.forward * hit.distance);

				buttonSelected = hit.transform.GetComponent<Button>() ;
				if (buttonSelected != null 
					&& OtherLaser != null 
					&& OtherLaser.buttonSelected == null) 
				{
					buttonSelected.Select();
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
				buttonSelected = null;

				if (eventSystem != null && OtherLaser != null && OtherLaser.buttonSelected == null) 
				{
					eventSystem.SetSelectedGameObject(null);
				}
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

	void InputUI()
	{
		if(buttonSelected != null && move.GetButtonDown(MoveController.MoveButton.MoveButton_Move))
		{
			buttonSelected.onClick.Invoke ();
		}
	}
}
