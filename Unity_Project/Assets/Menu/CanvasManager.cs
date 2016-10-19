using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour {

    public static CanvasManager Get { get; private set; }

    public enum EState_Menu { EState_Menu_Main, EState_Menu_Pause, EState_Menu_Death, EState_Menu_InGame, EState_Menu_EndGame };
    public EState_Menu eState_Menu = EState_Menu.EState_Menu_Main;

    private Animator animator;

    void Start() 
	{
        Get = this;
        animator = GetComponent<Animator>();
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
            case EState_Menu.EState_Menu_Death :
				Time.timeScale = 0.0f;
                SetTrigger("DeathMenu");
                break;
            case EState_Menu.EState_Menu_EndGame:
                Time.timeScale = 0.0f;
                SetTrigger("EndGame");
                break;
        }
 	}

    public void SetTriggerDeath()
    {
		eState_Menu = EState_Menu.EState_Menu_Death;
    }

    public void SetTrigger(string trigger)
    {
        animator.SetTrigger(trigger);
    }
}
