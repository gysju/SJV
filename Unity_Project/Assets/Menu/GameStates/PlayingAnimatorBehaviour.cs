using UnityEngine;
using System.Collections;

public class PlayingAnimatorBehaviour : GameStateBaseAnimatorBehaviour
{
    private bool IsDone = false;
    public bool DeathOrFinish = false;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        CanvasManager.Get.eState_Menu = CanvasManager.EState_Menu.EState_Menu_InGame;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if ((Input.GetButtonDown("Start") 
            || ( CanvasManager.Get.eState_Menu == CanvasManager.EState_Menu.EState_Menu_Death 
            || CanvasManager.Get.eState_Menu == CanvasManager.EState_Menu.EState_Menu_EndGame) 
            && !IsDone && !animator.IsInTransition(layerIndex)) )
        {
            FadeToBlack();
            IsDone = true;
        }
        else if (fadeToBlackIsFinish() && !animator.IsInTransition(layerIndex))
            animator.SetTrigger("PauseMenu");
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        IsDone = false;
        DeathOrFinish = false;
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
