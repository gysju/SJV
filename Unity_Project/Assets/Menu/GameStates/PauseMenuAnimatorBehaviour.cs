using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class PauseMenuAnimatorBehaviour : GameStateBaseAnimatorBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        CanvasManager.Get.eState_Menu = CanvasManager.EState_Menu.EState_Menu_Pause;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
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

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);

		if ( GetMenu().NextState == "Restart")
            SceneManager.LoadSceneAsync("scene");
    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}
