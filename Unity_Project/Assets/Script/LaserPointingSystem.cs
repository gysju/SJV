﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class LaserPointingSystem : MonoBehaviour {

	public float MinimalDistance = 1.0f;

	[SerializeField]
	private LayerMask mask;

	public LaserPointingSystem OtherLaser;
    public EnemyBarPosition enemyHUD;
    [HideInInspector]
    public GameObject currentPointedObject = null;

	private LineRenderer lineRenderer;
	private RaycastHit hit;

	private EventSystem eventSystem;

	private MoveController move;
	private Button buttonSelected;
    private int count; 
	private Transform ThisTransform;

    private bool positionIsCorrect = false;
    private NavMeshHit hitTeleport;

    void Start () 
	{
		eventSystem =  GameObject.Find("EventSystem").GetComponent<EventSystem>();
		lineRenderer = GetComponent<LineRenderer> ();
		move = GetComponent<MoveController> ();
		count = move.MoveIndex;
		ThisTransform = transform;
    }

    void Update () 
	{
		CheckMask ();
		InputUI(); 

		if (count >= 1)  
		{
			if (Physics.Raycast (ThisTransform.position, ThisTransform.forward, out hit, 250.0f, mask))  
			{
				lineRenderer.SetPosition (1, Vector3.forward * hit.distance);
				detectionType (hit);

                if (Input.GetKey(KeyCode.Keypad0))
                {
                    if ( NavMesh.SamplePosition(hit.point, out hitTeleport, 1.0f, NavMesh.GetAreaFromName("Walkable")))
                    {
                        positionIsCorrect = true;
                        lineRenderer.startColor = Color.green;
                        lineRenderer.endColor = Color.green;
                    }
                    else
                    {
                        positionIsCorrect = false;
                        lineRenderer.startColor = Color.red;
                        lineRenderer.endColor = Color.red;
                    }
                }
                else if (Input.GetKeyUp(KeyCode.Keypad0))
                {
                    if (positionIsCorrect)
                        BaseMecha.Instance.Teleport(hitTeleport.position);
                    lineRenderer.startColor = Color.white;
                    lineRenderer.endColor = Color.white;
                }
#if UNITY_PS4
				if (move != null)
				{
					move.lookAtHit = hit.point;
                    GameObject hitGameObject = hit.transform.gameObject;
                    bool unitLayer = (hitGameObject.layer == LayerMask.NameToLayer("Unit"));
                    if (hitGameObject != currentPointedObject && hitGameObject != OtherLaser.currentPointedObject && unitLayer)
					{
                        currentPointedObject = hitGameObject;
                        enemyHUD.SetEnemy(currentPointedObject);
                    }
                    else if (!unitLayer)
                    {
                        currentPointedObject = null;
						enemyHUD.EraseEnemy();
                    }
				}
#endif
            }
            else
            {
                currentPointedObject = null;
				if ( enemyHUD != null)
                	enemyHUD.EraseEnemy();
                lineRenderer.SetPosition (1, Vector3.forward * MinimalDistance);
				buttonSelected = null;

				if (eventSystem != null && OtherLaser != null && OtherLaser.buttonSelected == null) 
				{
					eventSystem.SetSelectedGameObject(null);
				}
				#if UNITY_PS4
				if (move != null)
					move.lookAtHit = ThisTransform.position + ThisTransform.forward * 1000.0f;
				#endif
			}
			count = 0;
		}
		else
			count += 1;
	}

	void InputUI()
	{
		if(buttonSelected != null && ( move.GetButtonDown(MoveController.MoveButton.MoveButton_Trigger) || Input.GetButtonDown("Fire1")))
		{
			buttonSelected.onClick.Invoke (); //a fixer, le GetButtonDown ne fonctionne qu'une seul fois ( par manette )
		}
	}

	void detectionType(RaycastHit hit)
	{
		buttonSelected = hit.collider.transform.GetComponent<Button>() ;
		if (buttonSelected != null 
			&& OtherLaser != null 
			&& OtherLaser.buttonSelected == null) 
		{
			buttonSelected.Select();
		}
	}

	void CheckMask()
	{
        if (CanvasManager.Get == null)
            return;

		if (CanvasManager.EState_Menu.EState_Menu_InGame == CanvasManager.Get.eState_Menu)
			mask = (1 << LayerMask.NameToLayer ("Ground") | 1 << LayerMask.NameToLayer ("Unit") | 1 << LayerMask.NameToLayer ("Environment"));
		else
			mask = 1 << LayerMask.NameToLayer ("UI");
	}
}
