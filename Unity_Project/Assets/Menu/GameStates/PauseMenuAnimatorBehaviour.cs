using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class PauseMenuAnimatorBehaviour : GameStateBaseAnimatorBehaviour
{
	public string SceneToRestart;

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        CanvasManager.Get.eState_Menu = CanvasManager.EState_Menu.EState_Menu_Pause;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (GetMenu().bOnClick && !animator.IsInTransition(layerIndex))
        {
            FadeToBlack();
            GetMenu().bOnClick = false;

            SetInteractableButtonValue(false);
        }
        else if (fadeToBlackIsFinish() && !animator.IsInTransition(layerIndex))
        { 
            animator.SetTrigger(GetMenu().NextState);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);

		if ( GetMenu().NextState == "Restart")
			SceneManager.LoadSceneAsync(SceneToRestart);
    }
}
