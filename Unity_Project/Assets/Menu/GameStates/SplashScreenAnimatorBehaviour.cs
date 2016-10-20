using UnityEngine;
using System.Collections;

public class SplashScreenAnimatorBehaviour : GameStateBaseAnimatorBehaviour {

	//OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
	}
}
