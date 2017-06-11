using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class MainMenuAnimatorBehaviour : GameStateBaseAnimatorBehaviour
{
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    	// just don't use the update from parent.
    }
}
