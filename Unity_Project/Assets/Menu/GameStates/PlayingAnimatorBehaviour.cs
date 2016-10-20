using UnityEngine;
using System.Collections;

public class PlayingAnimatorBehaviour : GameStateBaseAnimatorBehaviour
{
    private bool IsDone = false;
    public bool DeathOrFinish = false;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        CanvasManager.Get.eState_Menu = CanvasManager.EState_Menu.EState_Menu_InGame;
    }

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

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        IsDone = false;
        DeathOrFinish = false;
    }
}
