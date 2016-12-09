using UnityEngine;
using System.Collections;

public class OptionMenuAnimatorBehaviour : GameStateBaseAnimatorBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
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
            animator.SetTrigger(GetMenu().NextState);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerPrefs.Save();
        base.OnStateExit(animator, stateInfo, layerIndex);
    }

}
