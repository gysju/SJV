using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;

public class StartMenuAnimatorBehaviour : GameStateBaseAnimatorBehaviour
{
	public int NextScene;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        CanvasManager.Get.eState_Menu = CanvasManager.EState_Menu.EState_Menu_Main;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
    {
		if ((GetMenu().bOnClick 
        #if UNITY_PS4 
            || CanvasManager.Get.CheckInputAnyPsMove(MoveController.MoveButton.MoveButton_Move) 
        #endif
        ) && !animator.IsInTransition(layerIndex))
        {
            FadeToBlack();
            GetMenu().bOnClick = false;

            SetInteractableButtonValue(false);
        }
        else if (fadeToBlackIsFinish() && !animator.IsInTransition(layerIndex))
            animator.SetTrigger(GetMenu().NextState);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
		if (GetMenu().NextState == "Playing")
        { 
			SceneManager.LoadSceneAsync(NextScene);
        
        }
    }
}
