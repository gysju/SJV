using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ikLimb : MonoBehaviour {

	public Transform upperArm;
	public Transform forearm;
	public Transform hand;
	public Transform target;
	public Transform elbowTarget;

	public bool IsEnabled = true;
	public bool debug = true;

	private Quaternion upperArmStartRotation;
	private Quaternion forearmStartRotation;
	private Quaternion handStartRotation;

	private Vector3 targetRelativeStartPosition;
	private Vector3 elbowTargetRelativeStartPosition;


	private Transform ThisTransform;
	private Vector3 LastestTargetPosition;

	//public float transition = 1.0f;	

	void Start () {
		ThisTransform = transform;
		upperArmStartRotation = upperArm.rotation;
		forearmStartRotation = forearm.rotation;
		handStartRotation = hand.rotation;
		targetRelativeStartPosition = target.position - upperArm.position;
		elbowTargetRelativeStartPosition = elbowTarget.position - upperArm.position;
	}
	
	void LateUpdate () {
		if (!IsEnabled || LastestTargetPosition == target.transform.position){
			return;
		}
		CalculateIK();
	}

	void CalculateIK()
	{
		//Calculate ikAngle variable.
		float upperArmLength = Vector3.Distance(upperArm.position, forearm.position);
		float forearmLength = Vector3.Distance(forearm.position, hand.position);
		float armLength = upperArmLength + forearmLength;
		float hypotenuse = upperArmLength;
		float targetDistance = Vector3.Distance(upperArm.position, target.position);	
		
		targetDistance = Mathf.Min(targetDistance, armLength - 0.0001f); //Do not allow target distance be further away than the arm's length.

		//var adjacent : float = (targetDistance * hypotenuse) / armLength;
		float adjacent = (Mathf.Pow(hypotenuse,2) - Mathf.Pow(forearmLength,2) + Mathf.Pow(targetDistance,2))/(2*targetDistance);
		float ikAngle = Mathf.Acos(adjacent/hypotenuse) * Mathf.Rad2Deg;

		//Store pre-ik info.
		Vector3 targetPosition  = target.position;
		Vector3 elbowTargetPosition  = elbowTarget.position;

		Transform upperArmParent = upperArm.parent;
		Transform forearmParent = forearm.parent;
		Transform handParent = hand.parent; 

		Vector3 upperArmScale = upperArm.localScale;
		Vector3 forearmScale = forearm.localScale;
		Vector3 handScale = hand.localScale;

		Vector3 upperArmLocalPosition = upperArm.localPosition;
		Vector3 forearmLocalPosition = forearm.localPosition;
		Vector3 handLocalPosition = hand.localPosition;

		Quaternion upperArmRotation = upperArm.rotation;
		Quaternion forearmRotation = forearm.rotation;
		Quaternion handRotation = hand.rotation;

		//Reset arm.
		target.position = targetRelativeStartPosition + upperArm.position;
		elbowTarget.position = elbowTargetRelativeStartPosition + upperArm.position;
		upperArm.rotation = upperArmStartRotation;
		forearm.rotation = forearmStartRotation;
		hand.rotation = handStartRotation;

		//Work with temporaty game objects and align & parent them to the arm.
		ThisTransform.position = upperArm.position;
		ThisTransform.LookAt(targetPosition, elbowTargetPosition - ThisTransform.position);
		
		Transform upperArmAxisCorrection = new GameObject("upperArmAxisCorrection").transform;
		Transform forearmAxisCorrection = new GameObject("forearmAxisCorrection").transform;
		Transform handAxisCorrection = new GameObject("handAxisCorrection").transform;
		
		upperArmAxisCorrection.position = upperArm.position;
		upperArmAxisCorrection.LookAt(forearm.position, ThisTransform.root.up);
		upperArmAxisCorrection.parent = ThisTransform;
		upperArm.parent = upperArmAxisCorrection;

		forearmAxisCorrection.position = forearm.position;
		forearmAxisCorrection.LookAt(hand.position, ThisTransform.root.up);
		forearmAxisCorrection.parent = upperArmAxisCorrection;
		forearm.parent = forearmAxisCorrection;
		
		handAxisCorrection.position = hand.position;
		handAxisCorrection.parent = forearmAxisCorrection;
		hand.parent = handAxisCorrection;
		
		//Reset targets.
		target.position = targetPosition;
		elbowTarget.position = elbowTargetPosition;	
		
		//Apply rotation for temporary game objects.
		upperArmAxisCorrection.LookAt(target,elbowTarget.position - upperArmAxisCorrection.position);
		upperArmAxisCorrection.localRotation =  Quaternion.Euler(upperArmAxisCorrection.localRotation.eulerAngles - new Vector3(ikAngle,0,0)); /// before :upperArmAxisCorrection.transform.localRotation.eulerAngles.x -= ikAngle;
		
		forearmAxisCorrection.LookAt(target,elbowTarget.position - upperArmAxisCorrection.position);
		handAxisCorrection.rotation = target.rotation;
		
		//Restore limbs.
		upperArm.parent = upperArmParent;
		forearm.parent = forearmParent;
		hand.parent = handParent;
		upperArm.localScale = upperArmScale;
		forearm.localScale = forearmScale;
		hand.localScale = handScale;
		upperArm.localPosition = upperArmLocalPosition;
		forearm.localPosition = forearmLocalPosition;
		hand.localPosition = handLocalPosition;
		
		//Clean up temporary game objets.
		Destroy(upperArmAxisCorrection.gameObject);
		Destroy(forearmAxisCorrection.gameObject);
		Destroy(handAxisCorrection.gameObject);

		LastestTargetPosition = target.transform.position;
		//Transition.
		//transition = Mathf.Clamp01(transition);
		//upperArm.rotation = Quaternion.Slerp(upperArmRotation, upperArm.rotation, transition);
		//forearm.rotation = Quaternion.Slerp(forearmRotation, forearm.rotation, transition);
		//hand.rotation = Quaternion.Slerp(handRotation, hand.rotation, transition);

		//Debug.
		if (debug)
		{
			Debug.DrawLine(forearm.position, elbowTarget.position, Color.yellow);
			Debug.DrawLine(upperArm.position, target.position, Color.red);
		}
	}
}
