using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour {

    public static CanvasManager Get { get; private set; }

    public enum EState_Menu { EState_Menu_Main, EState_Menu_Pause, EState_Menu_Defeat, EState_Menu_InGame, EState_Menu_Victory };
    public EState_Menu eState_Menu = EState_Menu.EState_Menu_Main;

    private Animator animator;
	public TrackedDeviceMoveControllers trackedDeviceControllers;
	private MoveController[] moveControllers;

    void Start() 
	{
        Get = this;
        animator = GetComponent<Animator>();
        trackedDeviceControllers = GameObject.Find("TrackedDevices").GetComponent<TrackedDeviceMoveControllers>();
        moveControllers = trackedDeviceControllers.GetComponentsInChildren<MoveController> ();
	}

    void Update() 
	{
        switch (eState_Menu)
        {
            case EState_Menu.EState_Menu_Main :
				Time.timeScale = 1.0f;
                break;
            case EState_Menu.EState_Menu_InGame :
                Time.timeScale = 1.0f;
                break;
            case EState_Menu.EState_Menu_Pause :
                Time.timeScale = 0.0f;
                break;
			case EState_Menu.EState_Menu_Defeat :
				Time.timeScale = 0.0f;
                SetTrigger("Defeat");
                break;
			case EState_Menu.EState_Menu_Victory:
                Time.timeScale = 0.0f;
                SetTrigger("Victory");
                break;
        }
 	}

	public bool CheckInputAnyPsMove( MoveController.MoveButton moveButton)
	{
		for(int i = 0; i < moveControllers.Length; i++)
		{
			if (moveControllers [i].GetButtonDown (moveButton))
				return true;
		}
		return false;
	}

	public void SetTriggerDefeat()
    {
		eState_Menu = EState_Menu.EState_Menu_Defeat;
    }

    public void SetTrigger(string trigger)
    {
        animator.SetTrigger(trigger);
    }
}
