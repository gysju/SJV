using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class LaserPointingSystem : MonoBehaviour
{
    public float MinimalDistance = 0f;
    public float MaxRaycastDistance = 250f;
    protected float CurrentRaycastDistance = 0f;

    public float ChangeRaySpeed = 0.5f;

    protected Coroutine ChangeRay = null;

    [SerializeField]
    private LayerMask mask;

    public LaserPointingSystem OtherLaser;
    public EnemyBarPosition enemyHUD;
    [HideInInspector]
    public GameObject currentPointedObject = null;

    private LineRenderer lineRenderer;
    [HideInInspector]
    public bool raycastHit;
    public RaycastHit hit;

    private EventSystem eventSystem;

    private MoveController move;
    private Button buttonSelected;
    private Transform ThisTransform;

    void Start()
    {
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        lineRenderer = GetComponent<LineRenderer>();
        move = GetComponent<MoveController>();
        ThisTransform = transform;
        CurrentRaycastDistance = 0f;
    }

    public void SetCurrentRayDist(float dist)
    {
        CurrentRaycastDistance = dist;
    }

    IEnumerator StartLightRay()
    {
        float time = 0f;
        float currentRay = CurrentRaycastDistance;
        while (time < ChangeRaySpeed)
        {
            time += Time.deltaTime;
            CurrentRaycastDistance = Mathf.Lerp(currentRay, 5f, (time / ChangeRaySpeed));
            yield return null;
        }
        CurrentRaycastDistance = MaxRaycastDistance;
    }

    IEnumerator StartShutdownRay()
    {
        float time = 0f;
        float currentRay = (CurrentRaycastDistance > 5f) ? 5f : CurrentRaycastDistance;
        while (time < ChangeRaySpeed)
        {
            time += Time.deltaTime;
            CurrentRaycastDistance = Mathf.Lerp(currentRay, MinimalDistance, (time / ChangeRaySpeed));
            yield return null;
        }
        CurrentRaycastDistance = MinimalDistance;
    }

    public void LightRay()
    {
        if (ChangeRay != null)
            StopCoroutine(ChangeRay);
        ChangeRay = StartCoroutine(StartLightRay());
    }

    public void ShutdownRay()
    {
        if (ChangeRay != null)
            StopCoroutine(ChangeRay);
        ChangeRay = StartCoroutine(StartShutdownRay());
    }

    void Update()
    {
        CheckMask();
        InputUI();
        Vector3 lineEndPos = Vector3.forward * CurrentRaycastDistance;

        raycastHit = Physics.Raycast(ThisTransform.position, ThisTransform.forward, out hit, CurrentRaycastDistance, mask);
        if (raycastHit)
        {
            lineEndPos = Vector3.forward * hit.distance;
            
            detectionType(hit);

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
            buttonSelected = null;
            if (enemyHUD != null)
                enemyHUD.EraseEnemy();

            if (eventSystem != null && eventSystem.currentSelectedGameObject != null && OtherLaser != null && OtherLaser.buttonSelected == null)
            {
                eventSystem.SetSelectedGameObject(null);
            }
            else
            {
#if UNITY_PS4
                if (move != null)
                {
					move.lookAtHit = ThisTransform.position + ThisTransform.forward * 1000.0f;
                    if (Input.GetKeyUp(KeyCode.Keypad0) || move.GetButtonUp(MoveController.MoveButton.MoveButton_Move))
                    {
                        setLineColor(Color.white);
                    }
                    if (Input.GetKeyDown(KeyCode.Keypad0) || move.GetButtonDown(MoveController.MoveButton.MoveButton_Move))
                    {
                        setLineColor(Color.red);
                    }
                }
#else
                if (Input.GetKeyUp(KeyCode.Keypad0))
                {
                    setLineColor(Color.white);
                }
                if (Input.GetKeyDown(KeyCode.Keypad0))
                {
                    setLineColor(Color.red);
                }

#endif
            }
#if UNITY_PS4
            if (move != null)
				move.lookAtHit = ThisTransform.position + ThisTransform.forward * 10.0f;
#endif
        }
        lineRenderer.SetPosition(1, lineEndPos);
    }

    void InputUI()
    {
        if (buttonSelected != null && (move.GetButtonDown(MoveController.MoveButton.MoveButton_Trigger) || Input.GetButtonDown("Fire1")))
        {
            buttonSelected.onClick.Invoke();
            SoundManager.Instance.PlaySoundOnShot("mecha_button_press", buttonSelected.GetComponent<ButtonInteraction>().audioSource);
        }
    }

    void detectionType(RaycastHit hit)
    {
        buttonSelected = hit.collider.transform.GetComponent<Button>();
        if (buttonSelected != null
            && OtherLaser != null
            && OtherLaser.buttonSelected == null)
        {
            buttonSelected.Select();
#if UNITY_PS4
            ButtonInteraction but = buttonSelected.GetComponent<ButtonInteraction>();
            if ( but != null) but.Selected();
#endif
        }
    }

    void CheckMask()
    {
        if (CanvasManager.Get == null)
            return;

        if (CanvasManager.EState_Menu.EState_Menu_InGame == CanvasManager.Get.eState_Menu)
            mask = (1 << LayerMask.NameToLayer("Ground") | 1 << LayerMask.NameToLayer("Unit") | 1 << LayerMask.NameToLayer("Environment"));
        else
            mask = 1 << LayerMask.NameToLayer("UI");
    }

    public void setLineColor(Color col)
    {
        lineRenderer.startColor = col;
        lineRenderer.endColor = col;
    }
}
